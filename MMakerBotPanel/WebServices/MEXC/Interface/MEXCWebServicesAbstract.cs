namespace MMakerBotPanel.WebServices.MEXC.Interface
{
    using MMakerBotPanel.WebServices.Interface;

    public abstract class MEXCWebServicesAbstract : ACEXAPIWebServices
    {
        private protected readonly string BaseURL = "https://www.mexc.com";

        private protected readonly string BaseApiURL = "https://api.mexc.com";

        private protected readonly string SymbolsUrl = "/open/api/v2/market/symbols";

        private protected readonly string statusUrl = "/api/v3/time";

        private protected readonly string DataUrl = "/api/v3/klines";

        private protected readonly string OrderUrl = "/api/v3/order";

        private protected readonly string balanceUrl = "/api/v3/account";

        private protected readonly string cancelOrdersUrl = "/api/v3/openOrders";

        private protected readonly string queryOrderUrl = "/api/v3/order";

        private protected readonly string orderbookUrl = "/api/v3/depth";
    }
}