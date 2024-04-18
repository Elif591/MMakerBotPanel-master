namespace MMakerBotPanel.WebServices.Bitget
{
    using MMakerBotPanel.Business;
    using MMakerBotPanel.Models;
    using MMakerBotPanel.WebServices.Bitget.Interface;
    using MMakerBotPanel.WebServices.Bitget.Model;
    using RestSharp;
    using System.Web.Script.Serialization;

    public class BitgetWebServices : BitgetWebServicesAbstract
    {
        public PairListModel Parity()
        {
            PairListModel ret = new PairListModel();
            RestClient client = new RestClient(baseURL + symbolURL)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            _ = request.AddHeader("Content-type", "application/json");
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Bitfinex, "", "", baseURL + symbolURL);
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
                        ret = ser.Deserialize<PairListModel>(response.Content);
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

        public StockDataModel StockData(string symbol, string period ,string limit)
        {
            StockDataModel ret = new StockDataModel();
            RestClient client = new RestClient(baseURL + stockDataURL + $"?symbol={symbol}&period={period}&limit={limit}")
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            _ = request.AddHeader("Content-type", "application/json");
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Bitget, "", "", baseURL + stockDataURL + $"?symbol={symbol}&period={period}&limit={limit}");
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

        public CreateOrderModel CreateOrder(string APIKey, object dataQueryString, string signature , string passphrase , string timestamp)
        {
            CreateOrderModel ret = new CreateOrderModel();
            string URL = baseURL + createOrderURL;
            RestClient client = new RestClient(URL)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.POST);
            _ = request.AddHeader("ACCESS-KEY", APIKey);
            _ = request.AddHeader("ACCESS-SIGN", signature);
            _ = request.AddHeader("ACCESS-PASSPHRASE", passphrase);
            _ = request.AddHeader("ACCESS-TIMESTAMP", timestamp);
            _ = request.AddHeader("locale", "en-US");
            _ = request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(dataQueryString);   
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Bitget, "", response.Content, baseURL + createOrderURL);
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

        public CancelOrderModel CancelOrder(string APIKey, object dataQueryString, string signature , string passphrase , string timestamp)
        {
            CancelOrderModel ret = new CancelOrderModel();
            string URL = baseURL + cancelOrderURL;
            RestClient client = new RestClient(URL)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.POST);
            _ = request.AddHeader("ACCESS-KEY", APIKey);
            _ = request.AddHeader("ACCESS-SIGN", signature);
            _ = request.AddHeader("ACCESS-PASSPHRASE", passphrase);
            _ = request.AddHeader("ACCESS-TIMESTAMP", timestamp);
            _ = request.AddHeader("locale", "en-US");
            _ = request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(dataQueryString);
            RestResponse response = (RestResponse)client.Execute(request);

            Logger.GetLogger().LogApi(LOGAPITYPE.Bitget, "", "", baseURL + cancelOrderURL);
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

        public QueryOrderModel QueryOrder(string APIKey, object dataQueryString, string signature, string passphrase, string timestamp)
        {
            QueryOrderModel ret = new QueryOrderModel();
            string URL = baseURL + queryOrderURL;
            RestClient client = new RestClient(URL)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.POST);
            _ = request.AddHeader("ACCESS-KEY", APIKey);
            _ = request.AddHeader("ACCESS-SIGN", signature);
            _ = request.AddHeader("ACCESS-PASSPHRASE", passphrase);
            _ = request.AddHeader("ACCESS-TIMESTAMP", timestamp);
            _ = request.AddHeader("locale", "en-US");
            _ = request.AddHeader("Content-Type", "application/json");
            request.AddJsonBody(dataQueryString);
            RestResponse response = (RestResponse)client.Execute(request);

            Logger.GetLogger().LogApi(LOGAPITYPE.Bitget, "", "", baseURL + queryOrderURL);
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

        public BalanceModel GetBalance(string APIKey, object dataQueryString, string signature, string passphrase, string timestamp)
        {
            BalanceModel ret = new BalanceModel();
            string URL = baseURL + balanceURL;
            RestClient client = new RestClient(URL)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.POST);
            _ = request.AddHeader("ACCESS-KEY", APIKey);
            _ = request.AddHeader("ACCESS-SIGN", signature);
            _ = request.AddHeader("ACCESS-PASSPHRASE", passphrase);
            _ = request.AddHeader("ACCESS-TIMESTAMP", timestamp);
            _ = request.AddHeader("locale", "en-US");
            _ = request.AddHeader("Content-Type", "application/json");
           // request.AddJsonBody(dataQueryString);
            RestResponse response = (RestResponse)client.Execute(request);

            Logger.GetLogger().LogApi(LOGAPITYPE.Bitget, "", "", baseURL + balanceURL);
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

        public ActiveOrdersModel GetActiveOrders(string APIKey, object dataQueryString, string signature, string passphrase, string timestamp)
        {
            ActiveOrdersModel ret = new ActiveOrdersModel();
            string URL = baseURL + activeOrderURL;
            RestClient client = new RestClient(URL)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.POST);
            _ = request.AddHeader("ACCESS-KEY", APIKey);
            _ = request.AddHeader("ACCESS-SIGN", signature);
            _ = request.AddHeader("ACCESS-PASSPHRASE", passphrase);
            _ = request.AddHeader("ACCESS-TIMESTAMP", timestamp);
            _ = request.AddHeader("locale", "en-US");
            _ = request.AddHeader("Content-Type", "application/json");
             request.AddJsonBody(dataQueryString);
            RestResponse response = (RestResponse)client.Execute(request);

            Logger.GetLogger().LogApi(LOGAPITYPE.Bitget, "", "", baseURL + activeOrderURL);
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
                        ret = ser.Deserialize<ActiveOrdersModel>(response.Content);
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

        public OrderBookModel GetOrderBook(string dataQueryString)
        {
            OrderBookModel ret = new OrderBookModel();
            string URL = baseURL + orderBookURL + $"{dataQueryString}";
            RestClient client = new RestClient(URL)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            _ = request.AddHeader("locale", "en-US");
            _ = request.AddHeader("Content-Type", "application/json");
            //request.AddJsonBody(dataQueryString);
            RestResponse response = (RestResponse)client.Execute(request);

            Logger.GetLogger().LogApi(LOGAPITYPE.Bitget, "", "", baseURL + orderBookURL + $"{dataQueryString}");
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
        public PairListModel GetPair(string symbol)
        {
            PairListModel ret = new PairListModel();
            string URL = baseURL + getPairURL + $"{symbol}";
            RestClient client = new RestClient(URL)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            _ = request.AddHeader("locale", "en-US");
            _ = request.AddHeader("Content-Type", "application/json");
            //request.AddJsonBody(dataQueryString);
            RestResponse response = (RestResponse)client.Execute(request);

            Logger.GetLogger().LogApi(LOGAPITYPE.Bitget, "", "", baseURL + getPairURL + $"{symbol}");
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
                        ret = ser.Deserialize<PairListModel>(response.Content);
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