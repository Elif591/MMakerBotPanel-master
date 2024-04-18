using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using MMakerBotPanel.Models;

namespace MMakerBotPanel.Database.Model
{
    [Table("ErrorLog", Schema = "Log")]
    public class ErrorLog
    {
        [Key]
        public int ErrorLogID { get; set; }
        public LOGTYPE LogType { get; set; }
        public string Data { get; set; }
        public Int32 TimeStamp { get; set; }
        public string MethodName { get; set; }
        public string Exception { get; set; }
    }

}
