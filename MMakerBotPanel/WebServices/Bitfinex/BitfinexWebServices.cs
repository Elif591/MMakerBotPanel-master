namespace MMakerBotPanel.WebServices.Bitfinex
{
    using MMakerBotPanel.Business;
    using MMakerBotPanel.Models;
    using MMakerBotPanel.WebServices.Bitfinex.Interface;
    using MMakerBotPanel.WebServices.Bitfinex.Model;
    using RestSharp;
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Web.Script.Serialization;

    public class BitfinexWebServices : BitfinexWebServicesAbstract
    {
        public SymbolModel Parity(string symbol)
        {
            SymbolModel ret = new SymbolModel();
            _ = new List<List<object>>();
            RestClient client = new RestClient(baseURL + symbolURL + $"?symbols={symbol}")
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            _ = request.AddHeader("Content-type", "application/json");
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Bitfinex, "", "", baseURL + symbolURL + $"?symbols={symbol}");
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
                        ret.BitfinexSymbolDataModels.Data = data;
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
            _ = new List<List<double>>();
            StockDataModel ret = new StockDataModel();
            RestClient client = new RestClient(baseURL + stockDataURL + $"/trade:{interval}:{symbol}/hist?limit={limit}")
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            //request.AddHeader("Content-type", "application/json");
            _ = request.AddHeader("accept", "application/json");
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Bitfinex, "", "", baseURL + stockDataURL + $"/trade:{interval}:{symbol}/hist?limit={limit}");
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
                        List<List<double>> data = ser.Deserialize<List<List<double>>>(response.Content);
                        foreach (List<double> item in data)
                        {
                            StockDataDetail addData = new StockDataDetail();
                            double[] dataItem = item.ToArray();
                            CultureInfo CI = new CultureInfo("en-US", false);
                            addData.time = Convert.ToInt64(dataItem[0], CI);
                            addData.volume = (long)Convert.ToDouble(dataItem[5], CI);
                            addData.low = Convert.ToDouble(dataItem[4], CI);
                            addData.close = Convert.ToDouble(dataItem[2], CI);
                            addData.high = Convert.ToDouble(dataItem[3], CI);
                            addData.open = Convert.ToDouble(dataItem[1], CI);
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
            Logger.GetLogger().LogApi(LOGAPITYPE.Bitfinex, "", "", baseURL + statusURL);
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
                        ret.status = ser.Deserialize<List<int>>(response.Content);

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
        public TimeModel Time()
        {
            TimeModel ret = new TimeModel();
            RestClient client = new RestClient(baseURL + timeURL + "?limit=1")
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            _ = request.AddHeader("Content-type", "application/json");
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Bitfinex, "", "", baseURL + timeURL + "?limit=1");
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
                        ret.time = ser.Deserialize<List<List<double>>>(response.Content);

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
        public CreateOrderModel CreateOrder(string timestamp, string apiKey, string sign, object body)
        {
            CreateOrderModel ret = new CreateOrderModel();
            RestClient client = new RestClient(baseApiURL + createOrderURL)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.POST);
            _ = request.AddHeader("Content-Type", "application/json");
            _ = request.AddHeader("bfx-nonce", timestamp);
            _ = request.AddHeader("bfx-apikey", apiKey);
            _ = request.AddHeader("bfx-signature", sign);
            _ = request.AddJsonBody(body);
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Bitfinex, "", "", baseApiURL + createOrderURL);
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
        public BalanceModel Balance(string timestamp, string apiKey, string sign)
        {
            BalanceModel ret = new BalanceModel();
            RestClient client = new RestClient(baseURL + balanceURL)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.POST);
            _ = request.AddHeader("Content-Type", "application/json");
            _ = request.AddHeader("bfx-nonce", timestamp);
            _ = request.AddHeader("bfx-apikey", apiKey);
            _ = request.AddHeader("bfx-signature", sign);
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Bitfinex, "", "", baseURL + balanceURL);
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
        public ActiveOrderModel ActiveOrder(string timestamp, string apiKey, string sign , string symbol)
        {
            ActiveOrderModel ret = new ActiveOrderModel();
            RestClient client = new RestClient(baseURL + activeOrderURL + $"/{symbol}")
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.POST);
            _ = request.AddHeader("Content-Type", "application/json");
            _ = request.AddHeader("bfx-nonce", timestamp);
            _ = request.AddHeader("bfx-apikey", apiKey);
            _ = request.AddHeader("bfx-signature", sign);
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Bitfinex, "", "", baseURL + activeOrderURL);
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
        public OrderBookModel OrderBook(string symbol)
        {
            OrderBookModel ret = new OrderBookModel();
            RestClient client = new RestClient(baseURL + orderBookURL + $"/{symbol}/P0?len=100")
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            _ = request.AddHeader("Content-type", "application/json");
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Bitfinex, "", "", baseURL + orderBookURL + $"/{symbol}/P0?len=100");
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
        public TradeModel Trade(string symbol)
        {
            TradeModel ret = new TradeModel();
            RestClient client = new RestClient(baseURL + tradesURL + $"/{symbol}/hist?limit=1000")
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            _ = request.AddHeader("Content-type", "application/json");
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Bitfinex, "", "", baseURL + tradesURL + $"/{symbol}/hist?limit=1000");
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
                        ret.Trades = ser.Deserialize<List<List<double>>>(response.Content);

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
        public CancelOrderModel CancelOrder(string timestamp, string apiKey, string sign)
        {
            CancelOrderModel ret = new CancelOrderModel();
            RestClient client = new RestClient(baseURL + cancelOrdersURL)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.POST);
            _ = request.AddHeader("Content-Type", "application/json");
            _ = request.AddHeader("bfx-nonce", timestamp);
            _ = request.AddHeader("bfx-apikey", apiKey);
            _ = request.AddHeader("bfx-signature", sign);
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Bitfinex, "", "", baseURL + cancelOrdersURL);
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
        public QueryOrderModel QueryOrder(string timestamp, string apiKey, string sign , object body)
        {
            QueryOrderModel ret = new QueryOrderModel();
            RestClient client = new RestClient(baseURL + queryOrderURL)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.POST);
            _ = request.AddHeader("Content-Type", "application/json");
            _ = request.AddHeader("bfx-nonce", timestamp);
            _ = request.AddHeader("bfx-apikey", apiKey);
            _ = request.AddHeader("bfx-signature", sign);
             request.AddJsonBody(body);
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Bitfinex, "", "", baseURL + queryOrderURL);
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