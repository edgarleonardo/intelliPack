using System.Web;
using System.Web.Mvc;

namespace IntelliPackWeb
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
            filters.Add(new IntelliPackWeb.Base.SecurityFilter());
        }
    }
}
