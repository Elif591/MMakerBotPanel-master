using MMakerBotPanel.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MMakerBotPanel.Database.Model
{
    [Table("GridStrategyLog", Schema = "Log")]
    public class GridStrategyLog
    {
        [Key]
        public int GridStrategyLogID { get; set; }
        public LOGTYPE LogType { get; set; }
        public string Data { get; set; }
        public string MethodName { get; set; }
        public Int32 TimeStamp { get; set; }

    }
}