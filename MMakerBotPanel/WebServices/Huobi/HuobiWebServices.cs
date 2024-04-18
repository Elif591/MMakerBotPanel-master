namespace MMakerBotPanel.WebServices.Huobi
{
    using MMakerBotPanel.Business;
    using MMakerBotPanel.Models;
    using MMakerBotPanel.WebServices.Huobi.Interface;
    using MMakerBotPanel.WebServices.Huobi.Model;
    using Newtonsoft.Json;
    using RestSharp;
    using System;
    using System.Globalization;
    using System.Web.Script.Serialization;

    public class HuobiWebServices : HuobiWebServicesAbstract
    {
        public SymbolModel Parity()
        {
            SymbolModel ret = new SymbolModel();
            RestClient client = new RestClient(baseURL + symbolURL)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            _ = request.AddHeader("Content-type", "application/json");
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Huobi , "", "", baseURL + symbolURL);
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
                        ret = ser.Deserialize<SymbolModel>(response.Content);
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
        public StatusModel Status()
        {
            StatusModel ret = new StatusModel();
            RestClient client = new RestClient(baseURL + statusURL)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            _ = request.AddHeader("Content-type", "application/json");
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Huobi, "", "", baseURL + statusURL);
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
        public TradeModel Trade(string symbol)
        {
            TradeModel ret = new TradeModel();
            RestClient client = new RestClient(baseURL + tradeURL + $"?symbol={symbol}&size=500")
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            _ = request.AddHeader("Content-type", "application/json");
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Huobi, "", "", baseURL + tradeURL + $"?symbol={symbol}&size=500");
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
                        ret = ser.Deserialize<TradeModel>(response.Content);
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
        public OrderBookModel OrderBook(string symbol)
        {
            OrderBookModel ret = new OrderBookModel();
            RestClient client = new RestClient(baseURL + orderBookURL + $"?symbol={symbol}&type=step0&depth=20")
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            _ = request.AddHeader("Content-type", "application/json");
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Huobi, "", "", baseURL + orderBookURL + $"?symbol={symbol}&type=step0&depth=20");
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
        public StockDataModel StockData(string symbol, string interval, string limit)
        {
            StockDataModel ret = new StockDataModel();
            RestClient client = new RestClient(baseURL + stockDataURL + $"?period={interval}&size={limit}&symbol={symbol}")
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            _ = request.AddHeader("Content-type", "application/json");
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Huobi, "", "", baseURL + stockDataURL + $"?period={interval}&size={limit}&symbol={symbol}");
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
                        ret.HuobiChartDataDetailModels = ser.Deserialize<StockDataDetail>(response.Content);
                        foreach (Data item in ret.HuobiChartDataDetailModels.data)
                        {
                            ChartData addData = new ChartData();
                            CultureInfo CI = new CultureInfo("en-US", false);
                            addData.time = Convert.ToInt64(item.id, CI);
                            addData.volume = (long)Convert.ToDouble(item.vol, CI);
                            addData.close = Convert.ToDouble(item.close, CI);
                            addData.high = Convert.ToDouble(item.high, CI);
                            addData.low = Convert.ToDouble(item.low, CI);
                            addData.open = Convert.ToDouble(item.open, CI);
                            ret.HuobiChartModels.Add(addData);
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
        public AccountModel Account(string accountBody)
        {
            AccountModel ret = new AccountModel();
            RestClient client = new RestClient(baseURL + accountURL + $"?{accountBody}")
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Huobi, "", "", baseURL + accountURL + $"?{accountBody}");
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
                        ret = ser.Deserialize<AccountModel>(response.Content);
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
        public CreateOrderModel CreateOrder(object body, string orderBody)
        {
            string URL = baseURL + createOrderURL + $"?{orderBody}";
            CreateOrderModel ret = new CreateOrderModel();
            RestClient client = new RestClient(URL)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.POST);
            _ = request.AddHeader("Content-Type", "application/json");
            _ = request.AddJsonBody(body);
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Huobi, "", "" , baseURL + createOrderURL + $"?{orderBody}");
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
                        ret = ser.Deserialize<CreateOrderModel>(response.Content);
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
        public BalanceModel Balance(string endPoint, string walletBody)
        {
            string URL = baseURL + $"{endPoint}?{walletBody}";
            BalanceModel ret = new BalanceModel();
            RestClient client = new RestClient(URL)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            _ = request.AddHeader("Content-Type", "application/json");
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Huobi, "", "", baseURL + $"{endPoint}?{walletBody}");
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
        public ActiveOrderModel ActiveOrder(string activeOrderBody)
        {
            string URL = baseURL + activeOrderURL + $"?{activeOrderBody}";
            ActiveOrderModel ret = new ActiveOrderModel();
            RestClient client = new RestClient(URL)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            _ = request.AddHeader("Content-Type", "application/json");
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Huobi, "", "" , baseURL + activeOrderURL + $"?{activeOrderBody}");
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
                        ret = ser.Deserialize<ActiveOrderModel>(response.Content);
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
        public CancelOrderModel CancelOrders(object body, string orderBody)
        {
            string URL = baseURL + cancelOrderURL + $"?{orderBody}";
            CancelOrderModel ret = new CancelOrderModel();
            RestClient client = new RestClient(URL)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.POST);
            _ = request.AddHeader("Content-Type", "application/json");
            _ = request.AddJsonBody(body);
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Huobi, "", "", baseURL + cancelOrderURL + $"?{orderBody}");
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
                        ret = ser.Deserialize<CancelOrderModel>(response.Content);
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
        public QueryOrderModel QueryOrder(object endpoint, string sign)
        {
            string URL = baseURL + endpoint + $"?{sign}";
            QueryOrderModel ret = new QueryOrderModel();
            RestClient client = new RestClient(URL)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            _ = request.AddHeader("Content-Type", "application/json");
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Huobi, "", "", baseURL + endpoint + $"?sign");
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
    }
}