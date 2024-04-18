using MMakerBotPanel.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MMakerBotPanel.Database.Model
{
    [Table("UserLog", Schema = "Log")]
    public class UserLog
    {  
        [Key]
        public int UserLogID { get; set; }
        public LOGTYPE LogType { get; set; }
        public string MethodName { get; set; }
        public string  BrowserInfo { get; set; }
        public string  OSInfo { get; set; }
        public string IPNumber { get; set; }
        public Int32 TimeStamp { get; set; }
        public int UserID { get; set; }
    }
}