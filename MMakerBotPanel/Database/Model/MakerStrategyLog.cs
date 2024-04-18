namespace MMakerBotPanel.Database.Model
{
    using MMakerBotPanel.Models;
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("MakerStrategyLog", Schema = "Log")]
    public class MakerStrategyLog
    {
        [Key]
        public int MakerStrategyLogID { get; set; }
        public LOGTYPE LogType { get; set; }
        public string Data { get; set; }
        public string MethodName { get; set; }
        public Int32 TimeStamp { get; set; }
    }
}