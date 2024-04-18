namespace MMakerBotPanel.WebServices.Upbit
{
    using MMakerBotPanel.Business;
    using MMakerBotPanel.Models;
    using MMakerBotPanel.WebServices.Upbit.Interface;
    using MMakerBotPanel.WebServices.Upbit.Model;
    using Newtonsoft.Json;
    using RestSharp;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web.Script.Serialization;

    public class UpbitWebServices : UpbitWebServicesAbstract
    {
        public UpbitSymbolModel getParity()
        {

            UpbitSymbolModel ret = new UpbitSymbolModel();

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
                        ret.UpbitSymbolDetailModels = ser.Deserialize<List<UpbitSymbolDetailModel>>(response.Content);
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
                        ret.statusUpbitModels = ser.Deserialize<List<StatusUpbitModel>>(response.Content);

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
        public UpbitDataModel getStockData(string symbol, string interval)
        {
            _ = new List<UpbitDataDetailModel>();
            UpbitDataModel ret = new UpbitDataModel();

            RestClient client = new RestClient(BaseURL + DataUrl + interval + "?market=" + symbol + "&count=1000")
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
                        List<UpbitDataDetailModel> data = ser.Deserialize<List<UpbitDataDetailModel>>(response.Content);


                        foreach (UpbitDataDetailModel item in data)
                        {

                            UpbitChartDataModel addData = new UpbitChartDataModel();


                            CultureInfo CI = new CultureInfo("en-US", false);

                            addData.time = Convert.ToInt64(item.timestamp, CI);
                            addData.open = Convert.ToDouble(item.opening_price, CI);
                            addData.high = Convert.ToDouble(item.high_price, CI);
                            addData.low = Convert.ToDouble(item.low_price, CI);
                            addData.close = Convert.ToDouble(item.trade_price, CI);
                            addData.volume = (long)Convert.ToDouble(item.candle_acc_trade_volume, CI);
                            ret.UpbitChartDataModels.Add(addData);

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
        public UpbitDataModel getStockDataDate(string symbol, string interval)
        {
            _ = new List<UpbitDataDetailModel>();
            UpbitDataModel ret = new UpbitDataModel();

            RestClient client = new RestClient(BaseURL + DataUrl + interval + "?market=" + symbol + "&count=1")
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
                        List<UpbitDataDetailModel> data = ser.Deserialize<List<UpbitDataDetailModel>>(response.Content);


                        foreach (UpbitDataDetailModel item in data)
                        {

                            UpbitChartDataModel addData = new UpbitChartDataModel();


                            CultureInfo CI = new CultureInfo("en-US", false);

                            addData.time = Convert.ToInt64(item.timestamp, CI);
                            addData.open = Convert.ToDouble(item.opening_price, CI);
                            addData.high = Convert.ToDouble(item.high_price, CI);
                            addData.low = Convert.ToDouble(item.low_price, CI);
                            addData.close = Convert.ToDouble(item.trade_price, CI);
                            addData.volume = (long)Convert.ToDouble(item.candle_acc_trade_volume, CI);
                            ret.UpbitChartDataModels.Add(addData);

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
    }
}