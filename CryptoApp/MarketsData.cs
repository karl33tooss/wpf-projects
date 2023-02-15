using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoApp
{
    internal class MarketsData
    {
        public string exchangeId { get; set; }
        public string baseSymbol { get; set; }
        public string quoteSymbol { get; set; }
        public string priceUsd { get; set; }
    }
}
