namespace MMakerBotPanel.Business.Exchange
{
    using MMakerBotPanel.Models;

    public class BybitHelper : ExchangeHelperAbstract
    {
        private static BybitHelper _BybitHelper;

        private static readonly object _Lock = new object();

        private BybitHelper()
        {
            _cexExchangeEnum = CEXEXCHANGE.Bybit;
        }

        public static BybitHelper GetBybitHelper()
        {
            lock (_Lock)
            {
                if (_BybitHelper == null)
                {
                    _BybitHelper = new BybitHelper();
                }
                return _BybitHelper;
            }
        }
    }
}