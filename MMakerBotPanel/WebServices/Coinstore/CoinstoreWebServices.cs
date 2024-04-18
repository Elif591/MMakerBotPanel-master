namespace MMakerBotPanel.WebServices.Coinstore
{
    using MMakerBotPanel.Business;
    using MMakerBotPanel.Models;
    using MMakerBotPanel.WebServices.Coinstore.Interface;
    using MMakerBotPanel.WebServices.Coinstore.Model;
    using Newtonsoft.Json;
    using RestSharp;
    using System;
    using System.Globalization;
    using System.Web.Script.Serialization;

    public class CoinstoreWebServices : CoinstoreWebServicesAbstract
    {
        public CoinstoreSymbolModel getParity()
        {

            CoinstoreSymbolModel ret = new CoinstoreSymbolModel();

            RestClient client = new RestClient(BaseURL + SymbolsUrl)
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
                        if (response.ResponseUri.ToString() != (BaseURL + SymbolsUrl))
                        {
                            ret.genericResult.Message = "Bilimp Adresi geçersiz";
                            return ret;
                        }

                        JavaScriptSerializer ser = new JavaScriptSerializer
                        {
                            MaxJsonLength = int.MaxValue
                        };
                        ret.CoinstoreSymbolDataModels = ser.Deserialize<CoinstoreSymbolDataModel>(response.Content);
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
                //log.WriteLog(LOGTYPE.ERROR, "CheckServer -> Response = null  -> URL: " + _ApiURL + _CheckServerPath);
                ret.genericResult.IsOK = false;
                ret.genericResult.Message = "Hata";
                return ret;
            }
        }
        public StatusModel status()
        {
            StatusModel ret = new StatusModel();

            RestClient client = new RestClient(BaseURL + statusUrl)
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
                        if (response.ResponseUri.ToString() != (BaseURL + statusUrl))
                        {
                            ret.genericResult.Message = "Bilimp Adresi geçersiz";
                            return ret;
                        }

                        JavaScriptSerializer ser = new JavaScriptSerializer
                        {
                            MaxJsonLength = int.MaxValue
                        };
                        ret = ser.Deserialize<StatusModel>(response.Content);

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
                    //log.WriteLog(LOGTYPE.ERROR, "CheckServer -> Response ContentLength = 0  -> URL: " + _ApiURL + _CheckServerPath);
                    ret.genericResult.IsOK = false;
                    ret.genericResult.Message = "Hata";
                    return ret;
                }

            }
            else
            {
                //log.WriteLog(LOGTYPE.ERROR, "CheckServer -> Response = null  -> URL: " + _ApiURL + _CheckServerPath);
                ret.genericResult.IsOK = false;
                ret.genericResult.Message = "Hata";
                return ret;
            }
        }
        public CoinStoreDataModel getStockData(string symbol, string interval)
        {

            CoinStoreDataModel ret = new CoinStoreDataModel();

            RestClient client = new RestClient(BaseURL + DataUrl + symbol + "?period=4hour&size=1000")
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

                        ret.CoinStoreDataDetailModels = ser.Deserialize<CoinStoreDataDetailModel>(response.Content);



                        foreach (Item item in ret.CoinStoreDataDetailModels.data.item)
                        {

                            CoinStoreChartDataModel addData = new CoinStoreChartDataModel();


                            CultureInfo CI = new CultureInfo("en-US", false);
                            addData.time = Convert.ToInt64(item.startTime + 7200, CI);
                            addData.volume = (long)Convert.ToDouble(item.volume, CI);
                            addData.close = Convert.ToDouble(item.close, CI);
                            addData.high = Convert.ToDouble(item.high, CI);
                            addData.low = Convert.ToDouble(item.low, CI);
                            addData.open = Convert.ToDouble(item.open, CI);

                            ret.CoinStoreChartDataModels.Add(addData);

                        }

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
                //log.WriteLog(LOGTYPE.ERROR, "CheckServer -> Response = null  -> URL: " + _ApiURL + _CheckServerPath);
                ret.genericResult.IsOK = false;
                ret.genericResult.Message = "Hata";
                return ret;
            }

        }
        public CoinStoreDataModel getStockDataDate(string symbol, string interval)
        {

            CoinStoreDataModel ret = new CoinStoreDataModel();

            RestClient client = new RestClient(BaseURL + DataUrl + symbol + "?period=4hour&size=1")
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

                        ret.CoinStoreDataDetailModels = ser.Deserialize<CoinStoreDataDetailModel>(response.Content);



                        foreach (Item item in ret.CoinStoreDataDetailModels.data.item)
                        {

                            CoinStoreChartDataModel addData = new CoinStoreChartDataModel();


                            CultureInfo CI = new CultureInfo("en-US", false);
                            addData.time = Convert.ToInt64(item.startTime + 7200, CI);
                            addData.volume = (long)Convert.ToDouble(item.volume, CI);
                            addData.close = Convert.ToDouble(item.close, CI);
                            addData.high = Convert.ToDouble(item.high, CI);
                            addData.low = Convert.ToDouble(item.low, CI);
                            addData.open = Convert.ToDouble(item.open, CI);

                            ret.CoinStoreChartDataModels.Add(addData);

                        }

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
                //log.WriteLog(LOGTYPE.ERROR, "CheckServer -> Response = null  -> URL: " + _ApiURL + _CheckServerPath);
                ret.genericResult.IsOK = false;
                ret.genericResult.Message = "Hata";
                return ret;
            }
        }
        public CoinstoreExchange newOrder(string signature, string apiKey, string timeStamp, object body)
        {
            CoinstoreExchange ret = new CoinstoreExchange();

            RestClient client = new RestClient(BaseURL + OrderUrl)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.POST);
            _ = request.AddHeader("Content-Type", "application/json");
            _ = request.AddHeader("X-CS-APIKEY", apiKey);
            _ = request.AddHeader("X-CS-EXPIRES", timeStamp);
            _ = request.AddHeader("X-CS-SIGN", signature);
            _ = request.AddJsonBody(body);
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
                        ret = ser.Deserialize<CoinstoreExchange>(response.Content);

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
                    //log.WriteLog(LOGTYPE.ERROR, "CheckServer -> Response ContentLength = 0  -> URL: " + _ApiURL + _CheckServerPath);
                    ret.genericResult.IsOK = false;
                    ret.genericResult.Message = "Hata";
                    return ret;
                }

            }
            else
            {
                //log.WriteLog(LOGTYPE.ERROR, "CheckServer -> Response = null  -> URL: " + _ApiURL + _CheckServerPath);
                ret.genericResult.IsOK = false;
                ret.genericResult.Message = "Hata";
                return ret;
            }

        }


    }
}