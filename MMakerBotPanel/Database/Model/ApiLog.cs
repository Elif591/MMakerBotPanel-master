namespace MMakerBotPanel.Database.Model
{
    using MMakerBotPanel.Models;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("ApiLog", Schema = "Log")]
    public class ApiLog
    {

        [Key]
        public int ApiLogID { get; set; }
        public LOGAPITYPE LogType { get; set; }
        public string RequestData { get; set; }
        public string ResponseData { get; set; }
        public string Url { get; set; }
        public Int32 TimeStamp { get; set; }
    }
}