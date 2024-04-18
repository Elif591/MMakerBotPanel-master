namespace MMakerBotPanel.Business.Services
{
    using MMakerBotPanel.Business.Services.Models;
    using MMakerBotPanel.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    public class TaskSchedulerService : IObservable<ObserverMessage>
    {
        private static TaskSchedulerService _TaskSchedulerService;

        private static readonly object _lock = new object();

        private Timer _TaskScheduler;

        private readonly List<IObserver<ObserverMessage>> observers;

        private TaskSchedulerService()
        {
            observers = new List<IObserver<ObserverMessage>>();
        }

        public static TaskSchedulerService GetTaskSchedulerService()
        {
            lock (_lock)
            {
                if (_TaskSchedulerService == null)
                {
                    _TaskSchedulerService = new TaskSchedulerService();
                }
                return _TaskSchedulerService;
            }
        }

        public void SetTimer(int dueTime, int period)
        {
            _TaskScheduler?.Dispose();
            _TaskScheduler = new Timer(TaskSchedulerService_Tick, null, dueTime, period);
        }

        public void TaskSchedulerService_Tick(object sender)
        {
            CacheHelper.Add<Int64>(CACHEKEYENUMTYPE.TaskSchedulerService_LastActivity.ToString(), Convert.ToInt64((DateTime.Now - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds), DateTimeOffset.Now.AddSeconds(5));
            SendMessage(new ObserverMessage());
        }

        public IDisposable Subscribe(IObserver<ObserverMessage> observer)
        {
            if (!observers.Contains(observer))
            {
                observers.Add(observer);
            }

            return new Unsubscriber(observers, observer);
        }

        private class Unsubscriber : IDisposable
        {
            private readonly List<IObserver<ObserverMessage>> _observers;
            private readonly IObserver<ObserverMessage> _observer;

            public Unsubscriber(List<IObserver<ObserverMessage>> observers, IObserver<ObserverMessage> observer)
            {
                _observers = observers;
                _observer = observer;
            }

            public void Dispose()
            {
                if (_observer != null && _observers.Contains(_observer))
                {
                    _ = _observers.Remove(_observer);
                }
            }
        }
        ////Gözlemciler" (observers) koleksiyonunuzun bir sabit liste olması durumunda, SendMessage metodu içinde bu hatayı almanız mümkündür. Sabit listeler, koleksiyona yapılan değişiklikleri kabul etmezler. Bu nedenle,
        ////foreach döngüsü sırasında bir gözlemcinin OnNext yöntemini çağırdığınızda, hata alırsınız.
        //Bunun nedeni, await Task.Run(() => { observer.OnNext(loc); }); satırındaki Task.Run kullanımıdır.Task.Run yöntemi, belirtilen işlemin farklı bir iş parçacığı üzerinde asenkron olarak çalışmasını sağlar.Ancak, bu 
        //işlemler aynı anda birden fazla gözlemciye ait OnNext yöntemini çağırabilir ve böylece sabit listeyi değiştirir.
        //Bu hatayı çözmek için, foreach döngüsü sırasında gözlemciler listesini kopyalamak ve bu kopya üzerinde işlem yapmak iyi bir yaklaşım olabilir.Böylece orijinal koleksiyon değişmez kalır.

        public async void SendMessage(ObserverMessage loc)
        {
            var observersCopy = observers.ToList();
            foreach (IObserver<ObserverMessage> observer in observersCopy)
            {
                // Asenkron görevi başlat
                await Task.Run(() =>
                {
                    observer.OnNext(loc);
                });

            }
        }

        public void EndTransmission()
        {
            foreach (IObserver<ObserverMessage> observer in observers.ToArray())
            {
                if (observers.Contains(observer))
                {
                    observer.OnCompleted();
                }
            }

            observers.Clear();
        }

    }
}
