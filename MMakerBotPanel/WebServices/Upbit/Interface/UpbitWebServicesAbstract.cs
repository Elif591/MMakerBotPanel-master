namespace MMakerBotPanel.WebServices.Upbit.Interface
{
    using MMakerBotPanel.WebServices.Interface;

    public abstract class UpbitWebServicesAbstract : ACEXAPIWebServices
    {
        private protected readonly string BaseURL = "https://api.upbit.com";

        private protected readonly string SymbolsUrl = "/v1/market/all?isDetails=false";

        private protected readonly string DataUrl = "/v1/candles/";

        private protected readonly string statusUrl = "/v1/trades/ticks?market=USDT-BTC&count=1";
    }
}