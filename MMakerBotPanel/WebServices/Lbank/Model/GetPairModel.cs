namespace MMakerBotPanel.WebServices.Lbank.Model
{
    using MMakerBotPanel.Models;

    public class GetPairModel
    {
        public GetPairModel()
        {
            genericResult = new GenericResult();
        }
        public GenericResult genericResult { get; set; }
        public int amountScale { get; set; }
        public int transferAmtScale { get; set; }
        public string assetCode { get; set; }
        public string canWithDraw { get; set; }
        public double fee { get; set; }
        public int type { get; set; }
        public double min { get; set; }
        public double minTransfer { get; set; }
        public int chain { get; set; }
    }
}