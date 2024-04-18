namespace MMakerBotPanel.Business.Exchange
{
    using MMakerBotPanel.Models;

    public class KrakenHelper : ExchangeHelperAbstract
    {

        private static KrakenHelper _KrakenHelper;

        private static readonly object _Lock = new object();

        private KrakenHelper()
        {
            _cexExchangeEnum = CEXEXCHANGE.Kraken;
        }
        public static KrakenHelper GetKrakenHelper()
        {
            lock (_Lock)
            {
                if (_KrakenHelper == null)
                {
                    _KrakenHelper = new KrakenHelper();
                }
                return _KrakenHelper;
            }
        }
    }
}