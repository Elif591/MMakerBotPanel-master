namespace MMakerBotPanel.WebServices.Probit.Interface
{
    using MMakerBotPanel.WebServices.Interface;

    public abstract class ProbitWebServicesAbstract : ACEXAPIWebServices
    {
        private protected readonly string baseURL = "https://api.probit.com/api/";

        private protected readonly string symbolURL = "exchange/v1/market";

        private protected readonly string stockDataURL = "exchange/v1/candle";

        private protected readonly string balanceURL = "exchange/v1/balance";

        private protected readonly string orderBookURL = "exchange/v1/order_book";

        private protected readonly string openOrderURL = "exchange/v1/open_order";

        private protected readonly string createOrderURL = "exchange/v1/new_order";

        private protected readonly string cancelOrderURL = "exchange/v1/cancel_order";

        private protected readonly string queryOrderURL = "exchange/v1/order";
    }
}