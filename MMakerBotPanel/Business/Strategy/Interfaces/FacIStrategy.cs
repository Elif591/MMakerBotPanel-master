namespace MMakerBotPanel.Business.Strategy.Interfaces
{
    using MMakerBotPanel.Models;

    public interface FacIStrategy
    {
        WORKERTYPE Type { get; }
        int WorkerID { get; }
    }
}
