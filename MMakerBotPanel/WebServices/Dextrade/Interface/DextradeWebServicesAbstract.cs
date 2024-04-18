namespace MMakerBotPanel.WebServices.Dextrade.Interface
{
    using MMakerBotPanel.WebServices.Interface;

    public abstract class DextradeWebServicesAbstract : ACEXAPIWebServices
    {
        private protected readonly string baseURL = "https://api.dex-trade.com/";

        private protected readonly string dataBaseURL = "https://socket.dex-trade.com/";

        private protected readonly string symbolURL = "v1/public/symbols";

        private protected readonly string statusURL = "v1/public/trades?pair=BTCUSDT";

        private protected readonly string stockDataURL = "graph/hist";

        private protected readonly string createOrderURL = "v1/private/create-order";

        private protected readonly string activeOrderURL = "v1/private/orders";
        
        private protected readonly string cancelOrderURL = "v1/private/delete-order";

        private protected readonly string queryOrderURL = "v1/private/get-order";

        private protected readonly string balanceURL = "v1/private/balances";

        private protected readonly string orderBookURL = "v1/public/book";

        private protected readonly string tradeURL = "v1/public/trades";

    }
}