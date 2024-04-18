namespace MMakerBotPanel.Business.Exchange
{
    using MMakerBotPanel.Models;

    public class AlterdiceHelper : ExchangeHelperAbstract
    {
        private static AlterdiceHelper _AlterdiceHelper;

        private static readonly object _Lock = new object();

        private AlterdiceHelper()
        {
            _cexExchangeEnum = CEXEXCHANGE.Alterdice;
        }

        public static AlterdiceHelper GetAlterdiceHelper()
        {
            lock (_Lock)
            {
                if (_AlterdiceHelper == null)
                {
                    _AlterdiceHelper = new AlterdiceHelper();
                }
                return _AlterdiceHelper;
            }
        }

    }
}