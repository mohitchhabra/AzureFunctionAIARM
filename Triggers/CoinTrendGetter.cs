using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.WindowsAzure.Storage.Table;
using ml.CoinValue.Entities;
using ml.CoinValue.Utils;
using ml.CoinValue.Models;
using System.Linq;

namespace ml.CoinValue.Triggers
{
    public static class CoinTrendGetter
    {
        private const int StandardCount = 10;

        [FunctionName("CoinTrendGetter")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", Route = "symbol")] HttpRequest req,
            [Table(CoinValueSaver.TableName, Connection = "AzureWebJobsStorage")] CloudTable table,
            ILogger log)
        {
            log.LogInformation($"CoinTrendGetter executed at: {DateTime.Now}");

            var coinsQuery = new TableQuery<CoinEntity>();
            var coinsSegment = await table.ExecuteQuerySegmentedAsync(coinsQuery, null);
            var coinsResult = coinsSegment.Results;

            var trend = new CoinTrend();

            var count = coinsResult.Count;
            if (count == 0)
            {
                // This is a error that might be interesting
                // because our database should contain entries at all time
                log.LogWarning("No coins found");
                return new StatusCodeResult(StatusCodes.Status404NotFound);
            }
            else
            {
                var selectedCount = (count < StandardCount) ? count : StandardCount;
                var coinValues = coinsResult
                    .Skip(count - selectedCount)
                    .Select(c => c.PriceUsd)
                    .ToArray();

                // Prepare the data for analysis
                var currentIndex = 1;
                var indexes = coinValues.Select(c => (double)currentIndex++).ToArray();

                // Calculate the linear regression
                double rSquared, yIntercept, slope;
                Stats.CalculateLinearRegression(indexes, coinValues, 0, selectedCount, out rSquared, out yIntercept, out slope);

                // Prepare the result
                trend.CurrentValue = coinValues.Last();
                trend.Trend = (slope > 0.05) ? 1    // Positive trend
                            : (slope < -0.05) ? -1  // Negative trend
                            : 0;                    // Flat trend
            }

            return new OkObjectResult(trend);
        }
    }
}
