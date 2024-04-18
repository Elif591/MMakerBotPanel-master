namespace MMakerBotPanel.WebServices.Kraken.Interface
{
    using MMakerBotPanel.WebServices.Interface;

    public abstract class KrakenWebServicesAbstract : ACEXAPIWebServices
    {
        private protected readonly string BaseURL = "https://api.kraken.com";

        private protected readonly string SymbolsURL = "/0/public/AssetPairs";

        private protected readonly string StockDataURL = "/0/public/OHLC";

        private protected readonly string statusURL = "/0/public/Time";

        private protected readonly string orderURL = "/0/private/AddOrder";

        private protected readonly string balanceURL = "/0/private/TradeBalance";

        private protected readonly string activeOrderURL = "/0/private/OpenOrders";

        private protected readonly string orderBookURL = "/0/public/Depth";

        private protected readonly string cancelOrderURL = "/0/private/CancelAll";
    }
}