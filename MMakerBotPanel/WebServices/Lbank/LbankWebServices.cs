namespace MMakerBotPanel.WebServices.Lbank
{
    using MMakerBotPanel.Business;
    using MMakerBotPanel.Models;
    using MMakerBotPanel.WebServices.Lbank.Interface;
    using MMakerBotPanel.WebServices.Lbank.Model;
    using Newtonsoft.Json;
    using RestSharp;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web.Script.Serialization;

    public class LbankWebServices : LbankWebServicesAbstract
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
            Logger.GetLogger().LogApi(LOGAPITYPE.Lbank, "", "", baseURL + symbolURL);
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
        public GetPairModel GetPair(string assestCode)
        {
            GetPairModel ret = new GetPairModel();
            RestClient client = new RestClient(baseURL + pairURL + $"?assetCode={assestCode}")
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            _ = request.AddHeader("Content-type", "application/json");
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Lbank, "", "", baseURL + pairURL + $"?assetCode={assestCode}");
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
                        ret = ser.Deserialize<GetPairModel>(response.Content);
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
            RestRequest request = new RestRequest(Method.POST);
            _ = request.AddHeader("Content-type", "application/json");
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Kucoin, "", "", baseURL + statusURL);
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
        public OrderBookModel OrderBook(string symbol)
        {
            OrderBookModel ret = new OrderBookModel();
            RestClient client = new RestClient(baseURL + orderBookURL + $"?symbol={symbol}&size=60")
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            _ = request.AddHeader("Content-type", "application/json");
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Kucoin, "", "", baseURL + orderBookURL + $"?symbol={symbol}&size=60");
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
            Logger.GetLogger().LogApi(LOGAPITYPE.Kucoin, "", "", baseURL + tradeURL + $"?symbol={symbol}&size=500");
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
        public StockDataModel StockData(string symbol, string interval, string limit, string time)
        {
            _ = new StockDataDetail();
            StockDataModel ret = new StockDataModel();
            RestClient client = new RestClient(baseURL + stockDataURL + $"?symbol={symbol}&type={interval}&size={limit}&time={time}")
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            _ = request.AddHeader("Content-type", "application/json");
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Kucoin, "", "", baseURL + stockDataURL + $"?symbol={symbol}&type={interval}&size={limit}&time={time}");
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
                        StockDataDetail data = ser.Deserialize<StockDataDetail>(response.Content);
                        foreach (List<double> item in data.data)
                        {
                            ChartData addData = new ChartData();
                            double[] dataItem = item.ToArray();
                            CultureInfo CI = new CultureInfo("en-US", false);
                            addData.time = Convert.ToInt64(dataItem[0], CI);
                            addData.open = Convert.ToDouble(dataItem[1], CI);
                            addData.high = Convert.ToDouble(dataItem[2], CI);
                            addData.low = Convert.ToDouble(dataItem[3], CI);
                            addData.close = Convert.ToDouble(dataItem[4], CI);
                            addData.volume = (long)Convert.ToDouble(dataItem[5], CI);
                            ret.LbankChartDataModels.Add(addData);

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
        public TimeModel Time()
        {
            TimeModel ret = new TimeModel();
            RestClient client = new RestClient(baseURL + timeURL)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            _ = request.AddHeader("Content-type", "application/json");
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Kucoin, "", "", baseURL + timeURL);
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
                        ret = ser.Deserialize<TimeModel>(response.Content);
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
        public CreateOrderModel CreateOrder(string parametreStr)
        {
            CreateOrderModel ret = new CreateOrderModel();
            RestClient client = new RestClient(baseURL + createOrderURL + "?" + parametreStr)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.POST);
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Kucoin, "", "", baseURL + createOrderURL + "?" + parametreStr);
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
        public BalanceModel Balance(string parametreStr)
        {
            RestClient client = new RestClient(baseURL + balanceURL + "?" + parametreStr)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.POST);
            BalanceModel ret = new BalanceModel();
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Kucoin, "", "", baseURL + balanceURL + "?" + parametreStr);
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
                            ret.balances = JsonConvert.DeserializeObject<List<SpotBalances>>(response.Content);
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
        public ActiveOrderModel ActiveOrder(string parametreStr)
        {
            RestClient client = new RestClient(baseURL + activeOrderURL + "?" + parametreStr)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.POST);
            ActiveOrderModel ret = new ActiveOrderModel();
            _ = new ActiveOrders();
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Kucoin, "", "", baseURL + activeOrderURL + "?" + parametreStr);
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
                            ret.activeOrders = JsonConvert.DeserializeObject<List<ActiveOrders>>(response.Content);
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
        public CancelOrderModel CancelOrder(string parametreStr)
        {
            CancelOrderModel ret = new CancelOrderModel();
            RestClient client = new RestClient(baseURL + cancelOrderURL + "?" + parametreStr)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.POST);
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Kucoin, "", "", baseURL + cancelOrderURL + "?" + parametreStr);
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
        public QueryOrderModel QueryOrder(string parametreStr)
        {
            QueryOrderModel ret = new QueryOrderModel();
            RestClient client = new RestClient(baseURL + queryOrderURL + "?" + parametreStr)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.POST);
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Kucoin, "", "", baseURL + queryOrderURL + "?" + parametreStr);
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