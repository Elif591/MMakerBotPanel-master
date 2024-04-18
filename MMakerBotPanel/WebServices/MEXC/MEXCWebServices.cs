namespace MMakerBotPanel.WebServices.MEXC
{
    using MMakerBotPanel.Business;
    using MMakerBotPanel.Models;
    using MMakerBotPanel.WebServices.MEXC.Interface;
    using MMakerBotPanel.WebServices.MEXC.Model;
    using RestSharp;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web.Script.Serialization;

    public class MEXCWebServices : MEXCWebServicesAbstract
    {
        public MexcSymbolModel getParity()
        {

            MexcSymbolModel ret = new MexcSymbolModel();

            RestClient client = new RestClient(BaseURL + SymbolsUrl)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            _ = request.AddHeader("Content-type", "application/json");

            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Mexc, "", "", BaseURL + SymbolsUrl);

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
                        ret = ser.Deserialize<MexcSymbolModel>(response.Content);
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

            RestClient client = new RestClient(BaseApiURL + statusUrl)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            _ = request.AddHeader("Content-type", "application/json");

            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Mexc, "", "", BaseURL + statusUrl);

            if (response != null)
            {
                if (response.ResponseStatus == ResponseStatus.Completed)
                {

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        if (response.ResponseUri.ToString() != (BaseApiURL + statusUrl))
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
        public MexcDataModel getStockData(string symbol, string interval)
        {
            _ = new List<List<object>>();
            MexcDataModel ret = new MexcDataModel();



            RestClient client = new RestClient(BaseApiURL + DataUrl + "?symbol=" + symbol + "&interval=" + interval + "&limit=1000")
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            _ = request.AddHeader("Content-type", "application/json");

            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Mexc, "", "", BaseApiURL + DataUrl + "?symbol=" + symbol + "&interval=" + interval + "&limit=1000");

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
                        List<List<object>> data = ser.Deserialize<List<List<object>>>(response.Content);


                        foreach (List<object> item in data)
                        {

                            MexcDataChartModel addData = new MexcDataChartModel();

                            object[] dataItem = item.ToArray();
                            CultureInfo CI = new CultureInfo("en-US", false);

                            addData.time = Convert.ToInt64(dataItem[0], CI);
                            addData.open = Convert.ToDouble(dataItem[1], CI);
                            addData.high = Convert.ToDouble(dataItem[2], CI);
                            addData.low = Convert.ToDouble(dataItem[3], CI);
                            addData.close = Convert.ToDouble(dataItem[4], CI);
                            addData.volume = (long)Convert.ToDouble(dataItem[5], CI);
                            ret.MexcDataChartModels.Add(addData);

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

        public MexcDataModel getStockDataDate(string symbol, string interval)
        {
            _ = new List<List<object>>();
            MexcDataModel ret = new MexcDataModel();



            RestClient client = new RestClient(BaseApiURL + DataUrl + "?symbol=" + symbol + "&interval=" + interval + "&limit=1")
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            _ = request.AddHeader("Content-type", "application/json");

            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Mexc, "", "", BaseApiURL + DataUrl + "?symbol=" + symbol + "&interval=" + interval + "&limit=1");

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
                        List<List<object>> data = ser.Deserialize<List<List<object>>>(response.Content);


                        foreach (List<object> item in data)
                        {

                            MexcDataChartModel addData = new MexcDataChartModel();

                            object[] dataItem = item.ToArray();
                            CultureInfo CI = new CultureInfo("en-US", false);

                            addData.time = Convert.ToInt64(dataItem[0], CI);
                            addData.open = Convert.ToDouble(dataItem[1], CI);
                            addData.high = Convert.ToDouble(dataItem[2], CI);
                            addData.low = Convert.ToDouble(dataItem[3], CI);
                            addData.close = Convert.ToDouble(dataItem[4], CI);
                            addData.volume = (long)Convert.ToDouble(dataItem[5], CI);
                            ret.MexcDataChartModels.Add(addData);

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

        public MEXCExchange CreateOrder(string apiKey, string signature, string queryString)
        {
            MEXCExchange ret = new MEXCExchange();
            RestClient client = new RestClient(BaseApiURL + OrderUrl + $"?{queryString}&signature={signature}")
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.POST);
            _ = request.AddHeader("X-MEXC-APIKEY", apiKey);
            _ = request.AddHeader("Content-Type", "application/json");


            RestResponse response = (RestResponse)client.Execute(request);

            Logger.GetLogger().LogApi(LOGAPITYPE.Mexc, "", "", BaseApiURL + OrderUrl + $"?{queryString}&signature={signature}");
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
                        ret = ser.Deserialize<MEXCExchange>(response.Content);
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

        public BalanceModel GetBalance(string apiKey, string signature, string queryString)
        {
            BalanceModel ret = new BalanceModel();
            RestClient client = new RestClient(BaseApiURL + balanceUrl + $"?{queryString}&signature={signature}")
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.POST);
            _ = request.AddHeader("X-MEXC-APIKEY", apiKey);
            _ = request.AddHeader("Content-Type", "application/json");


            RestResponse response = (RestResponse)client.Execute(request);

            Logger.GetLogger().LogApi(LOGAPITYPE.Mexc, "", "", BaseApiURL + balanceUrl + $"?{queryString}&signature={signature}");
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
                        ret = ser.Deserialize<BalanceModel>(response.Content);
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

        public CancelOrdersModel CancelOrders(string apiKey, string signature, string queryString)
        {
            CancelOrdersModel ret = new CancelOrdersModel();
            RestClient client = new RestClient(BaseApiURL + cancelOrdersUrl + $"?{queryString}&signature={signature}")
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.DELETE);
            _ = request.AddHeader("X-MEXC-APIKEY", apiKey);
            _ = request.AddHeader("Content-Type", "application/json");


            RestResponse response = (RestResponse)client.Execute(request);

            Logger.GetLogger().LogApi(LOGAPITYPE.Mexc, "", "", BaseApiURL + cancelOrdersUrl + $"?{queryString}&signature={signature}");
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
                        ret.cancelOrders = ser.Deserialize<List<CancelOrders>>(response.Content);
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
        public ActiveOrdersModel ActiveOrders(string apiKey, string signature, string queryString)
        {
            ActiveOrdersModel ret = new ActiveOrdersModel();
            RestClient client = new RestClient(BaseApiURL + cancelOrdersUrl + $"?{queryString}&signature={signature}")
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            _ = request.AddHeader("X-MEXC-APIKEY", apiKey);
            _ = request.AddHeader("Content-Type", "application/json");


            RestResponse response = (RestResponse)client.Execute(request);

            Logger.GetLogger().LogApi(LOGAPITYPE.Mexc, "", "", BaseApiURL + cancelOrdersUrl + $"?{queryString}&signature={signature}");
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
                        ret.activeOrders = ser.Deserialize<List<ActiveOrders>>(response.Content);
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

        public QueryOrderModel QueryOrders(string apiKey, string signature, string queryString)
        {
            QueryOrderModel ret = new QueryOrderModel();
            RestClient client = new RestClient(BaseApiURL + queryOrderUrl + $"?{queryString}&signature={signature}")
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            _ = request.AddHeader("X-MEXC-APIKEY", apiKey);
            _ = request.AddHeader("Content-Type", "application/json");


            RestResponse response = (RestResponse)client.Execute(request);

            Logger.GetLogger().LogApi(LOGAPITYPE.Mexc, "", "", BaseApiURL + queryOrderUrl + $"?{queryString}&signature={signature}");
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
                        ret = ser.Deserialize<QueryOrderModel>(response.Content);
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

        public OrderBookModel OrderBook( string queryString)
        {
            OrderBookModel ret = new OrderBookModel();
            RestClient client = new RestClient(BaseApiURL + orderbookUrl + $"?{queryString}")
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            _ = request.AddHeader("Content-Type", "application/json");

            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Mexc, "", "", BaseApiURL + orderbookUrl + $"?{queryString}");
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
                        ret = ser.Deserialize<OrderBookModel>(response.Content);
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