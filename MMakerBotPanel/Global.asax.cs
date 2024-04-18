namespace MMakerBotPanel
{
    using MMakerBotPanel.Business.Services;
    using System.Web.Http;
    using System.Web.Mvc;
    using System.Web.Optimization;
    using System.Web.Routing;

    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            TaskSchedulerService.GetTaskSchedulerService().SetTimer(25000, 1000);
            PaymentControlService.GetPaymentControlService().Subscribe(TaskSchedulerService.GetTaskSchedulerService());
            WorkerSubscriptionService.GetWorkerSubscriptionService().Subscribe(TaskSchedulerService.GetTaskSchedulerService());
            WorkerWorkingControlService.GetWorkerWorkingControlService().Subscribe(TaskSchedulerService.GetTaskSchedulerService());
            ProfitService.GetProfitService().Subscribe(TaskSchedulerService.GetTaskSchedulerService()); 
        }
    }
}
