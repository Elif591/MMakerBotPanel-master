namespace MMakerBotPanel.Business.Strategy.Interfaces
{
    using MMakerBotPanel.Business.Services.Models;
    using MMakerBotPanel.Models;
    using System;

    public abstract class FacStrategyObserver : IObserver<ObserverMessage> , FacIStrategy
    {
        private protected WORKERTYPE type;
        private protected int workerID;
        private protected double deposit;
        public WORKERTYPE Type
        {
            get
            {
                return type;
            }
        }

        public int WorkerID
        {
            get
            {
                return workerID;
            }
        }

        private IDisposable unsubscriber;

        public void OnCompleted() { }

        public void OnError(Exception error)
        {

        }

        public abstract void OnNext(ObserverMessage value);

        public abstract void Tick();

        public virtual void Subscribe(IObservable<ObserverMessage> provider)
        {
            if (provider != null)
            {
                unsubscriber = provider.Subscribe(this);
            }
        }

        public virtual void Unsubscribe()
        {
            unsubscriber?.Dispose();

        }

    }
}