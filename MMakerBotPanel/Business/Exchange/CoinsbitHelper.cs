namespace MMakerBotPanel.Business.Exchange
{
    using MMakerBotPanel.Models;

    public class CoinsbitHelper : ExchangeHelperAbstract
    {
        private static CoinsbitHelper _CoinsbitHelper;

        private static readonly object _Lock = new object();

        private CoinsbitHelper()
        {
            _cexExchangeEnum = CEXEXCHANGE.Coinsbit;

        }
        public static CoinsbitHelper GetCoinsbitHelper()
        {
            lock (_Lock)
            {
                if (_CoinsbitHelper == null)
                {
                    _CoinsbitHelper = new CoinsbitHelper();
                }
                return _CoinsbitHelper;
            }
        }
    }
}