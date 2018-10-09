using System.Web;
using System.Web.Optimization;

namespace MyGame
{
    /// <summary>
    /// Bundle configuration class
    /// </summary>
    public class BundleConfig
    {
        /// <summary>
        /// Registers all bundles
        /// </summary>
        /// <param name="bundles">Bundle object</param>
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/bibles").Include(
                        "~/Scripts/Bibles/jquery-{version}.js",
                        "~/Scripts/Bibles/bootstrap.js",
                         "~/Scripts/Bibles/angular.min.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/Bibles/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/myScripts").Include(
                    "~/Scripts/deleteTable.js"));

            bundles.Add(new StyleBundle("~/bundles/RegLog").Include(
                      "~/Content/RegistrationAndLogin.css"));

            bundles.Add(new StyleBundle("~/bundles/bootstrap").Include(
                    "~/Content/Bootstrap/bootstrap-grid.min.css",
                    "~/Content/Bootstrap/bootstrap-reboot.min.css",
                    "~/Content/Bootstrap/bootstrap.min.css"
                ));

            bundles.Add(new StyleBundle("~/bundles/mainStyles").Include(
                        "~/Content/SiteStyles.css",
                        "~/Content/Checkers.css"
                        ));
        }
    }
}
