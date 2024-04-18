namespace MMakerBotPanel.WebServices.Gateio.Interface
{
    using MMakerBotPanel.WebServices.Interface;

    public abstract class GateioWebServicesAbstract : ACEXAPIWebServices
    {
        private protected readonly string baseURL = "https://api.gateio.ws/";

        private protected readonly string symbolURL = "api/v4/spot/currency_pairs";

        private protected readonly string stockDataURL = "api/v4/spot/candlesticks";

        private protected readonly string statusURL = "api/v4/spot/time";

        private protected readonly string createOrderURL = "api/v4/spot/orders";

        private protected readonly string balanceURL = "api/v4/wallet/total_balance";

        private protected readonly string activeOrderURL = "api/v4/spot/open_orders";

        private protected readonly string orderBookURL = "api/v4/spot/order_book";

        private protected readonly string tradeURL = "api/v4/spot/trades";

        private protected readonly string cancelOrderURL = "api/v4/spot/orders";

        private protected readonly string queryOrderURL = "api/v4/spot/orders";

        private protected readonly string comissionFeeURL = "rebate/agency/commission_history";
    }
}