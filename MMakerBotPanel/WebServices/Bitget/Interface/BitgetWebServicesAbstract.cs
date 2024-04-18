namespace MMakerBotPanel.WebServices.Bitget.Interface
{
    using MMakerBotPanel.WebServices.Interface;

    public class BitgetWebServicesAbstract : ACEXAPIWebServices
    {
        private protected readonly string baseURL = "https://api.bitget.com/";

        private protected readonly string symbolURL = "api/spot/v1/public/products";

        private protected readonly string stockDataURL = "api/spot/v1/market/candles";

        private protected readonly string createOrderURL = "api/spot/v1/trade/orders";

        private protected readonly string cancelOrderURL = "api/spot/v1/trade/cancel-symbol-order";

        private protected readonly string queryOrderURL = "api/spot/v1/trade/orderInfo";

        private protected readonly string balanceURL = "api/spot/v1/account/bills";

        private protected readonly string activeOrderURL = "api/spot/v1/trade/open-orders";

        private protected readonly string orderBookURL = "api/spot/v1/market/depth";

        private protected readonly string getPairURL = "api/spot/v1/public/product";
    }
}