namespace MMakerBotPanel.Business.Exchange
{
    using MMakerBotPanel.Models;

    public class GateHelper : ExchangeHelperAbstract
    {
        private static GateHelper _GateHelper;

        private static readonly object _Lock = new object();

        private GateHelper()
        {
            _cexExchangeEnum = CEXEXCHANGE.Gateio;
        }

        public static GateHelper GetGateHelper()
        {
            lock (_Lock)
            {
                if (_GateHelper == null)
                {
                    _GateHelper = new GateHelper();
                }
                return _GateHelper;
            }
        }
    }
}
