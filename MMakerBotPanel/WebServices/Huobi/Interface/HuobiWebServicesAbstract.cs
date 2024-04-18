namespace MMakerBotPanel.WebServices.Huobi.Interface
{
    using MMakerBotPanel.WebServices.Interface;

    public abstract class HuobiWebServicesAbstract : ACEXAPIWebServices
    {
        private protected readonly string baseURL = "https://api.huobi.pro/";

        private protected readonly string symbolURL = "v2/settings/common/symbols";

        private protected readonly string stockDataURL = "market/history/kline";

        private protected readonly string statusURL = "v1/common/timestamp";

        private protected readonly string accountURL = "v1/account/accounts";

        private protected readonly string createOrderURL = "v1/order/orders/place";

        private protected readonly string activeOrderURL = "v1/order/openOrders";

        private protected readonly string orderBookURL = "market/depth";

        private protected readonly string tradeURL = "market/history/trade";

        private protected readonly string cancelOrderURL = "v1/order/orders/batchCancelOpenOrders";

        private protected readonly string queryOrderURL = "v1/order/orders";
    }
}