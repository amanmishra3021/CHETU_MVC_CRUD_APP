using System.Web;
using System.Web.Mvc;

namespace CHETU_MVC_CRUD_APP
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
