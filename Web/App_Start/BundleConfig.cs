using System.Web.Optimization;

namespace Web.App_Start
{
    public static class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            RegisterStyleBundles(bundles);
            RegisterJavascriptBundles(bundles);
        }

        private static void RegisterStyleBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/css")
                            .Include("~/Content/bootstrap.css")
                            .Include("~/Content/bootstrap-theme.css")
                            .Include("~/Content/carousel.css")
                            .Include("~/Content/site.css"));
        }

        private static void RegisterJavascriptBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/js")
                            .Include("~/Scripts/jquery-2.0.2.js")
                            .Include("~/Scripts/jquery-ui-1.10.3.custom/js/jquery-ui-1.10.3.custom.js")
                            .Include("~/Scripts/bootstrap.js"));
        }
    }
}