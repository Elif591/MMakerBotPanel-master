namespace MMakerBotPanel.WebServices.Hotbit.Interface
{
    using MMakerBotPanel.WebServices.Interface;

    public abstract class HotbitWebServicesAbstract : ACEXAPIWebServices
    {
        private protected readonly string BaseURL = "https://api.hotbit.io";

        private protected readonly string SymbolsUrl = "/api/v1/market.list";

        private protected readonly string statusUrl = "/api/v1/server.time";

        private protected readonly string StockDataUrl = "/api/v1/market.kline";
    }
}