namespace MMakerBotPanel.WebServices.Etherscan
{
    using MMakerBotPanel.WebServices.Etherscan.Interface;
    using MMakerBotPanel.WebServices.Etherscan.Model;
    using RestSharp;
    using System.Web.Script.Serialization;

    public class EtherscanWebServices : EtherscanWebServicesAbstract
    {
        public EtherscanStatusModel getEtherscanStatus(string txhash, string apikey)
        {
            EtherscanStatusModel ret = new EtherscanStatusModel();
            RestClient client = new RestClient(BaseUrl + statusUrl + "&txhash=" + txhash + "&apikey=" + apikey)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            _ = request.AddHeader("Content-type", "application/json");

            RestResponse response = (RestResponse)client.Execute(request);

            if (response != null)
            {

                if (response.ResponseStatus == ResponseStatus.Completed)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {

                        JavaScriptSerializer ser = new JavaScriptSerializer
                        {
                            MaxJsonLength = int.MaxValue
                        };
                        ret = ser.Deserialize<EtherscanStatusModel>(response.Content);
                        return ret;
                    }
                    else
                    {
                        ret.genericResult.IsOK = false;
                        ret.genericResult.Message = RequestError(response, "CheckServer");
                        return ret;
                    }
                }
                else
                {
                    ret.genericResult.IsOK = false;
                    ret.genericResult.Message = RequestError(response, "CheckServer");
                    return ret;
                }
            }
            else
            {
                ret.genericResult.IsOK = false;
                ret.genericResult.Message = "Hata";
                return ret;
            }
        }
    }
}