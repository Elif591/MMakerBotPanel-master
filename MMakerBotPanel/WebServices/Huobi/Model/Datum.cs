namespace MMakerBotPanel.WebServices.Huobi.Model
{
    using System.Collections.Generic;

    public class Datum
    {
        public string tags { get; set; }
        public string state { get; set; }
        public string wr { get; set; }
        public List<P1> p1 { get; set; }
        public int sm { get; set; }
        public string sc { get; set; }
        public List<P> p { get; set; }
        public string bc { get; set; }
        public string qc { get; set; }
        public bool te { get; set; }
        public bool cd { get; set; }
        public bool whe { get; set; }
        public string bcdn { get; set; }
        public string qcdn { get; set; }
        public object elr { get; set; }
        public object toa { get; set; }
        public string sp { get; set; }
        public object d { get; set; }
        public object smlr { get; set; }
        public object flr { get; set; }
        public int w { get; set; }
        public int tpp { get; set; }
        public int tap { get; set; }
        public int ttp { get; set; }
        public int fp { get; set; }
        public double? lr { get; set; }
        public string dn { get; set; }
        public string suspend_desc { get; set; }
    }
}