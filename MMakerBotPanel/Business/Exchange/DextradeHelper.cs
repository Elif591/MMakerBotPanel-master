namespace MMakerBotPanel.Business.Exchange
{
    using MMakerBotPanel.Models;

    public class DextradeHelper : ExchangeHelperAbstract
    {
        private static DextradeHelper _DextradeHelper;

        private static readonly object _Lock = new object();

        private DextradeHelper()
        {
            _cexExchangeEnum = CEXEXCHANGE.Dextrade;
        }

        public static DextradeHelper GetDextradeHelper()
        {
            lock (_Lock)
            {
                if (_DextradeHelper == null)
                {
                    _DextradeHelper = new DextradeHelper();
                }
                return _DextradeHelper;
            }
        }
    }
}