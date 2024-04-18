namespace MMakerBotPanel.WebServices.Coinstore.Interface
{
    using MMakerBotPanel.WebServices.Interface;

    public abstract class CoinstoreWebServicesAbstract : ACEXAPIWebServices
    {
        private protected readonly string BaseURL = "https://api.coinstore.com/api";

        private protected readonly string SymbolsUrl = "/v1/market/tickers";

        private protected readonly string DataUrl = "/v1/market/kline/";

        private protected readonly string OrderUrl = "/trade/order/place";

        private protected readonly string statusUrl = "/v1/market/trade/BTCUSDT?size=1";

    }
}