using Ninject.Modules;
using BLL;

namespace Construct_04
{
    public class NinjectRegistrations : NinjectModule
    {
        public override void Load()
        {
            Bind<IOrderService>().To<OrderService>();
            Bind<IReportService>().To<ReportService>();
            Bind<IDbCrud>().To<DBDataOperation>();
            Bind<IRecommendationService>().To<RecommendationService>();
            Bind<IAutorizationService>().To<AutorizationService>();
            Bind<IForecastService>().To<ForecastService>();
        }
    }
}
