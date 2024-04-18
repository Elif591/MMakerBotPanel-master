namespace MMakerBotPanel.Business.Exchange
{
    using MMakerBotPanel.Models;
    public class ProbitHelper : ExchangeHelperAbstract
    {

        private static ProbitHelper _ProbitHelper;

        private static readonly object _Lock = new object();

        private ProbitHelper()
        {
            _cexExchangeEnum = CEXEXCHANGE.Probit;
        }

        public static ProbitHelper GetProbitHelper()
        {
            lock (_Lock)
            {
                if (_ProbitHelper == null)
                {
                    _ProbitHelper = new ProbitHelper();
                }
                return _ProbitHelper;
            }
        }

    }

}