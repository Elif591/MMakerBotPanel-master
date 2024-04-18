namespace MMakerBotPanel.Models
{
    public class TicketList
    {
        public int TicketID { get; set; }
        public string Image { get; set; }
        public string Name { get; set; }
        public string LastName { get; set; }
        public string TicketName { get; set; }
        public string Email { get; set; }
        public string LastActivity { get; set; }
        public TICKETSTATUS TicketStatus { get; set; }
    }
}