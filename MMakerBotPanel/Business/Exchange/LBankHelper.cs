namespace MMakerBotPanel.Business.Exchange
{
    using MMakerBotPanel.Models;

    public class LBankHelper : ExchangeHelperAbstract
    {

        private static LBankHelper _LBankHelper;

        private static readonly object _Lock = new object();

        private LBankHelper()
        {
            _cexExchangeEnum = CEXEXCHANGE.Lbank;
        }
        public static LBankHelper GetLBankHelper()
        {
            lock (_Lock)
            {
                if (_LBankHelper == null)
                {
                    _LBankHelper = new LBankHelper();
                }
                return _LBankHelper;
            }
        }
    }
}