namespace MMakerBotPanel.Business
{
    using MMakerBotPanel.Models;
    using MMakerBotPanel.WebServices.Alterdice;
    using MMakerBotPanel.WebServices.Binance;
    using MMakerBotPanel.WebServices.Bitfinex;
    using MMakerBotPanel.WebServices.Bitget;
    using MMakerBotPanel.WebServices.Bybit;
    using MMakerBotPanel.WebServices.Coinbase;
    using MMakerBotPanel.WebServices.Coinsbit;
    using MMakerBotPanel.WebServices.Coinstore;
    using MMakerBotPanel.WebServices.Dextrade;
    using MMakerBotPanel.WebServices.Gateio;
    using MMakerBotPanel.WebServices.Hotbit;
    using MMakerBotPanel.WebServices.Huobi;
    using MMakerBotPanel.WebServices.Interface;
    using MMakerBotPanel.WebServices.Kraken;
    using MMakerBotPanel.WebServices.Kucoin;
    using MMakerBotPanel.WebServices.Lbank;
    using MMakerBotPanel.WebServices.MEXC;
    using MMakerBotPanel.WebServices.OKX;
    using MMakerBotPanel.WebServices.Probit;
    using MMakerBotPanel.WebServices.Upbit;

    public static class CexExchangeSelect
    {
        public static ICEXAPIController CexSelect(CEXEXCHANGE CexExchange)
        {
            ICEXAPIController CexApiController;


            switch (CexExchange)
            {
                case CEXEXCHANGE.Binance:
                    CexApiController = new BinanceApiController();
                    break;
                case CEXEXCHANGE.Kraken:
                    CexApiController = new KrakenApiController();
                    break;
                case CEXEXCHANGE.Kucoin:
                    CexApiController = new KucoinApiController();
                    break;
                case CEXEXCHANGE.Bitfinex:
                    CexApiController = new BitfinexApiController();
                    break;
                case CEXEXCHANGE.OKX:
                    CexApiController = new OKXApiController();
                    break;
                case CEXEXCHANGE.Huobi:
                    CexApiController = new HuobiApiController();
                    break;
                case CEXEXCHANGE.Bybit:
                    CexApiController = new BybitApiController();
                    break;
                case CEXEXCHANGE.Mexc:
                    CexApiController = new MEXCApiController();
                    break;
                case CEXEXCHANGE.Gateio:
                    CexApiController = new GateioApiController();
                    break;
                case CEXEXCHANGE.Lbank:
                    CexApiController = new LbankApiController();
                    break;
                case CEXEXCHANGE.Coinsbit:
                    CexApiController = new CoinsbitApiController();
                    break;
                case CEXEXCHANGE.Alterdice:
                    CexApiController = new AlterdiceApiController();
                    break;
                case CEXEXCHANGE.Dextrade:
                    CexApiController = new DextradeApiController();
                    break;
                case CEXEXCHANGE.Bitget:
                    CexApiController = new BitgetApiController();
                    break;
                case CEXEXCHANGE.Probit:
                    CexApiController = new ProbitApiController();
                    break;
                default:
                    CexApiController = new BinanceApiController();
                    break;
            }

            return CexApiController;
        }

    }
}