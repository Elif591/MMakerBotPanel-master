namespace MMakerBotPanel.Business.Exchange
{
    using MMakerBotPanel.Models;
    public class BitgetHelper : ExchangeHelperAbstract
    {
        private static BitgetHelper _BitgetHelper;

        private static readonly object _Lock = new object();

        private BitgetHelper()
        {
            _cexExchangeEnum = CEXEXCHANGE.Bitget;
        }

        public static BitgetHelper GetBitgetHelper()
        {
            lock (_Lock)
            {
                if (_BitgetHelper == null)
                {
                    _BitgetHelper = new BitgetHelper();
                }
                return _BitgetHelper;
            }
        }
    }
}