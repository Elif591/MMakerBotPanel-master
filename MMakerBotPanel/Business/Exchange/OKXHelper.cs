namespace MMakerBotPanel.Business.Exchange
{

    using MMakerBotPanel.Models;

    public class OKXHelper : ExchangeHelperAbstract
    {
        private static OKXHelper _OKXHelper;

        private static readonly object _Lock = new object();

        private OKXHelper()
        {
            _cexExchangeEnum = CEXEXCHANGE.OKX;
        }
        public static OKXHelper GetOKXHelper()
        {
            lock (_Lock)
            {
                if (_OKXHelper == null)
                {
                    _OKXHelper = new OKXHelper();
                }
                return _OKXHelper;
            }
        }
    }
}