namespace MMakerBotPanel.WebServices.Binance.Interface
{
    using MMakerBotPanel.WebServices.Interface;

    public abstract class BinanceWebServicesAbstract : ACEXAPIWebServices
    {
        private protected readonly string baseURL = "https://api.binance.com/";

        private protected readonly string statusURL = "sapi/v1/system/status";

        private protected readonly string timeURL = "api/v3/time";

        private protected readonly string symbolURL = "api/v3/exchangeInfo";

        private protected readonly string stockDataURL = "api/v3/uiKlines";

        private protected readonly string createOrderURL = "api/v3/order";

        private protected readonly string cancelOrderURL = "api/v3/openOrders";

        private protected readonly string activeOrderURL = "api/v3/openOrders";

        private protected readonly string queryOrderURL = "api/v3/order";

        private protected readonly string balanceURL = "sapi/v3/asset/getUserAsset";

        private protected readonly string orderBookURL = "api/v3/depth";

        private protected readonly string tradeURL = "api/v3/trades";

        private protected readonly string comissionURL = "sapi/v1/asset/tradeFee";


    }
}