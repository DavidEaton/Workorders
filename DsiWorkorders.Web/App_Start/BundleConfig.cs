﻿using System.Web;
using System.Web.Optimization;

namespace DsiWorkorders.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            // Scripts ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            // The Kendo JavaScript bundle
            bundles.Add(new ScriptBundle("~/bundles/kendo").Include(
                    "~/Scripts/kendo.web.min.js",
                // or kendo.all.min.js if you want to use Kendo UI Web and Kendo UI DataViz
                    "~/Scripts/kendo.aspnetmvc.min.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));


            bundles.Add(new ScriptBundle("~/bundles/bootstrapSwitch").Include(
                         "~/Scripts/bootstrap-switch.js"));

            // Stlyes ++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++++
            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/site.css",
                "~/Content/bootstrap.css",
                "~/Content/bootstrap-responsive.css",
                "~/Content/bootstrap-switch.css"));

            bundles.Add(new StyleBundle("~/Content/jqueryui").Include(
                 "~/Content/themes/base/minified/jquery-ui.min.css"));

            // The Kendo CSS bundle
            bundles.Add(new StyleBundle("~/Content/kendo").Include(
                    "~/Content/kendo.common.min.css",
                    "~/Content/kendo.default.min.css"));

            // Clear all items from the ignore list to allow minified CSS and JavaScript files in debug mode
            bundles.IgnoreList.Clear();

            // Add back the default ignore list rules sans the ones which affect minified files and debug mode
            bundles.IgnoreList.Ignore("*.intellisense.js");
            bundles.IgnoreList.Ignore("*-vsdoc.js");
            bundles.IgnoreList.Ignore("*.debug.js", OptimizationMode.WhenEnabled);
        }
    }
}