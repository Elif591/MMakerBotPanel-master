namespace MMakerBotPanel.WebServices.Bitfinex.Interface
{
    using MMakerBotPanel.WebServices.Interface;

    public abstract class BitfinexWebServicesAbstract : ACEXAPIWebServices
    {
        private protected readonly string baseURL = "https://api-pub.bitfinex.com/";

        private protected readonly string baseApiURL = "https://api.bitfinex.com/";

        private protected readonly string symbolURL = "v2/tickers";

        private protected readonly string stockDataURL = "v2/candles";

        private protected readonly string statusURL = "v2/platform/status";

        private protected readonly string timeURL = "v2/trades/tBTCUSD/hist";

        private protected readonly string createOrderURL = "v2/auth/w/order/submit";

        private protected readonly string balanceURL = "v2/auth/r/wallets";

        private protected readonly string activeOrderURL = "v2/auth/r/orders";

        private protected readonly string queryOrderURL = "v2/auth/r/orders";

        private protected readonly string orderBookURL = "v2/book";

        private protected readonly string tradesURL = "v2/trades";

        private protected readonly string cancelOrdersURL = "v2/auth/w/order/cancel/multi";

    }
}