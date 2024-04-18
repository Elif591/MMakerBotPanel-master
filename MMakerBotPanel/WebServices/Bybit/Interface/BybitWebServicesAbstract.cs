namespace MMakerBotPanel.WebServices.Bybit.Interface
{
    using MMakerBotPanel.WebServices.Interface;

    public abstract class BybitWebServicesAbstract : ACEXAPIWebServices
    {
        private protected readonly string baseURL = "https://api.bybit.com/";

        private protected readonly string symbolURL = "spot/v1/symbols";

        private protected readonly string pairURL = "v5/market/instruments-info";

        private protected readonly string stockDataURL = "spot/quote/v1/kline";

        private protected readonly string statusURL = "v3/public/time";

        private protected readonly string createOrderURL = "v5/order/create";

        private protected readonly string activeOrderURL = "v5/order/realtime";

        private protected readonly string balanceURL = "v5/account/wallet-balance";

        private protected readonly string orderBookURL = "v5/market/orderbook";

        private protected readonly string tradeURL = "v5/market/recent-trade";

        private protected readonly string cancelOrderURL = "v5/order/cancel-all";

        private protected readonly string queryOrderURL = "v5/order/history?category=spot";

    }
}