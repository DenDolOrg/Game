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
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/myScripts").Include(
                    "~/Scripts/RegistrationAndLogin.js"));

            bundles.Add(new StyleBundle("~/Content/RegLog").Include(
                      "~/Content/RegistrationAndLogin.css"));

            bundles.Add(new StyleBundle("~/Content/mainStyles").Include(
                        "~/Content/SiteStyles.css"));
        }
    }
}
