using System.Web.Http;
using System.Web.Mvc;

namespace MaxxExpress.Areas.PortalPage
{
    public class PortalPageAreaRegitration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "PortalPage";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "PortalPage_Default",
                "portal/{action}/{apiId}",
                new { controller = "portal", action = "Index", apiId = UrlParameter.Optional });

            PortalPageConfig.Register(GlobalConfiguration.Configuration);
        }
    }
}