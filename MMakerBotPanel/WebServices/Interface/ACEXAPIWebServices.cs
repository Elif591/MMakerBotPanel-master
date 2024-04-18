namespace MMakerBotPanel.WebServices.Interface
{
    using RestSharp;

    public abstract class ACEXAPIWebServices
    {

        private protected int RequestTimeout = 120000;

        public string RequestError(RestResponse response, string metotName)
        {
            string message;

            //if (response.ResponseStatus == ResponseStatus.Completed)
            //{
            //    if (response.StatusCode != System.Net.HttpStatusCode.Unauthorized)
            //    {
            //        _Settings.Cevrimici = false;
            //    }
            //    else
            //    {
            //        if (Settings.GetSettings().Login)
            //        {
            //            Settings.GetSettings().SetSettings(Settings.GetSettings().BilimpAdresi, "", "", "A92xMLD0TzWUeOKFI9u7TA==", "", "", false);
            //            NotifyManager.GetNotifyManager().ShowMessageBox(CMESSAGETYPE.ERROR, Resources["kullanici_adi_veya_sifreyi_hatali"].ToString());
            //        }
            //    }
            //}
            //else if (response.ResponseStatus == ResponseStatus.TimedOut)
            //{
            //    log.WriteLog(LOGTYPE.ERROR, metotName + " -> ResponseStatus: TimedOut -> URL: " + _ApiURL);
            //}
            //else
            //{
            //    log.WriteLog(LOGTYPE.ERROR, metotName + " -> ResponseStatus: " + response.ResponseStatus + " -> Hata Mesajı: " + response.ErrorMessage + " -> URL: " + _ApiURL);
            //    _Settings.Cevrimici = false;
            //}

            message = "Hata";
            return message;
        }
    }
}