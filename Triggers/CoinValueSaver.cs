using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using ml.CoinValue.Entities;
using ml.CoinValue.Models;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json.Linq;

namespace ml.CoinValue.Triggers
{
    public class CoinValueSaver
    {
        private readonly TelemetryClient _telemetryClient;

        public CoinValueSaver(TelemetryConfiguration telemetryConfiguration) =>
            _telemetryClient = new TelemetryClient(telemetryConfiguration);

        public const string TableName = "coins";
        private const string ApiEndpoint = "https://api.coinmarketcap.com/v1/ticker/";

        [FunctionName("CoinValueSaver")]
        public async Task Run([TimerTrigger("0 */1 * * * *")]TimerInfo myTimer,
            [Table(TableName, Connection = "AzureWebJobsStorage")]CloudTable table, ILogger log)
        {
            log.LogInformation($"CoinValueSaver executed at: {DateTime.Now}");

            // Get coin value as JSON
            var client = new HttpClient();
            var json = await client.GetStringAsync(ApiEndpoint);

            // Parsing prices retrived from the API endpoint
            var priceBtc = 0.0;
            var priceEth = 0.0;

            try
            {
                var array = JArray.Parse(json);

                var priceString = array.Children<JObject>()
                    .FirstOrDefault(c => c.Property("symbol").Value.ToString().ToLower() == CoinTrend.SymbolBtc)?
                    .Property("price_usd").Value.ToString();

                if (priceString != null)
                    double.TryParse(priceString, out priceBtc);

                priceString = array.Children<JObject>()
                    .FirstOrDefault(c => c.Property("symbol").Value.ToString().ToLower() == CoinTrend.SymbolEth)?
                    .Property("price_usd").Value.ToString();

                if (priceString != null)
                    double.TryParse(priceString, out priceEth);
            }
            catch
            {
                throw;
            }

            if (priceBtc < 0.1 || priceEth < 0.1)
            {
                log.LogInformation("Something went wrong");
                return;
            }

            // Generating table operations and model instances
            var coinBtc = new CoinEntity
            {
                RowKey = "rowBtc" + DateTime.Now.Ticks,
                PartitionKey = "partition",

                Symbol = CoinTrend.SymbolBtc,
                TimeOfReading = DateTime.Now,
                PriceUsd = priceBtc
            };
            var coinBtcOperation = TableOperation.Insert(coinBtc);

            var coinEth = new CoinEntity
            {
                RowKey = "rowEth" + DateTime.Now.Ticks,
                PartitionKey = "partition",

                Symbol = CoinTrend.SymbolEth,
                TimeOfReading = DateTime.Now,
                PriceUsd = priceEth
            };
            var coinEthOperation = TableOperation.Insert(coinEth);

            // Insert new values in table
            var batch = new TableBatchOperation { coinBtcOperation, coinEthOperation };
            await table.ExecuteBatchAsync(batch);

            // Send a custom metrics with the prices
            // to Azure Application Insights
            var btcPriceMetric = new MetricTelemetry("Bitcoin price in USD", priceBtc);
            _telemetryClient.TrackMetric(btcPriceMetric);
            var ethPriceMetric = new MetricTelemetry("Ethereum price in USD", priceEth);
            _telemetryClient.TrackMetric(ethPriceMetric);
        }
    }
}
