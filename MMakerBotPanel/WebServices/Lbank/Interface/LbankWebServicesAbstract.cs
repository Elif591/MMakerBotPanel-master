namespace MMakerBotPanel.WebServices.Lbank.Interface
{
    using MMakerBotPanel.WebServices.Interface;

    public abstract class LbankWebServicesAbstract : ACEXAPIWebServices
    {
        private protected readonly string baseURL = "https://api.lbkex.com/";

        private protected readonly string symbolURL = "v2/currencyPairs.do";

        private protected readonly string stockDataURL = "v2/kline.do";

        private protected readonly string timeURL = "v2/timestamp.do";

        private protected readonly string createOrderURL = "v2/create_order.do";

        private protected readonly string statusURL = "v2/supplement/system_status.do";

        private protected readonly string balanceURL = "v2/supplement/user_info.do";

        private protected readonly string activeOrderURL = "v2/supplement/orders_info_no_deal.do";

        private protected readonly string orderBookURL = "v2/depth.do";

        private protected readonly string tradeURL = "v2/trades.do";

        private protected readonly string pairURL = "v2/withdrawConfigs.do";

        private protected readonly string cancelOrderURL = "v2/cancel_order.do";

        private protected readonly string queryOrderURL = "v2/orders_info.do";

    }
}