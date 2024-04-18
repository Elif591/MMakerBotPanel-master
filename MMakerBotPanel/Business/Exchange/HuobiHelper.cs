namespace MMakerBotPanel.Business.Exchange
{
    using MMakerBotPanel.Models;

    public class HuobiHelper : ExchangeHelperAbstract
    {
        private static HuobiHelper _HuobiHelper;

        private static readonly object _Lock = new object();

        private HuobiHelper()
        {
            _cexExchangeEnum = CEXEXCHANGE.Huobi;
        }

        public static HuobiHelper GetHuobiHelper()
        {
            lock (_Lock)
            {
                if (_HuobiHelper == null)
                {
                    _HuobiHelper = new HuobiHelper();
                }
                return _HuobiHelper;
            }
        }
    }
}