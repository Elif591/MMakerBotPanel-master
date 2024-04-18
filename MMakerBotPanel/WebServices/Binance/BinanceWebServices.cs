namespace MMakerBotPanel.WebServices.Binance
{
    using MMakerBotPanel.Business;
    using MMakerBotPanel.Models;
    using MMakerBotPanel.WebServices.Binance.Interface;
    using MMakerBotPanel.WebServices.Binance.Model;
    using RestSharp;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web.Script.Serialization;

    public class BinanceWebServices : BinanceWebServicesAbstract
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
            Logger.GetLogger().LogApi(LOGAPITYPE.Binance, "", "", baseURL + symbolURL);
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
        public SymbolModel GetParityQtyFormat(string symbol)
        {
            SymbolModel ret = new SymbolModel();
            RestClient client = new RestClient(baseURL + symbolURL + "?symbol=" + symbol)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            _ = request.AddHeader("Content-type", "application/json");
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Binance, "", "", baseURL + symbolURL);
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
            Logger.GetLogger().LogApi(LOGAPITYPE.Binance, "", "", baseURL + statusURL);
            if (response != null)
            {
                if (response.ResponseStatus == ResponseStatus.Completed)
                {
                    if (response.ContentLength > 0)
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
                        //log.WriteLog(LOGTYPE.ERROR, "CheckServer -> Response ContentLength = 0  -> URL: " + _ApiURL + _CheckServerPath);
                        ret.genericResult.IsOK = false;
                        ret.genericResult.Message = "Hata";
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
            Logger.GetLogger().LogApi(LOGAPITYPE.Binance, "", "", baseURL + timeURL);
            if (response != null)
            {
                if (response.ResponseStatus == ResponseStatus.Completed)
                {
                    if (response.ContentLength > 0)
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
                        //log.WriteLog(LOGTYPE.ERROR, "CheckServer -> Response ContentLength = 0  -> URL: " + _ApiURL + _CheckServerPath);
                        ret.genericResult.IsOK = false;
                        ret.genericResult.Message = "Hata";
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
            _ = new List<List<object>>();
            StockDataModel ret = new StockDataModel();
            RestClient client = new RestClient(baseURL + stockDataURL + $"?symbol={symbol}&interval={interval}&limit={limit}")
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            _ = request.AddHeader("Content-type", "application/json");
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Binance ,"", "", baseURL + stockDataURL + $"?symbol={symbol}&interval={interval}&limit={limit}");
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
                            StockDataDetail addData = new StockDataDetail();
                            object[] dataItem = item.ToArray();
                            CultureInfo CI = new CultureInfo("en-US", false);
                            addData.OpenTime = Convert.ToInt64(dataItem[0], CI);
                            addData.OpenPrice = Convert.ToDouble(dataItem[1], CI);
                            addData.HighPrice = Convert.ToDouble(dataItem[2], CI);
                            addData.LowPrice = Convert.ToDouble(dataItem[3], CI);
                            addData.ClosePrice = Convert.ToDouble(dataItem[4], CI);
                            addData.Volume = (long)Convert.ToDouble(dataItem[5], CI);
                            addData.CloseTime = Convert.ToInt64(dataItem[6], CI);
                            addData.QuoteAssetVolume = Convert.ToDouble(dataItem[7], CI);
                            addData.Trades = Convert.ToInt32(dataItem[8], CI);
                            addData.TakerBuyBaseAssetVolume = Convert.ToDouble(dataItem[9], CI);
                            addData.TakerBuyQuoteAssetVolume = Convert.ToDouble(dataItem[10], CI);
                            ret.stockDataDetailModels.Add(addData);
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
        public CreateOrderModel CreateOrder(string APIKey, string dataQueryString, string signature)
        {
            CreateOrderModel ret = new CreateOrderModel();
            string URL = baseURL + createOrderURL + "?" + dataQueryString + "&signature=" + signature;
            RestClient client = new RestClient(URL)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.POST);
            _ = request.AddHeader("Content-type", "application/json");
            _ = request.AddHeader("X-MBX-APIKEY", APIKey);
            RestResponse response = (RestResponse)client.Execute(request);

            Logger.GetLogger().LogApi(LOGAPITYPE.Binance, "", response.Content, baseURL + createOrderURL + "?" + dataQueryString + "&signature=" + signature);
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
        public CancelOrderModel CancelOrder(string APIKey, string dataQueryString, string signature)
        {
            CancelOrderModel ret = new CancelOrderModel();
            string URL = baseURL + cancelOrderURL + "?" + dataQueryString + "&signature=" + signature;
            RestClient client = new RestClient(URL)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.DELETE);
            _ = request.AddHeader("Content-type", "application/json");
            _ = request.AddHeader("X-MBX-APIKEY", APIKey);
            RestResponse response = (RestResponse)client.Execute(request);

            Logger.GetLogger().LogApi(LOGAPITYPE.Binance, "", "", baseURL + cancelOrderURL + "?" + dataQueryString + "&signature=" + signature);
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
                        ret.cancelOrderListModel = ser.Deserialize<List<CancelOrderListModel>>(response.Content);
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
        public QueryOrderModel QueryOrder(string APIKey, string dataQueryString, string signature)
        {
            QueryOrderModel ret = new QueryOrderModel();
            string URL = baseURL + queryOrderURL + "?" + dataQueryString + "&signature=" + signature;
            RestClient client = new RestClient(URL)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            _ = request.AddHeader("Content-type", "application/json");
            _ = request.AddHeader("X-MBX-APIKEY", APIKey);
            RestResponse response = (RestResponse)client.Execute(request);

            Logger.GetLogger().LogApi(LOGAPITYPE.Binance, "", response.Content, baseURL + queryOrderURL + "?" + dataQueryString + "&signature=" + signature);
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
        public ActiveOrderModel ActiveOrder(string APIKey, string dataQueryString, string signature)
        {
            ActiveOrderModel ret = new ActiveOrderModel();
            string URL = baseURL + activeOrderURL + "?" + dataQueryString + "&signature=" + signature;
            RestClient client = new RestClient(URL)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            _ = request.AddHeader("Content-type", "application/json");
            _ = request.AddHeader("X-MBX-APIKEY", APIKey);
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Binance , "", "", baseURL + activeOrderURL + "?" + dataQueryString + "&signature=" + signature);
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
                        ret.activeOrders = ser.Deserialize<List<ActiveOrder>>(response.Content);
                        List<ActiveOrder> newActiveOrders = new List<ActiveOrder>();
                        foreach (ActiveOrder item in ret.activeOrders)
                        {
                            ActiveOrder activeOrder = new ActiveOrder
                            {
                                price = item.price,
                                time = item.time,
                                clientOrderId = item.clientOrderId,
                                timeInForce = item.timeInForce,
                                orderListId = item.orderListId,
                                orderId = item.orderId,
                                executedQty = item.executedQty,
                                origQuoteOrderQty = item.origQuoteOrderQty,
                                cummulativeQuoteQty = item.cummulativeQuoteQty,
                                icebergQty = item.icebergQty,
                                origQty = item.origQty,
                                status = item.status,
                                type = item.type,
                                side = item.side,
                                stopPrice = item.stopPrice,
                                updateTime = item.updateTime,
                                isWorking = item.isWorking,
                                selfTradePreventionMode = item.selfTradePreventionMode
                            };
                            newActiveOrders.Add(activeOrder);
                        }
                        ret.activeOrders = newActiveOrders;
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
        public BalanceModel Balance(string APIKey, string dataQueryString, string signature)
        {
            BalanceModel ret = new BalanceModel();
            string URL = baseURL + balanceURL + "?" + dataQueryString + "&signature=" + signature;
            RestClient client = new RestClient(URL)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.POST);
            _ = request.AddHeader("Content-type", "application/json");
            _ = request.AddHeader("X-MBX-APIKEY", APIKey);
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Binance, "", "", baseURL + balanceURL + "?" + dataQueryString + "&signature=" + signature);
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
                        ret.getWallets = ser.Deserialize<List<BalanceData>>(response.Content);
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
            RestClient client = new RestClient(baseURL + orderBookURL + $"?symbol={symbol}")
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            _ = request.AddHeader("Content-type", "application/json");
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Binance, "", "", baseURL + orderBookURL + $"?symbol={symbol}");
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
            RestClient client = new RestClient(baseURL + tradeURL + $"?symbol={symbol}")
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            _ = request.AddHeader("Content-type", "application/json");
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Binance, "" ,"", baseURL + tradeURL + $"?symbol={symbol}");
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
        public ComissionModel ComissionFee(string APIKey, string dataQueryString, string signature)
        {
            ComissionModel ret = new ComissionModel();
            string URL = baseURL + comissionURL + "?" + dataQueryString + "&signature=" + signature;
            RestClient client = new RestClient(URL)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            _ = request.AddHeader("Content-type", "application/json");
            _ = request.AddHeader("X-MBX-APIKEY", APIKey);
            RestResponse response = (RestResponse)client.Execute(request);

            Logger.GetLogger().LogApi(LOGAPITYPE.Binance, "", "", baseURL + comissionURL + "?" + dataQueryString + "&signature=" + signature);
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
                        ret.comissionFee = ser.Deserialize<List<ComissionFeeModel>>(response.Content);
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