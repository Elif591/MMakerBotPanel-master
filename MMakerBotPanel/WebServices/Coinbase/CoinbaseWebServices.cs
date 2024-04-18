namespace MMakerBotPanel.WebServices.Coinbase
{
    using MMakerBotPanel.Business;
    using MMakerBotPanel.Models;
    using MMakerBotPanel.WebServices.Coinbase.Interface;
    using MMakerBotPanel.WebServices.Coinbase.Model;
    using Newtonsoft.Json;
    using RestSharp;
    using System;
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Script.Serialization;

    public class CoinbaseWebServices : CoinbaseWebServicesAbstract
    {
        public CoinbaseSymbolModel getParity()
        {

            CoinbaseSymbolModel ret = new CoinbaseSymbolModel();

            RestClient client = new RestClient(BaseURL + SymbolsURL)
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
                        if (response.ResponseUri.ToString() != (BaseURL + SymbolsURL))
                        {
                            ret.genericResult.Message = "Bilimp Adresi geçersiz";
                            return ret;
                        }

                        JavaScriptSerializer ser = new JavaScriptSerializer
                        {
                            MaxJsonLength = int.MaxValue
                        };
                        ret.CoinbaseSymbolList = ser.Deserialize<List<CoinbaseSymbol>>(response.Content);
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

        public CoinbaseDataModel getStockData(string symbol, string interval)
        {

            CoinbaseDataModel ret = new CoinbaseDataModel();
            RestClient client = new RestClient(BaseURL + DataUrl + "/" + symbol + "/candles?granularity=3600")
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

                        ret.CoinbaseDatDetailaModels.Data = ser.Deserialize<List<List<double>>>(response.Content);

                        foreach (List<double> item in ret.CoinbaseDatDetailaModels.Data)
                        {


                            double[] dataItem = item.ToArray();
                            CoinbaseChartDataModel addData = new CoinbaseChartDataModel
                            {
                                low = Convert.ToDouble(dataItem[1]),
                                high = Convert.ToDouble(dataItem[2]),
                                volume = Convert.ToInt64(dataItem[5]),
                                time = Convert.ToInt64(dataItem[0]),
                                open = Convert.ToDouble(dataItem[3]),
                                close = Convert.ToDouble(dataItem[4])
                            };

                            ret.CoinbaseChartDataModels.Add(addData);

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

        public CoinbaseDataModel getStockDataDate(string symbol, string interval)
        {

            CoinbaseDataModel ret = new CoinbaseDataModel();

            int endTime = (int)(DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1)).TotalSeconds;

            long startTime = endTime - 14400;

            RestClient client = new RestClient(BaseURL + DataUrl + "/" + symbol + "/candles?granularity=3600&end=" + endTime + "&start=" + startTime)
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

                        ret.CoinbaseDatDetailaModels.Data = ser.Deserialize<List<List<double>>>(response.Content);

                        foreach (List<double> item in ret.CoinbaseDatDetailaModels.Data)
                        {


                            double[] dataItem = item.ToArray();
                            CoinbaseChartDataModel addData = new CoinbaseChartDataModel
                            {
                                low = Convert.ToDouble(dataItem[1]),
                                high = Convert.ToDouble(dataItem[2]),
                                volume = Convert.ToInt64(dataItem[5]),
                                time = Convert.ToInt64(dataItem[0]),
                                open = Convert.ToDouble(dataItem[3]),
                                close = Convert.ToDouble(dataItem[4])
                            };

                            ret.CoinbaseChartDataModels.Add(addData);

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

        public StatusModel status()
        {
            StatusModel ret = new StatusModel();

            RestClient client = new RestClient(statusUrl)
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
                        if (response.ResponseUri.ToString() != statusUrl)
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

        public string AuthorizeClient()
        {
            _ = HttpUtility.UrlEncode("https://localhost:44309/");

            string URL = clientUrl + $"?response_type=code&client_id=0892d33bf42661ca19165c086e19c4cd9fd98afd949dae7b1492404b63f70714";
            RestClient client = new RestClient(URL)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            RestResponse response = (RestResponse)client.Execute(request);
      

            if (response != null)
            {

                if (response.ResponseStatus == ResponseStatus.Completed)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        //if (response.ResponseUri.ToString() != (URL))
                        //{
                        //ret.genericResult.Message = "Bilimp Adresi geçersiz";
                        //return ret;
                        //return "Error";
                        //}

                        //JavaScriptSerializer ser = new JavaScriptSerializer();
                        //ser.MaxJsonLength = Int32.MaxValue;
                        //ret = ser.Deserialize<ExchangeInfoModel>(response.Content);
                        //return ret;
                        return response.Content;
                    }
                    else
                    {
                        //ret.genericResult.IsOK = false;
                        //ret.genericResult.Message = RequestError(response, "CheckServer");
                        //return ret;
                        return "Error";
                    }
                }
                else
                {
                    //ret.genericResult.IsOK = false;
                    //ret.genericResult.Message = RequestError(response, "CheckServer");
                    //return ret;
                    return "Error";
                }
            }
            else
            {
                //log.WriteLog(LOGTYPE.ERROR, "CheckServer -> Response = null  -> URL: " + _ApiURL + _CheckServerPath);
                //ret.genericResult.IsOK = false;
                //ret.genericResult.Message = "Hata";
                //return ret;
                return "Error";
            }
        }

        public string CreateOrder(string ApiKey, string Timestamp, string Sign, object body)
        {
            string URL = orderUrl;

            RestClient client = new RestClient(URL)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.POST);
            _ = request.AddHeader("Content-Type", "application/json");
            _ = request.AddHeader("CB-ACCESS-KEY", ApiKey);
            _ = request.AddHeader("CB-ACCESS-TIMESTAMP", Timestamp);
            _ = request.AddHeader("CB-ACCESS-SIGN", Sign);
            //request.AddHeader("CB-ACCESS-PASSPHRASE", Passphrase);

            _ = request.AddJsonBody(body);

            RestResponse response = (RestResponse)client.Execute(request);

            if (response != null)
            {

                if (response.ResponseStatus == ResponseStatus.Completed)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        //if (response.ResponseUri.ToString() != (URL))
                        //{
                        //ret.genericResult.Message = "Bilimp Adresi geçersiz";
                        //return ret;
                        //return "Error";
                        //}

                        //JavaScriptSerializer ser = new JavaScriptSerializer();
                        //ser.MaxJsonLength = Int32.MaxValue;
                        //ret = ser.Deserialize<ExchangeInfoModel>(response.Content);
                        //return ret;
                        return response.Content;
                    }
                    else
                    {
                        //ret.genericResult.IsOK = false;
                        //ret.genericResult.Message = RequestError(response, "CheckServer");
                        //return ret;
                        return "Error";
                    }
                }
                else
                {
                    //ret.genericResult.IsOK = false;
                    //ret.genericResult.Message = RequestError(response, "CheckServer");
                    //return ret;
                    return "Error";
                }
            }
            else
            {
                //log.WriteLog(LOGTYPE.ERROR, "CheckServer -> Response = null  -> URL: " + _ApiURL + _CheckServerPath);
                //ret.genericResult.IsOK = false;
                //ret.genericResult.Message = "Hata";
                //return ret;
                return "Error";
            }
        }

    }
}