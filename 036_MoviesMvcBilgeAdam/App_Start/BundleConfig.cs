﻿using System.Web;
using System.Web.Optimization;

namespace _036_MoviesMvcBilgeAdam
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
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

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"
                      //,"~/Content/font-awesome.css"
                      ));

            bundles.Add(new StyleBundle("~/Content/fontawesome").Include(
                "~/Content/font-awesome.css"
                ));

            bundles.Add(new StyleBundle("~/plugins/datepickercss").Include(
                "~/Plugins/bootstrap-datepicker/dist/css/bootstrap-datepicker.min.css"
                ));
            bundles.Add(new ScriptBundle("~/plugins/datepickerjs").Include(
                "~/Plugins/bootstrap-datepicker/dist/js/bootstrap-datepicker.min.js",
                "~/Plugins/bootstrap-datepicker/dist/locales/bootstrap-datepicker.tr.min.js"
                ));
        }
    }
}
