using System.Web.Http;
using System.Web.Mvc;

namespace MaxxExpress.Areas.WorkPage
{
    public class WorkPageAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "WorkPage";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "WorkPage_Default",
                "Work/{action}/{apiId}",
                new { controller = "Work", action = "Index", apiId = UrlParameter.Optional });

            WorkPageConfig.Register(GlobalConfiguration.Configuration);
        }
    }
}