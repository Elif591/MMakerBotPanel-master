namespace MMakerBotPanel.WebServices.OKX.Interface
{
    using MMakerBotPanel.WebServices.Interface;

    public abstract class OKXWebServicesAbstract : ACEXAPIWebServices
    {
        private protected readonly string baseURL = "https://www.okx.com/";

        private protected readonly string symbolURL = "api/v5/market/tickers";

        private protected readonly string parityURL = "api/v5/market/ticker";

        private protected readonly string stockDataURL = "api/v5/market/candles";

        private protected readonly string statusURL = "api/v5/public/time";

        private protected readonly string createOrderURL = "api/v5/trade/order";

        private protected readonly string balanceURL = "api/v5/account/balance";

        private protected readonly string activeOrderURL = "api/v5/trade/orders-pending";

        private protected readonly string tradeURL = "api/v5/market/trades";

        private protected readonly string orderBookURL = "api/v5/market/books";

        private protected readonly string cancelOrderURL = "api/v5/sprd/mass-cancel";

        private protected readonly string queryOrderURL = "api/v5/trade/order";

        private protected readonly string comissionFeeURL = "api/v5/account/trade-fee";
    }
}