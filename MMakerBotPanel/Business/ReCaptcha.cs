namespace MMakerBotPanel.Business
{
    using System.Collections.Generic;

    public class ReCaptcha
    {
        public bool Success { get; set; }
        public List<string> ErrorCodes { get; set; }

        public static bool Validate(string encodedResponse)
        {
            if (string.IsNullOrEmpty(encodedResponse))
            {
                return false;
            }

            System.Net.WebClient client = new System.Net.WebClient();
            string secret = "6LeIxAcTAAAAAGG-vFI1TnRWxMZNFuojJ4WifJWe"; //localhost
            //string secret = "6Ld8zUAnAAAAAHGWB6amIkInRIYMA8Nu-k6Hwpln"; //aibot.cos-in.com

            if (string.IsNullOrEmpty(secret))
            {
                return false;
            }

            string googleReply = client.DownloadString(string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", secret, encodedResponse));

            System.Web.Script.Serialization.JavaScriptSerializer serializer = new System.Web.Script.Serialization.JavaScriptSerializer();

            ReCaptcha reCaptcha = serializer.Deserialize<ReCaptcha>(googleReply);

            return reCaptcha.Success;
        }
    }
}