namespace MMakerBotPanel.Business.Exchange
{
    using MMakerBotPanel.Models;

    public class BitfinexHelper : ExchangeHelperAbstract
    {
        private static BitfinexHelper _BitfinexHelper;

        private static readonly object _Lock = new object();

        private BitfinexHelper()
        {
            _cexExchangeEnum = CEXEXCHANGE.Bitfinex;

        }

        public static BitfinexHelper GetBitfinexHelper()
        {
            lock (_Lock)
            {
                if (_BitfinexHelper == null)
                {
                    _BitfinexHelper = new BitfinexHelper();
                }
                return _BitfinexHelper;
            }
        }

        //public static Tuple<string, string> GetUserAPIKeys()
        //{
        //    string APIKey = "bKqduyrEGgUd3jxyHxNkEUMA0ieJ4835JpvddYuhVP5";
        //    string SecretKey = "Pn76w0ZMj7SdivCQnVEv3wk8Tfwb2egATXmwKRM3kgf";
        //    Tuple<string, string> ret = new Tuple<string, string>(APIKey, SecretKey);
        //    return ret;
        //}
    }
}