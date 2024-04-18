namespace MMakerBotPanel.WebServices.Kucoin.Interface
{
    using MMakerBotPanel.WebServices.Interface;

    public abstract class KucoinWebServicesAbstract : ACEXAPIWebServices
    {
        private protected readonly string baseURL = "https://api.kucoin.com/";

        private protected readonly string symbolURL = "api/v1/symbols";

        private protected readonly string stockDataURL = "api/v1/market/candles";

        private protected readonly string statusURL = "api/v1/timestamp";

        private protected readonly string createOrderURL = "api/v1/orders";

        private protected readonly string balanceURL = "api/v1/accounts/ledgers";

        private protected readonly string orderBookURL = "api/v1/market/orderbook/level2_100";

        private protected readonly string tradeURL = "api/v1/market/histories";

        private protected readonly string cancelorderURL = "api/v1/stop-order/cancel";

        private protected readonly string queryorderURL = "api/v1/orders";

    }
}