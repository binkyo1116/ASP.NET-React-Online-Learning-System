using System.Web.Optimization;

namespace ludadmin
{
    public class BundleConfig
    {
        // For more information on Bundling, visit https://go.microsoft.com/fwlink/?LinkID=303951
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/WebFormsJs").Include(
                            "~/Scripts/WebForms/WebForms.js",
                            "~/Scripts/WebForms/WebUIValidation.js",
                            "~/Scripts/WebForms/MenuStandards.js",
                            "~/Scripts/WebForms/Focus.js",
                            "~/Scripts/WebForms/GridView.js",
                            "~/Scripts/WebForms/DetailsView.js",
                            "~/Scripts/WebForms/TreeView.js",
                            "~/Scripts/WebForms/WebParts.js"));

            // Order is very important for these files to work, they have explicit dependencies
            bundles.Add(new ScriptBundle("~/bundles/MsAjaxJs").Include(
                    "~/Scripts/WebForms/MsAjax/MicrosoftAjax.js",
                    "~/Scripts/WebForms/MsAjax/MicrosoftAjaxApplicationServices.js",
                    "~/Scripts/WebForms/MsAjax/MicrosoftAjaxTimer.js",
                    "~/Scripts/WebForms/MsAjax/MicrosoftAjaxWebForms.js"));

            // Use the Development version of Modernizr to develop with and learn from. Then, when you’re
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                            "~/Scripts/modernizr-*"));
            bundles.Add(new ScriptBundle("~/bundles/js").Include(
               "~/Scripts/jquery-{version}.js",
              "~/Scripts/bootstrap.js",
              "~/Scripts/jquery-ui.mi.js"
                  //"~/Scripts/respond.js",
                  //"~/Scripts/wow.min.js"
                  ));

            bundles.Add(new ScriptBundle("~/bundles/hintManagement").Include(
                "~/PageScripts/global.js",
                "~/PageScripts/hintManagement.js"
            ));

            bundles.Add(new ScriptBundle("~/bundles/htmlHintManagement").Include(
                "~/PageScripts/global.js",
                "~/PageScripts/HTMLHintManagement.js"
            ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                         "~/Content/font-awesome.min.css",
                         // "~/Content/animate.min.css",
                         "~/Content/style.css",
                          "~/Content/custome-style.css",
                           "~/Content/jquery-ui.css"
                      ));
        }
    }
}