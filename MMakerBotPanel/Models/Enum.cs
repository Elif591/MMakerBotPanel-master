namespace MMakerBotPanel.Models
{
    using System;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using System.Reflection;

    public enum CHECKDATESTATUS
    {
        [Display(Name = "30sn")]
        Second30 = 30,
        [Display(Name = "1m")]
        Minutes1 = 60,
        [Display(Name = "2m")]
        Minutes2 = 120,
        [Display(Name = "5m")]
        Minutes5 = 300,
    }

    public enum PURCHASESTATUS
    {
        [Display(Name = "Suucess")]
        Confirm = 1,
        [Display(Name = "Pending")]
        Pending = 2,
        [Display(Name = "Error")]
        Reject = 3,

    }
    public static class EnumExtensions
    {
        public static string GetDisplayName(this Enum value)
        {
            return value.GetType()
              .GetMember(value.ToString())
              .First()
              .GetCustomAttribute<DisplayAttribute>()
              ?.GetName();
        }
    }
    public enum EXCHANGEAPIKEY
    {
        [Display(Name = "API Key")]
        ApiKey = 1,

        [Display(Name = "Secret Key")]
        SecretKey = 2,

        [Display(Name = "Pass Phrase")]
        PassPhrase = 3

    }

    public enum TICKETSTATUS
    {
        [Display(Name = "Awaiting Answer")]
        AwaitingAnswer = 1,

        [Display(Name = "Answered")]
        Answered = 2,

        [Display(Name = "Closed")]
        Closed = 3
    }

    public enum REPLYSTATUS
    {
        [Description("Reply")]
        Reply = 0,
        [Description("Replied")]
        Replied = 1
    }

    public enum STATE
    {
        [Description("Pending")]
        Pending = 1,
        [Description("Success")]
        Success = 2
    }

    public enum WORKERSTATE
    {
        [Description("Pending TXID")]
        [Display(Name = "Pending TXID")]
        PendingTXID = 1,

        [Description("Working")]
        [Display(Name = "Working")]
        Working = 2,

        [Description("Expired")]
        [Display(Name = "Expired")]
        Expired = 3,

        [Description("Paused")]
        [Display(Name = "Paused")]
        Paused = 4
    }

    public enum EXCHANGETYPE
    {
        [Description("CEX")]
        CEX = 1,
        [Description("DEX")]
        DEX = 2
    }

    public enum CEXEXCHANGE
    {
        [Description("Binance")]
        Binance = 1,
        [Description("Kraken")]
        Kraken = 2,
        [Description("Kucoin")]
        Kucoin = 3,
        [Description("Bitfinex")]
        Bitfinex = 4,
        [Description("OKX")]
        OKX = 5,
        [Description("Huobi")]
        Huobi = 6,
        [Description("Bybit")]
        Bybit = 7,
        [Description("MEXC")]
        Mexc = 8,
        [Description("Gate.io")]
        Gateio = 9,
        [Description("Lbank")]
        Lbank = 10,
        [Description("Alterdice")]
        Alterdice = 11,
        [Description("Dex-Trade")]
        Dextrade = 12,
        [Description("Bitget")]
        Bitget = 13,
        [Description("Coinsbit")]
        Coinsbit = 14,
        [Description("Probit")]
        Probit = 15
    }


    public enum LOGAPITYPE
    {
        [Description("Binance")]
        Binance = 1,
        [Description("Kraken")]
        Kraken = 2,
        [Description("Kucoin")]
        Kucoin = 3,
        [Description("Bitfinex")]
        Bitfinex = 4,
        [Description("OKX")]
        OKX = 5,
        [Description("Huobi")]
        Huobi = 6,
        [Description("Bybit")]
        Bybit = 7,
        [Description("MEXC")]
        Mexc = 8,
        [Description("Gate.io")]
        Gateio = 9,
        [Description("Lbank")]
        Lbank = 10,
        [Description("Alterdice")]
        Alterdice = 11,
        [Description("Dex-Trade")]
        Dextrade = 12,
        [Description("Bitget")]
        Bitget = 13,
        [Description("Coinsbit")]
        Coinsbit = 14,
        [Description("Probit")]
        Probit = 15

    }

    public enum DEXEXCHANGE
    {
        [Description("Pancakeswap")]
        Pancakeswap = 1,
        [Description("Uniswap V2")]
        UniswapV2 = 2,
        [Description("Uniswap V3")]
        UniswapV3 = 3,
        [Description("TraderJoe")]
        TraderJoe = 4

    }

    public enum LOGTYPE
    {
        ERROR = 1,
        WARNING = 2,
        INFO = 3
    }

    public enum GridCache
    {
        tick = 20000
    }

    public enum RETURNTYPE
    {
        HEX = 0,
        BASE64 = 1,
    }

    public enum CACHEKEYENUMTYPE
    {
        ProfitService_LastActivity = 1008,
        PaymentControlService_LastActivity = 1000,
        TaskSchedulerService_LastActivity = 1001,
        OrderSetting_TLL = 1002,
        OrderSettings_CheckPeriyod = 1003,
        Etherscan_APIKey = 1004,
        WorkerSubscriptionService_LastActivity = 1005,
        WorkerWorkingControlService_LastActivity = 1006,
        GridStrategy_LastActivity = 1007
    }

    public enum USERRISKTYPE
    {
        LowRiskProfile = 1,
        MediumRiskProfile = 2,
        HighRiskProfile = 3,

    }

    public enum SIDE
    {
        Buy = 0,
        Sell = 1,
    }

    public enum TYPE_TRADE
    {
        LIMIT = 0,
        MARKET = 1
    }

    public enum RiskLevel
    {
        Low = 1,
        Medium = 2,
        High = 3
    }
    public enum TimeInterval
    {
        [Description("7G")]
        [Display(Name = "7G")]
        SevenDays = 7,
        [Description("30G")]
        [Display(Name = "30G")]
        ThirtyDays = 30,
        [Description("180G")]
        [Display(Name = "180G")]
        OneHundredEightyDays = 180
    }

    public enum ParameterTYPE
    {
        auto = 0,
        manual = 1
    }

    public enum BOTSTATUS
    {
        start = 1,
        stop = 2,
        takeProfit = 3,
        stoploss = 4
    }


    public enum WORKERTYPE
    {
        CexGridWorker = 1,
        DexGridWorker = 2,
        AdvancedMMakerBot = 3
    }
}