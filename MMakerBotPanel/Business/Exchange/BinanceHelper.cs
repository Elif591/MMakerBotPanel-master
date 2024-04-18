namespace MMakerBotPanel.Business.Exchange
{
    using MMakerBotPanel.Models;

    public class BinanceHelper : ExchangeHelperAbstract
    {

        private static BinanceHelper _BinanceHelper;

        private static readonly object _Lock = new object();
        private BinanceHelper()
        {
            _cexExchangeEnum = CEXEXCHANGE.Binance;
        }
        public static BinanceHelper GetBinanceHelper()
        {
            lock (_Lock)
            {
                if (_BinanceHelper == null)
                {
                    _BinanceHelper = new BinanceHelper();
                }
                return _BinanceHelper;
            }
        }

    }
}