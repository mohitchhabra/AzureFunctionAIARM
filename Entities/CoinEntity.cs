using System;
using Microsoft.WindowsAzure.Storage.Table;

namespace ml.CoinValue.Entities
{
    public class CoinEntity : TableEntity
    {
        public double PriceUsd { get; set; }

        public string Symbol { get; set; }

        public DateTime TimeOfReading { get; set; }
    }
}
