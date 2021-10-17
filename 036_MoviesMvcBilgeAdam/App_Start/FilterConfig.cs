using System.Web;
using System.Web.Mvc;

namespace _036_MoviesMvcBilgeAdam
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
