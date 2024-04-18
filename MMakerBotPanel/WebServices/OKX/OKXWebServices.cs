namespace MMakerBotPanel.WebServices.OKX
{
    using MMakerBotPanel.Business;
    using MMakerBotPanel.Models;
    using MMakerBotPanel.WebServices.OKX.Interface;
    using MMakerBotPanel.WebServices.OKX.Model;
    using Newtonsoft.Json;
    using RestSharp;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web.Script.Serialization;

    public class OKXWebServices : OKXWebServicesAbstract
    {
        public SymbolModel Parity()
        {
            SymbolModel ret = new SymbolModel();
            RestClient client = new RestClient(baseURL + symbolURL + "?instType=SPOT")
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            _ = request.AddHeader("Content-type", "application/json");
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.OKX, "","", baseURL + symbolURL + "?instType=SPOT");
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
        public SymbolModel GetParity(string symbol)
        {
            SymbolModel ret = new SymbolModel();
            RestClient client = new RestClient(baseURL + parityURL + "?instId="+ symbol + "-SPOT")
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            _ = request.AddHeader("Content-type", "application/json");
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.OKX, "", "", baseURL + symbolURL + "?instId=" + symbol + "-SPOT");
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
            Logger.GetLogger().LogApi(LOGAPITYPE.OKX, "","", baseURL + statusURL);
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
            RestClient client = new RestClient(baseURL + tradeURL + $"?instId={symbol}&limit=500")
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            _ = request.AddHeader("Content-type", "application/json");
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.OKX, "","", baseURL + tradeURL + $"?instId={symbol}&limit=500");
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
            RestClient client = new RestClient(baseURL + orderBookURL + $"?instId={symbol}&sz=400")
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            _ = request.AddHeader("Content-type", "application/json");
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.OKX, "","", baseURL + orderBookURL + $"?instId={symbol}&sz=400");
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
            RestClient client = new RestClient(baseURL + stockDataURL + $"?instId={symbol}&bar={interval}&limit={limit}")
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            _ = request.AddHeader("Content-type", "application/json");
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.OKX, "","", baseURL + stockDataURL + $"?instId={symbol}&bar={interval}&limit={limit}");
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
                        ret = ser.Deserialize<StockDataModel>(response.Content);
                        foreach (List<string> item in ret.data)
                        {
                            StockDataDetail addData = new StockDataDetail();

                            object[] dataItem = item.ToArray();
                            CultureInfo CI = new CultureInfo("en-US", false);
                            addData.Time = Convert.ToInt64(dataItem[0]);
                            addData.openPrice = Convert.ToDouble(dataItem[1], CI);
                            addData.highPrice = Convert.ToDouble(dataItem[2], CI);
                            addData.lowPrice = Convert.ToDouble(dataItem[3], CI);
                            addData.closePrice = Convert.ToDouble(dataItem[4], CI);
                            addData.Volume = (long)Convert.ToDouble(dataItem[5], CI);
                            ret.OKXChartDataDetailModels.Add(addData);
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
        public CreateOrderModel CreateOrder(string ApiKey, string Timestamp, string Sign, object body, string PassPhrase)
        {
            string URL = baseURL + createOrderURL;
            CreateOrderModel ret = new CreateOrderModel();
            RestClient client = new RestClient(URL)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.POST);
            _ = request.AddHeader("Content-Type", "application/json");
            _ = request.AddHeader("OK-ACCESS-KEY", ApiKey);
            _ = request.AddHeader("OK-ACCESS-SIGN", Sign);
            _ = request.AddHeader("OK-ACCESS-TIMESTAMP", Timestamp);
            _ = request.AddHeader("OK-ACCESS-PASSPHRASE", PassPhrase);
            _ = request.AddJsonBody(body);
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.OKX, "","", baseURL + createOrderURL);
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
        public BalanceModel Balance(string ApiKey, string Timestamp, string Sign, string PassPhrase)
        {
            string URL = baseURL + balanceURL;
            BalanceModel ret = new BalanceModel();
            RestClient client = new RestClient(URL)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            _ = request.AddHeader("Content-Type", "application/json");
            _ = request.AddHeader("OK-ACCESS-KEY", ApiKey);
            _ = request.AddHeader("OK-ACCESS-SIGN", Sign);
            _ = request.AddHeader("OK-ACCESS-TIMESTAMP", Timestamp);
            _ = request.AddHeader("OK-ACCESS-PASSPHRASE", PassPhrase);
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.OKX, "","", baseURL + balanceURL);
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
        public ActiveOrderModel ActiveOrder(string ApiKey, string Timestamp, string Sign, string PassPhrase)
        {
            string URL = baseURL + activeOrderURL + "?instType=SPOT";
            ActiveOrderModel ret = new ActiveOrderModel();
            RestClient client = new RestClient(URL)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            _ = request.AddHeader("Content-Type", "application/json");
            _ = request.AddHeader("OK-ACCESS-KEY", ApiKey);
            _ = request.AddHeader("OK-ACCESS-SIGN", Sign);
            _ = request.AddHeader("OK-ACCESS-TIMESTAMP", Timestamp);
            _ = request.AddHeader("OK-ACCESS-PASSPHRASE", PassPhrase);
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.OKX, "","", baseURL + activeOrderURL + "?instType=SPOT");
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
        public CancelOrderModel CancelOrders(string ApiKey, string Timestamp, string Sign, string PassPhrase ,object body)
        {
            string URL = baseURL + cancelOrderURL;
            CancelOrderModel ret = new CancelOrderModel();
            RestClient client = new RestClient(URL)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.POST);
            _ = request.AddHeader("Content-Type", "application/json");
            _ = request.AddHeader("OK-ACCESS-KEY", ApiKey);
            _ = request.AddHeader("OK-ACCESS-SIGN", Sign);
            _ = request.AddHeader("OK-ACCESS-TIMESTAMP", Timestamp);
            _ = request.AddHeader("OK-ACCESS-PASSPHRASE", PassPhrase);
            _ = request.AddJsonBody(body);
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.OKX, "", "", baseURL + cancelOrderURL);
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
        public QueryOrderModel QueryOrder(string ApiKey, string Timestamp, string Sign, string PassPhrase , string orderId, string symbol)
        {
            string URL = baseURL + queryOrderURL + $"?ordId={orderId}&instId={symbol}";
            QueryOrderModel ret = new QueryOrderModel();
            RestClient client = new RestClient(URL)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            _ = request.AddHeader("Content-Type", "application/json");
            _ = request.AddHeader("OK-ACCESS-KEY", ApiKey);
            _ = request.AddHeader("OK-ACCESS-SIGN", Sign);
            _ = request.AddHeader("OK-ACCESS-TIMESTAMP", Timestamp);
            _ = request.AddHeader("OK-ACCESS-PASSPHRASE", PassPhrase);
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.OKX, "", "", baseURL + queryOrderURL + $"?ordId={orderId}&instId={symbol}");
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
        public ComissionFeeModel ComissionFee(string ApiKey, string Timestamp, string Sign, string PassPhrase, string symbol)
        {
            string URL = baseURL + comissionFeeURL + $"?instType=SPOT&instId={symbol}";
            ComissionFeeModel ret = new ComissionFeeModel();
            RestClient client = new RestClient(URL)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            _ = request.AddHeader("Content-Type", "application/json");
            _ = request.AddHeader("OK-ACCESS-KEY", ApiKey);
            _ = request.AddHeader("OK-ACCESS-SIGN", Sign);
            _ = request.AddHeader("OK-ACCESS-TIMESTAMP", Timestamp);
            _ = request.AddHeader("OK-ACCESS-PASSPHRASE", PassPhrase);
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.OKX, "", "", baseURL + comissionFeeURL + $"?instType=SPOT&instId={symbol}");
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
                        ret = ser.Deserialize<ComissionFeeModel>(response.Content);
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