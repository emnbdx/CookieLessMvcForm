using System.Web.Optimization;

namespace CookieLessMvcForm
{
    public class BundleConfig
    {
        // Pour plus d'informations sur le regroupement, visitez http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/site.css"));

            bundles.Add(new ScriptBundle("~/Script/js").Include(
                "~/Scripts/jquery-1.9.1.js",
                "~/Scripts/bootstrap.js"));
        }
    }
}
