namespace MMakerBotPanel.WebServices.Kucoin
{
    using MMakerBotPanel.Business;
    using MMakerBotPanel.Models;
    using MMakerBotPanel.WebServices.Kucoin.Interface;
    using MMakerBotPanel.WebServices.Kucoin.Model;
    using Newtonsoft.Json;
    using RestSharp;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web.Script.Serialization;

    public class KucoinWebServices : KucoinWebServicesAbstract
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
            Logger.GetLogger().LogApi(LOGAPITYPE.Kucoin, "", "", baseURL + symbolURL);
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
            RestClient client = new RestClient(baseURL + orderBookURL + $"?symbol={symbol}")
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            _ = request.AddHeader("Content-type", "application/json");
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Kucoin, "", "", baseURL + orderBookURL + $"?symbol={symbol}");
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
            Logger.GetLogger().LogApi(LOGAPITYPE.Kucoin, "", "", baseURL + tradeURL + $"?symbol={symbol}");
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
        public StockDataModel StockData(string symbol, string type, string startAt)
        {
            StockDataModel ret = new StockDataModel();
            RestClient client = new RestClient(baseURL + stockDataURL + $"?symbol={symbol}&type={type}&startAt={startAt}")
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            _ = request.AddHeader("Content-type", "application/json");
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Kucoin, "", "", baseURL + stockDataURL + $"?symbol={symbol}&type={type}&startAt={startAt}");
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
                            addData.startTime = Convert.ToInt64(dataItem[0], CI);
                            addData.openingPrice = Convert.ToDouble(dataItem[1], CI);
                            addData.closingPrice = Convert.ToDouble(dataItem[2], CI);
                            addData.highestPrice = Convert.ToDouble(dataItem[3], CI);
                            addData.lowestPrice = Convert.ToDouble(dataItem[4], CI);
                            addData.transactionVolume = (long)Convert.ToDouble(dataItem[5], CI);
                            addData.transactionAmount = Convert.ToDouble(dataItem[6], CI);
                            ret.KucoinDataDetailModels.Add(addData);
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
        public CreateOrderModel CreateOrder(string apiKey, string signature, string timestamp, string passPhrase, object body)
        {
            CreateOrderModel ret = new CreateOrderModel();
            RestClient client = new RestClient(baseURL + createOrderURL)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.POST);
            _ = request.AddHeader("Content-Type", "application/json");
            _ = request.AddHeader("KC-API-KEY", apiKey);
            _ = request.AddHeader("KC-API-SIGN", signature);
            _ = request.AddHeader("KC-API-TIMESTAMP", timestamp);
            _ = request.AddHeader("KC-API-PASSPHRASE", passPhrase);
            _ = request.AddHeader("KC-API-KEY-VERSION", "2");
            _ = request.AddJsonBody(body);
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Kucoin, "", "", baseURL + createOrderURL);
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
        public ActiveOrderModel ActiveOrder(string apiKey, string signature, string timestamp, string passPhrase)
        {
            ActiveOrderModel ret = new ActiveOrderModel();
            RestClient client = new RestClient(baseURL + createOrderURL + "?status=active")
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            _ = request.AddHeader("Content-Type", "application/json");
            _ = request.AddHeader("KC-API-KEY", apiKey);
            _ = request.AddHeader("KC-API-SIGN", signature);
            _ = request.AddHeader("KC-API-TIMESTAMP", timestamp);
            _ = request.AddHeader("KC-API-PASSPHRASE", passPhrase);
            _ = request.AddHeader("KC-API-KEY-VERSION", "2");
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Kucoin, "", "", baseURL + createOrderURL + "?status=active");
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
        public BalanceModel Balance(string apiKey, string signature, string timestamp, string passPhrase)
        {
            BalanceModel ret = new BalanceModel();
            RestClient client = new RestClient(baseURL + balanceURL)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            _ = request.AddHeader("Content-Type", "application/json");
            _ = request.AddHeader("KC-API-KEY", apiKey);
            _ = request.AddHeader("KC-API-SIGN", signature);
            _ = request.AddHeader("KC-API-TIMESTAMP", timestamp);
            _ = request.AddHeader("KC-API-PASSPHRASE", passPhrase);
            _ = request.AddHeader("KC-API-KEY-VERSION", "2");
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Kucoin, "", "", baseURL + balanceURL);
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
        public CancelOrderModel CancelOrder(string apiKey, string signature, string timestamp, string passPhrase , string symbol)
        {
            CancelOrderModel ret = new CancelOrderModel();
            RestClient client = new RestClient(baseURL + cancelorderURL + $"?symbol={symbol}")
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.DELETE);
            _ = request.AddHeader("Content-Type", "application/json");
            _ = request.AddHeader("KC-API-KEY", apiKey);
            _ = request.AddHeader("KC-API-SIGN", signature);
            _ = request.AddHeader("KC-API-TIMESTAMP", timestamp);
            _ = request.AddHeader("KC-API-PASSPHRASE", passPhrase);
            _ = request.AddHeader("KC-API-KEY-VERSION", "2");
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Kucoin, "", "", baseURL + cancelorderURL + $"?symbol={symbol}");
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
        public QueryOrderModel QueryOrder(string apiKey, string signature, string timestamp, string passPhrase , string orderId)
        {
            QueryOrderModel ret = new QueryOrderModel();
            RestClient client = new RestClient(baseURL + queryorderURL + $"/{orderId}")
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            _ = request.AddHeader("Content-Type", "application/json");
            _ = request.AddHeader("KC-API-KEY", apiKey);
            _ = request.AddHeader("KC-API-SIGN", signature);
            _ = request.AddHeader("KC-API-TIMESTAMP", timestamp);
            _ = request.AddHeader("KC-API-PASSPHRASE", passPhrase);
            _ = request.AddHeader("KC-API-KEY-VERSION", "2");
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Kucoin, "", "", baseURL + queryorderURL + $"/{orderId}");
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