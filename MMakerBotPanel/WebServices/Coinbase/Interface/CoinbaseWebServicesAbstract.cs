namespace MMakerBotPanel.WebServices.Coinbase.Interface
{
    using MMakerBotPanel.WebServices.Interface;

    public abstract class CoinbaseWebServicesAbstract : ACEXAPIWebServices
    {
        private protected readonly string BaseURL = "https://api.exchange.coinbase.com";

        private protected readonly string SymbolsURL = "/products";

        private protected readonly string DataUrl = "/products";

        private protected readonly string statusUrl = "https://api.coinbase.com/v2/time";

        private protected readonly string orderUrl = "https://api.coinbase.com/api/v3/brokerage/orders";

        private protected readonly string clientUrl = "https://www.coinbase.com/oauth/authorize";


    }
}