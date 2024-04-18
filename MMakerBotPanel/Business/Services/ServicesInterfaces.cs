namespace MMakerBotPanel.Business.Services
{
    using MMakerBotPanel.Business.Services.Models;
    using System;

    public abstract class ServicesInterfaces : IObserver<ObserverMessage>
    {
        private IDisposable unsubscriber;

        public void OnCompleted() { }

        public void OnError(Exception error) { }

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
