namespace MMakerBotPanel.WebServices.Gateio
{
    using MMakerBotPanel.Business;
    using MMakerBotPanel.Models;
    using MMakerBotPanel.WebServices.Gateio.Interface;
    using MMakerBotPanel.WebServices.Gateio.Model;
    using RestSharp;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web.Script.Serialization;

    public class GateioWebServices : GateioWebServicesAbstract
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
            Logger.GetLogger().LogApi(LOGAPITYPE.Gateio, "", "", baseURL + symbolURL);
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
                        ret.data = ser.Deserialize<List<Datum>>(response.Content);
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
            Logger.GetLogger().LogApi(LOGAPITYPE.Gateio, "", "", baseURL + statusURL);
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
        public OrderBookModel OrderBook(string pair)
        {
            OrderBookModel ret = new OrderBookModel();
            RestClient client = new RestClient(baseURL + orderBookURL + $"?currency_pair={pair}&limit=500")
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            _ = request.AddHeader("Accept", "application/json");
            _ = request.AddHeader("Content-type", "application/json");
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Gateio, "", "", baseURL + orderBookURL + $"?currency_pair={pair}&limit=500");
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
        public TradeModel Trade(string pair)
        {
            TradeModel ret = new TradeModel();
            RestClient client = new RestClient(baseURL + tradeURL + $"?currency_pair={pair}&limit=100")
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            _ = request.AddHeader("Accept", "application/json");
            _ = request.AddHeader("Content-type", "application/json");
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Gateio, "", "", baseURL + tradeURL + $"?currency_pair={pair}&limit=100");
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
                        ret.trades = ser.Deserialize<List<TradesData>>(response.Content);
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
            RestClient client = new RestClient(baseURL + stockDataURL + $"?limit={limit}&interval={interval}&currency_pair={symbol}")
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            _ = request.AddHeader("Content-type", "application/json");
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Gateio, "", "", baseURL + stockDataURL + $"?limit={limit}&interval={interval}&currency_pair={symbol}");
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

                        ret.GateioData = ser.Deserialize<List<List<string>>>(response.Content);
                        foreach (List<string> item in ret.GateioData)
                        {
                            StockDataDetail addData = new StockDataDetail();
                            object[] dataItem = item.ToArray();
                            CultureInfo CI = new CultureInfo("en-US", false);
                            addData.startTime = Convert.ToInt64(dataItem[0], CI);
                            addData.tradingVolume = (long)Convert.ToDouble(dataItem[1], CI);
                            addData.closePrice = Convert.ToDouble(dataItem[2], CI);
                            addData.highestPrice = Convert.ToDouble(dataItem[3], CI);
                            addData.lowestPrice = Convert.ToDouble(dataItem[4], CI);
                            addData.openPrice = Convert.ToDouble(dataItem[5], CI);
                            addData.tradingAmount = Convert.ToDouble(dataItem[6], CI);
                            ret.GateioDataDetailModels.Add(addData);
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
        public CreateOrderModel CreateOrder(string Key, string Timestamp, string Sign, object body)
        {
            string URL = baseURL + createOrderURL;
            RestClient client = new RestClient(URL)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.POST);
            CreateOrderModel ret = new CreateOrderModel();
            _ = request.AddHeader("Accept", "application/json");
            _ = request.AddHeader("Content-Type", "application/json");
            _ = request.AddHeader("KEY", Key);
            _ = request.AddHeader("Timestamp", Timestamp);
            _ = request.AddHeader("SIGN", Sign);
            _ = request.AddJsonBody(body);
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Gateio, "", "", baseURL + createOrderURL);
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
        public BalanceModel Balance(string Key, string Timestamp, string Sign)
        {
            string URL = baseURL + balanceURL;
            BalanceModel ret = new BalanceModel();
            RestClient client = new RestClient(URL)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            _ = request.AddHeader("Accept", "application/json");
            _ = request.AddHeader("Content-Type", "application/json");
            _ = request.AddHeader("KEY", Key);
            _ = request.AddHeader("Timestamp", Timestamp);
            _ = request.AddHeader("SIGN", Sign);
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Gateio, "", "", baseURL + balanceURL);
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
        public ActiveOrderModel ActiveOrder(string Key, string Timestamp, string Sign)
        {
            string URL = baseURL + activeOrderURL;
            ActiveOrderModel ret = new ActiveOrderModel();
            RestClient client = new RestClient(URL)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            _ = request.AddHeader("Accept", "application/json");
            _ = request.AddHeader("Content-Type", "application/json");
            _ = request.AddHeader("KEY", Key);
            _ = request.AddHeader("Timestamp", Timestamp);
            _ = request.AddHeader("SIGN", Sign);
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Gateio, "", "", baseURL + activeOrderURL);
            if (response != null)
            {
                if (response.ResponseStatus == ResponseStatus.Completed)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        try
                        {
                            JavaScriptSerializer ser = new JavaScriptSerializer
                            {
                                MaxJsonLength = int.MaxValue
                            };
                            ret = ser.Deserialize<ActiveOrderModel>(response.Content);
                            return ret;
                        }
                        catch (Exception)
                        {
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
        public CancelOrderModel CancelOrders(string Key, string Timestamp, string Sign, string query_param)
        {
            string URL = baseURL + cancelOrderURL + $"?{query_param}";
            RestClient client = new RestClient(URL)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.DELETE);
            CancelOrderModel ret = new CancelOrderModel();
            _ = request.AddHeader("Accept", "application/json");
            _ = request.AddHeader("Content-Type", "application/json");
            _ = request.AddHeader("KEY", Key);
            _ = request.AddHeader("Timestamp", Timestamp);
            _ = request.AddHeader("SIGN", Sign);
            RestResponse response = (RestResponse)client.Execute(request);

            Logger.GetLogger().LogApi(LOGAPITYPE.Gateio, "", "", baseURL + cancelOrderURL + $"?{query_param}");

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
        public QueryOrderModel QueryOrder(string Key, string Timestamp, string Sign, string query_param , string orderId)
        {
            string URL = baseURL + queryOrderURL + $"/{orderId}?{query_param}";
            RestClient client = new RestClient(URL)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.DELETE);
            QueryOrderModel ret = new QueryOrderModel();
            _ = request.AddHeader("Accept", "application/json");
            _ = request.AddHeader("Content-Type", "application/json");
            _ = request.AddHeader("KEY", Key);
            _ = request.AddHeader("Timestamp", Timestamp);
            _ = request.AddHeader("SIGN", Sign);
            RestResponse response = (RestResponse)client.Execute(request);

            Logger.GetLogger().LogApi(LOGAPITYPE.Gateio, "", "", baseURL + queryOrderURL + $"/{orderId}?{query_param}");

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
                        ret = ser.Deserialize<QueryOrderModel> (response.Content);
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
        public PairModel ComissionFee( string query_param)
        {
            string URL = baseURL + symbolURL + $"/{query_param}";
            RestClient client = new RestClient(URL)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            PairModel ret = new PairModel();
            _ = request.AddHeader("Content-Type", "application/json");
            RestResponse response = (RestResponse)client.Execute(request);

            Logger.GetLogger().LogApi(LOGAPITYPE.Gateio, "", "", baseURL + symbolURL + $"/{query_param}");

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
                        ret = ser.Deserialize<PairModel>(response.Content);
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