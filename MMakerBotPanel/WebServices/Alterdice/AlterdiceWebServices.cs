namespace MMakerBotPanel.WebServices.Alterdice
{
    using MMakerBotPanel.Business;
    using MMakerBotPanel.Models;
    using MMakerBotPanel.WebServices.Alterdice.Interface;
    using MMakerBotPanel.WebServices.Alterdice.Model;
    using RestSharp;
    using System;
    using System.Collections.Generic;
    using System.Web.Script.Serialization;
    using Newtonsoft.Json;
    public class AlterdiceWebServices : AlterdiceWebServicesAbstract
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
            Logger.GetLogger().LogApi(LOGAPITYPE.Alterdice, "", "", baseURL + symbolURL);
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
                        //Logger.GetLogger().LogApi(LOGAPITYPE.Alterdice, JsonConvert.SerializeObject(request), JsonConvert.SerializeObject(ret), baseURL + symbolURL);
                        return ret;
                    }
                }
                else
                {
                    ret.genericResult.IsOK = false;
                    ret.genericResult.Message = RequestError(response, "CheckServer");
                    //Logger.GetLogger().LogApi(LOGAPITYPE.Alterdice, JsonConvert.SerializeObject(request), JsonConvert.SerializeObject(ret), baseURL + symbolURL);
                    return ret;
                }
            }
            else
            {
                ret.genericResult.IsOK = false;
                ret.genericResult.Message = "Error";
                //Logger.GetLogger().LogApi(LOGAPITYPE.Alterdice, JsonConvert.SerializeObject(request), JsonConvert.SerializeObject(ret), baseURL + symbolURL);
                return ret;
            }

        }
        public StatusModel Status()
        {
            StatusModel ret = new StatusModel();

            RestClient client = new RestClient(baseURL + statusURL + "?pair=BTCUSDT")
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            _ = request.AddHeader("Content-type", "application/json");

            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Alterdice, "", "", baseURL + statusURL + "?pair=BTCUSDT");
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
                        //Logger.GetLogger().LogApi(LOGAPITYPE.Alterdice, JsonConvert.SerializeObject(request), JsonConvert.SerializeObject(ret), baseURL + symbolURL);
                        return ret;
                    }
                }
                else
                {
                    ret.genericResult.IsOK = false;
                    ret.genericResult.Message = RequestError(response, "CheckServer");
                    //Logger.GetLogger().LogApi(LOGAPITYPE.Alterdice, JsonConvert.SerializeObject(request), JsonConvert.SerializeObject(ret), baseURL + symbolURL);
                    return ret;
                }

            }
            else
            {
                ret.genericResult.IsOK = false;
                ret.genericResult.Message = "Error";
                //Logger.GetLogger().LogApi(LOGAPITYPE.Alterdice, JsonConvert.SerializeObject(request), JsonConvert.SerializeObject(ret), baseURL + symbolURL);
                return ret;
            }
        }
        public StockDataModel StockData(string symbol, string interval, string limit, string start)
        {
            StockDataModel ret = new StockDataModel();
            RestClient client = new RestClient(baseSocketURL + stockDataURL + $"?t={symbol}&r={interval}&limit={limit}")
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            _ = request.AddHeader("Content-type", "application/json");
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Alterdice, "" , "", baseSocketURL + stockDataURL + $"?t={symbol}&r={interval}&limit={limit}");
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
                        ret.stockDataDetailModels = ser.Deserialize<List<StockDataDetail>>(response.Content);


                        foreach (StockDataDetail item in ret.stockDataDetailModels)
                        {
                            ChartData addData = new ChartData
                            {
                                low = Convert.ToDouble(item.low),
                                high = Convert.ToDouble(item.high),
                                volume = Convert.ToInt64(item.volume),
                                time = Convert.ToInt64(item.time),
                                open = Convert.ToDouble(item.open),
                                close = Convert.ToDouble(item.close),
                                pair_id = (long)Convert.ToDouble(item.pair_id)
                            };
                            ret.chartDataModels.Add(addData);
                        }

                        return ret;
                    }
                    else
                    {
                        ret.genericResult.IsOK = false;
                        ret.genericResult.Message = RequestError(response, "CheckServer");
                        //Logger.GetLogger().LogApi(LOGAPITYPE.Alterdice, JsonConvert.SerializeObject(request), JsonConvert.SerializeObject(ret), baseURL + symbolURL);
                        return ret;
                    }
                }
                else
                {
                    ret.genericResult.IsOK = false;
                    ret.genericResult.Message = RequestError(response, "CheckServer");
                    //Logger.GetLogger().LogApi(LOGAPITYPE.Alterdice, JsonConvert.SerializeObject(request), JsonConvert.SerializeObject(ret), baseURL + symbolURL);
                    return ret;
                }
            }
            else
            {
                ret.genericResult.IsOK = false;
                ret.genericResult.Message = "Error";
                //Logger.GetLogger().LogApi(LOGAPITYPE.Alterdice, JsonConvert.SerializeObject(request), JsonConvert.SerializeObject(ret), baseURL + symbolURL);
                return ret;
            }
        }
        public CreateOrderModel CreateOrder(string Token, string signature, object body)
        {
            string URL = baseURL + createOrderURL;
            RestClient client = new RestClient(URL)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.POST);
            _ = request.AddHeader("Content-type", "application/json");
            _ = request.AddHeader("login-token", Token);
            _ = request.AddHeader("x-auth-sign", signature);
            _ = request.AddJsonBody(body);
            RestResponse response = (RestResponse)client.Execute(request);
            CreateOrderModel ret = new CreateOrderModel();
            Logger.GetLogger().LogApi(LOGAPITYPE.Alterdice,"" , "", baseURL + createOrderURL);
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
                        //Logger.GetLogger().LogApi(LOGAPITYPE.Alterdice, JsonConvert.SerializeObject(request), JsonConvert.SerializeObject(ret), baseURL + symbolURL);
                        return ret;
                    }
                }
                else
                {
                    ret.genericResult.IsOK = false;
                    ret.genericResult.Message = RequestError(response, "CheckServer");
                    //Logger.GetLogger().LogApi(LOGAPITYPE.Alterdice, JsonConvert.SerializeObject(request), JsonConvert.SerializeObject(ret), baseURL + symbolURL);
                    return ret;
                }
            }
            else
            {
                ret.genericResult.IsOK = false;
                ret.genericResult.Message = "Error";
                //Logger.GetLogger().LogApi(LOGAPITYPE.Alterdice, JsonConvert.SerializeObject(request), JsonConvert.SerializeObject(ret), baseURL + symbolURL);
                return ret;
            }
        }
        public BalanceModel Balance(string Token, string signature, object body)
        {
            string URL = baseURL + balanceURL;
            RestClient client = new RestClient(URL)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.POST);
            _ = request.AddHeader("Content-type", "application/json");
            _ = request.AddHeader("login-token", Token);
            _ = request.AddHeader("x-auth-sign", signature);
            _ = request.AddJsonBody(body);
            RestResponse response = (RestResponse)client.Execute(request);
            BalanceModel ret = new BalanceModel();
            Logger.GetLogger().LogApi(LOGAPITYPE.Alterdice, "" , "", baseURL + balanceURL);
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
                        //Logger.GetLogger().LogApi(LOGAPITYPE.Alterdice, JsonConvert.SerializeObject(request), JsonConvert.SerializeObject(ret), baseURL + symbolURL);
                        return ret;
                    }
                }
                else
                {
                    ret.genericResult.IsOK = false;
                    ret.genericResult.Message = RequestError(response, "CheckServer");
                    //Logger.GetLogger().LogApi(LOGAPITYPE.Alterdice, JsonConvert.SerializeObject(request), JsonConvert.SerializeObject(ret), baseURL + symbolURL);
                    return ret;
                }
            }
            else
            {
                ret.genericResult.IsOK = false;
                ret.genericResult.Message = "Error";
                //Logger.GetLogger().LogApi(LOGAPITYPE.Alterdice, JsonConvert.SerializeObject(request), JsonConvert.SerializeObject(ret), baseURL + symbolURL);
                return ret;
            }
        }
        public ActiveOrderModel ActiveOrder(string Token, string signature, object body)
        {
            string URL = baseURL + activeOrderURL;
            RestClient client = new RestClient(URL)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.POST);
            _ = request.AddHeader("Content-type", "application/json");
            _ = request.AddHeader("login-token", Token);
            _ = request.AddHeader("x-auth-sign", signature);
            _ = request.AddJsonBody(body);
            RestResponse response = (RestResponse)client.Execute(request);
            ActiveOrderModel ret = new ActiveOrderModel();
            Logger.GetLogger().LogApi(LOGAPITYPE.Alterdice, "" , "", baseURL + activeOrderURL);
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
                        //Logger.GetLogger().LogApi(LOGAPITYPE.Alterdice, JsonConvert.SerializeObject(request), JsonConvert.SerializeObject(ret), baseURL + symbolURL);
                        return ret;
                    }
                }
                else
                {
                    ret.genericResult.IsOK = false;
                    ret.genericResult.Message = RequestError(response, "CheckServer");
                    //Logger.GetLogger().LogApi(LOGAPITYPE.Alterdice, JsonConvert.SerializeObject(request), JsonConvert.SerializeObject(ret), baseURL + symbolURL);
                    return ret;
                }
            }
            else
            {
                ret.genericResult.IsOK = false;
                ret.genericResult.Message = "Error";
                //Logger.GetLogger().LogApi(LOGAPITYPE.Alterdice, JsonConvert.SerializeObject(request), JsonConvert.SerializeObject(ret), baseURL + symbolURL);
                return ret;
            }
        }
        public OrderBookModel OrderBook(string pair)
        {
            OrderBookModel ret = new OrderBookModel();
            RestClient client = new RestClient(baseURL + orderBookURL + $"?pair={pair}")
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            _ = request.AddHeader("Content-type", "application/json");
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Alterdice, "" , "", baseURL + orderBookURL + $"?pair={pair}");
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
                        //Logger.GetLogger().LogApi(LOGAPITYPE.Alterdice, JsonConvert.SerializeObject(request), JsonConvert.SerializeObject(ret), baseURL + symbolURL);
                        return ret;
                    }
                }
                else
                {
                    ret.genericResult.IsOK = false;
                    ret.genericResult.Message = RequestError(response, "CheckServer");
                    //Logger.GetLogger().LogApi(LOGAPITYPE.Alterdice, JsonConvert.SerializeObject(request), JsonConvert.SerializeObject(ret), baseURL + symbolURL);
                    return ret;
                }

            }
            else
            {
                ret.genericResult.IsOK = false;
                ret.genericResult.Message = "Error";
                //Logger.GetLogger().LogApi(LOGAPITYPE.Alterdice, JsonConvert.SerializeObject(request), JsonConvert.SerializeObject(ret), baseURL + symbolURL);
                return ret;
            }
        }
        public TradeModel Trade(string symbol)
        {
            TradeModel ret = new TradeModel();
            RestClient client = new RestClient(baseURL + tradeURL + $"?pair={symbol}")
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            _ = request.AddHeader("Content-type", "application/json");
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Alterdice, "" , "", baseURL + tradeURL + $"?pair={symbol}");
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
                        //Logger.GetLogger().LogApi(LOGAPITYPE.Alterdice, JsonConvert.SerializeObject(request), JsonConvert.SerializeObject(ret), baseURL + symbolURL);
                        return ret;
                    }
                }
                else
                {
                    ret.genericResult.IsOK = false;
                    ret.genericResult.Message = RequestError(response, "CheckServer");
                    //Logger.GetLogger().LogApi(LOGAPITYPE.Alterdice, JsonConvert.SerializeObject(request), JsonConvert.SerializeObject(ret), baseURL + symbolURL);
                    return ret;
                }

            }
            else
            {
                ret.genericResult.IsOK = false;
                ret.genericResult.Message = "Hata";
                //Logger.GetLogger().LogApi(LOGAPITYPE.Alterdice, JsonConvert.SerializeObject(request), JsonConvert.SerializeObject(ret), baseURL + symbolURL);
                return ret;
            }
        }
        public CancelOrderModel Cancelorders(string Token, string signature, object body)
        {
            string URL = baseURL + cancelOrdersURL;
            RestClient client = new RestClient(URL)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.POST);
            _ = request.AddHeader("Content-type", "application/json");
            _ = request.AddHeader("login-token", Token);
            _ = request.AddHeader("x-auth-sign", signature);
            _ = request.AddJsonBody(body);
            RestResponse response = (RestResponse)client.Execute(request);
            CancelOrderModel ret = new CancelOrderModel();
            Logger.GetLogger().LogApi(LOGAPITYPE.Alterdice, "", "", baseURL + cancelOrdersURL);
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
                        //Logger.GetLogger().LogApi(LOGAPITYPE.Alterdice, JsonConvert.SerializeObject(request), JsonConvert.SerializeObject(ret), baseURL + symbolURL);
                        return ret;
                    }
                }
                else
                {
                    ret.genericResult.IsOK = false;
                    ret.genericResult.Message = RequestError(response, "CheckServer");
                    //Logger.GetLogger().LogApi(LOGAPITYPE.Alterdice, JsonConvert.SerializeObject(request), JsonConvert.SerializeObject(ret), baseURL + symbolURL);
                    return ret;
                }
            }
            else
            {
                ret.genericResult.IsOK = false;
                ret.genericResult.Message = "Error";
                //Logger.GetLogger().LogApi(LOGAPITYPE.Alterdice, JsonConvert.SerializeObject(request), JsonConvert.SerializeObject(ret), baseURL + symbolURL);
                return ret;
            }
        }
        public QueryOrderModel Queryorder(string Token, string signature, object body)
        {
            string URL = baseURL + queryOrderURL;
            RestClient client = new RestClient(URL)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.POST);
            _ = request.AddHeader("Content-type", "application/json");
            _ = request.AddHeader("login-token", Token);
            _ = request.AddHeader("x-auth-sign", signature);
            _ = request.AddJsonBody(body);
            RestResponse response = (RestResponse)client.Execute(request);
            QueryOrderModel ret = new QueryOrderModel();
            Logger.GetLogger().LogApi(LOGAPITYPE.Alterdice, "", "", baseURL + queryOrderURL);
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
                        //Logger.GetLogger().LogApi(LOGAPITYPE.Alterdice, JsonConvert.SerializeObject(request), JsonConvert.SerializeObject(ret), baseURL + symbolURL);
                        return ret;
                    }
                }
                else
                {
                    ret.genericResult.IsOK = false;
                    ret.genericResult.Message = RequestError(response, "CheckServer");
                    //Logger.GetLogger().LogApi(LOGAPITYPE.Alterdice, JsonConvert.SerializeObject(request), JsonConvert.SerializeObject(ret), baseURL + symbolURL);
                    return ret;
                }
            }
            else
            {
                ret.genericResult.IsOK = false;
                ret.genericResult.Message = "Error";
                //Logger.GetLogger().LogApi(LOGAPITYPE.Alterdice, JsonConvert.SerializeObject(request), JsonConvert.SerializeObject(ret), baseURL + symbolURL);
                return ret;
            }
        }
    }
}