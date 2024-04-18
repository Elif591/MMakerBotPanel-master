namespace MMakerBotPanel.WebServices.Etherscan.Interface
{
    using MMakerBotPanel.WebServices.Interface;

    public abstract class EtherscanWebServicesAbstract : ACEXAPIWebServices
    {
        private protected readonly string BaseUrl = "https://api.etherscan.io/api";

        private protected readonly string statusUrl = "?module=transaction&action=gettxreceiptstatus";
    }
}