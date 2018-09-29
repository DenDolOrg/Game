using System.Web;
using System.Web.Mvc;

namespace MyGame
{
    /// <summary>
    /// Filters fonfiguration class
    /// </summary>
    public class FilterConfig
    {
        /// <summary>
        /// Filter registration method
        /// </summary>
        /// <param name="filters">Filter collection object</param>
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }
    }
}
