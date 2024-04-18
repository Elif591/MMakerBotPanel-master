namespace MMakerBotPanel.WebServices.Kraken
{
    using MMakerBotPanel.Business;
    using MMakerBotPanel.Models;
    using MMakerBotPanel.WebServices.Kraken.Interface;
    using MMakerBotPanel.WebServices.Kraken.Model;
    using Newtonsoft.Json.Linq;
    using RestSharp;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Script.Serialization;

    public class KrakenWebServices : KrakenWebServicesAbstract
    {
        public KrakenSymbolModel getParity()
        {

            KrakenSymbolModel ret = new KrakenSymbolModel();

            RestClient client = new RestClient(BaseURL + SymbolsURL)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            _ = request.AddHeader("Content-type", "application/json");

            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Kraken, "", "", BaseURL + SymbolsURL);

            if (response != null)
            {

                if (response.ResponseStatus == ResponseStatus.Completed)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        if (response.ResponseUri.ToString() != (BaseURL + SymbolsURL))
                        {
                            ret.genericResult.Message = "Bilimp Adresi geçersiz";
                            return ret;
                        }

                        JavaScriptSerializer ser = new JavaScriptSerializer
                        {
                            MaxJsonLength = int.MaxValue
                        };
                        //ret.CoinbaseSymbolList = ser.Deserialize<List<CoinbaseSymbol>>(response.Content);
                        JObject getResult = JObject.Parse(response.Content);
                        JToken item1 = getResult.Children().ToList()[1].Children().ToList()[0];

                        foreach (JToken item in item1)
                        {
                            string a = item.Children().ToList()[0].ToString();
                            KrakenSymbol krakenSymbol = ser.Deserialize<KrakenSymbol>(a);
                            ret.krakenSymbols.Add(krakenSymbol);
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
        public StatusModel status()
        {
            StatusModel ret = new StatusModel();

            RestClient client = new RestClient(BaseURL + statusURL)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            _ = request.AddHeader("Content-type", "application/json");

            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Kraken, "", "", BaseURL + statusURL);

            if (response != null)
            {
                if (response.ResponseStatus == ResponseStatus.Completed)
                {

                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        if (response.ResponseUri.ToString() != (BaseURL + statusURL))
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
        public KrakenCandleData getStockData(string symbol, string interval)
        {
            KrakenCandleData ret = new KrakenCandleData();
            int unixTimestamp = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

            unixTimestamp -= 10368000;
            RestClient client = new RestClient(BaseURL + StockDataURL + "?pair=" + symbol + "&interval=" + interval + "&since=" + unixTimestamp)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            _ = request.AddHeader("Content-type", "application/json");

            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Kraken, "", "", BaseURL + StockDataURL + "?pair=" + symbol + "&interval=" + interval + "&since=" + unixTimestamp);

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
                        string data = response.Content.Replace(symbol, "CandleData");
                        ret = ser.Deserialize<KrakenCandleData>(data);
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
        public KrakenCandleData getStockDataDate(string symbol, string interval)
        {
            KrakenCandleData ret = new KrakenCandleData();
            string unixTimestamp = Convert.ToString((int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
            RestClient client = new RestClient(BaseURL + StockDataURL + "?pair=" + symbol + "&interval=" + interval + "&since=" + unixTimestamp)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            _ = request.AddHeader("Content-type", "application/json");

            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Kraken, "", "", BaseURL + StockDataURL + "?pair=" + symbol + "&interval=" + interval + "&since=" + unixTimestamp);

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
                        string data = response.Content.Replace(symbol, "CandleData");
                        ret = ser.Deserialize<KrakenCandleData>(data);
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
        public KrakenExchange CreateOrder(string signature, string apiKey, string nonce, Dictionary<string, string> parameters)
        {
            KrakenExchange ret = new KrakenExchange();

            RestClient client = new RestClient(BaseURL + orderURL)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("API-Key", apiKey);
            request.AddHeader("API-Sign", signature);
            request.AddParameter("application/x-www-form-urlencoded", string.Join("&", parameters), ParameterType.RequestBody);

            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Kraken, "", "", BaseURL + orderURL);
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
                        ret = ser.Deserialize<KrakenExchange>(response.Content);

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
                    ret.genericResult.Message = "Hata";
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
        public BalanceModel GetBalance(string signature, string apiKey, string nonce, Dictionary<string, string> parameters)
        {
            BalanceModel ret = new BalanceModel();

            RestClient client = new RestClient(BaseURL + balanceURL)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("API-Key", apiKey);
            request.AddHeader("API-Sign", signature);
            request.AddParameter("application/x-www-form-urlencoded", string.Join("&", parameters), ParameterType.RequestBody);

            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Kraken, "", "", BaseURL + orderURL);
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
                        ret.result = ser.Deserialize<string>(response.Content);
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
                    ret.genericResult.Message = "Hata";
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
        public ActiveOrdersModel GetActiveOrders(string signature, string apiKey, string nonce, Dictionary<string, string> parameters)
        {
            ActiveOrdersModel ret = new ActiveOrdersModel();

            RestClient client = new RestClient(BaseURL + activeOrderURL)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("API-Key", apiKey);
            request.AddHeader("API-Sign", signature);
            request.AddParameter("application/x-www-form-urlencoded", string.Join("&", parameters), ParameterType.RequestBody);

            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Kraken, "", "", BaseURL + activeOrderURL);
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
                        ret.result = ser.Deserialize<string>(response.Content);
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
                    ret.genericResult.Message = "Hata";
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

        public OrderBookModel GetOrderBook(string symbol)
        {
            OrderBookModel ret = new OrderBookModel();

            RestClient client = new RestClient(BaseURL + orderBookURL + $"?pair={symbol}" )
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.GET);
            _ = request.AddHeader("Content-type", "application/json");

            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Kraken, "", "", BaseURL + orderBookURL + $"?pair={symbol}");

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

        public CancelOrderModel CancelOrders(string apiKey , string signature, Dictionary<string, string> parameters)
        {
            CancelOrderModel ret = new CancelOrderModel();

            RestClient client = new RestClient(BaseURL + cancelOrderURL)
            {
                Timeout = RequestTimeout
            };
            RestRequest request = new RestRequest(Method.POST);
            request.AddHeader("API-Key", apiKey);
            request.AddHeader("API-Sign", signature);
            request.AddParameter("application/x-www-form-urlencoded", string.Join("&", parameters), ParameterType.RequestBody);
            RestResponse response = (RestResponse)client.Execute(request);
            Logger.GetLogger().LogApi(LOGAPITYPE.Kraken, "", "", BaseURL + cancelOrderURL);

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
    }
}