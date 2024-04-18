namespace MMakerBotPanel.WebServices.Coinsbit.Interface
{
    using MMakerBotPanel.WebServices.Interface;

    public abstract class CoinsbitWebServicesAbstract : ACEXAPIWebServices
    {
        private protected readonly string baseURL = "https://api.coinsbit.io/";

        private protected readonly string symbolURL = "api/v1/public/symbols";

        private protected readonly string pairURL = "api/v1/public/markets";

        private protected readonly string stockDataURL = "api/v1/public/kline";

        private protected readonly string createOrderURL = "api/v1/order/new";

        private protected readonly string statusURL = "api/v1/public/history/result";

        private protected readonly string activeOrderUrl = "api/v1/orders";

        private protected readonly string balanceURL = "api/v1/account/balances";

        private protected readonly string orderBookURL = "api/v1/public/book";

        private protected readonly string tradeURL = "api/v1/public/history/result";

        private protected readonly string cancelURL = "api/v1/order/cancel";


    }
}