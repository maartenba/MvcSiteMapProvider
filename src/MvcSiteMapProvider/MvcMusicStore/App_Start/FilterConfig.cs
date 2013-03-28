using System.Web.Mvc;

namespace MvcMusicStore.App_Start
{
    public class FilterConfig
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute()); 
        }
    }
}