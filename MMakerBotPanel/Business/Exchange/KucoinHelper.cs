namespace MMakerBotPanel.Business.Exchange
{
    using MMakerBotPanel.Models;

    public class KucoinHelper : ExchangeHelperAbstract
    {

        private static KucoinHelper _KucoinHelper;

        private static readonly object _Lock = new object();

        private KucoinHelper()
        {
            _cexExchangeEnum = CEXEXCHANGE.Kucoin;
        }
        public static KucoinHelper GetKucoinHelper()
        {
            lock (_Lock)
            {
                if (_KucoinHelper == null)
                {
                    _KucoinHelper = new KucoinHelper();
                }
                return _KucoinHelper;
            }
        }
    }

}