using System.Web;
using System.Web.Optimization;

namespace ATS.Web
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Clear();
            bundles.ResetAll();            
            BundleTable.EnableOptimizations = false;

            bundles.Add(new StyleBundle("~/Content/css").Include(
                       "~/Content/Loader.css",
                      "~/global/css/bootstrap.min.css",
                      "~/global/css/bootstrap-extend.min.css",
                      "~/assets/css/site.min.css",
                      "~/global/vendor/animsition/animsition.css",
                      "~/global/vendor/asscrollable/asScrollable.css",
                      "~/global/vendor/icheck/icheck.css",
                      "~/global/vendor/switchery/switchery.css",
                      "~/global/vendor/intro-js/introjs.css",
                      "~/global/vendor/flag-icon-css/flag-icon.css",
                      "~/global/vendor/toastr/toastr.css",
                      "~/global/vendor/jquery-wizard/jquery-wizard.css",
                      "~/global/vendor/jquery-selective/jquery-selective.css",
                      "~/global/vendor/bootstrap-markdown/bootstrap-markdown.css",
                      "~/global/vendor/bootstrap-datepicker/bootstrap-datepicker.css",
                      "~/assets/examples/css/dashboard/team.css",
                      "~/global/fonts/web-icons/web-icons.min.css",
                      "~/global/fonts/brand-icons/brand-icons.min.css"));

            bundles.Add(new StyleBundle("~/Content/core").Include(
                      "~/Content/icons/icon.css",
                      "~/Content/layout-grid.css",
                      "~/Content/icons/7-stroke/7-stroke.css",
                      "~/Content/icons/ionicons/ionicons.css",
                      "~/Content/icons/font-awesome/font-awesome.css",
                      "~/Content/icons/web-icons/web-icons.css"));

            bundles.Add(new ScriptBundle("~/ej").Include(
                        "~/Scripts/jquery-1.10.2.min.js",
                        "~/Scripts/jsrender.min.js",
                        "~/Scripts/jquery.easing.1.3.min.js",
                        "~/Scripts/jquery.globalize.min.js",
                        "~/Scripts/ej/ej.core.min.js",
                        "~/Scripts/ej/ej.data.min.js",
                        "~/Scripts/ej/ej.touch.min.js",
                        "~/Scripts/ej/ej.globalize.min.js",
                        "~/Scripts/ej/ej.grid.min.js",
                        "~/Scripts/ej/ej.pager.min.js",
                        "~/Scripts/ej/ej.waitingpopup.min.js",
                        "~/Scripts/ej/ej.toolbar.min.js",
                        "~/Scripts/ej/ej.dropdownlist.min.js",
                        "~/Scripts/ej/ej.checkbox.min.js",
                        "~/Scripts/ej/ej.scroller.min.js",
                        "~/Scripts/ej/ej.draggable.min.js",
                        "~/Scripts/ej/ej.dialog.min.js",
                        "~/Scripts/ej/ej.button.min.js",
                        "~/Scripts/ej/ej.autocomplete.min.js",
                        "~/Scripts/ej/ej.datepicker.min.js",
                        "~/Scripts/ej/ej.datetimepicker.min.js",
                        "~/Scripts/ej/ej.editor.min.js",
                        "~/Scripts/ej/ej.web.all.min.js",
                        "~/Scripts/ej/ej.unobtrusive.min.js"));


            bundles.Add(new ScriptBundle("~/scripts/js").Include(
                        //"~/global/vendor/jquery/jquery.js",
                        "~/global/vendor/bootstrap/bootstrap.js",
                        "~/global/vendor/animsition/animsition.js",
                        "~/global/vendor/aspieprogress/jquery-asPieProgress.js",
                        "~/global/vendor/asscrollable/jquery.asScrollable.all.js",
                        "~/global/vendor/mousewheel/jquery.mousewheel.js",
                        "~/global/vendor/icheck/icheck.min.js",
                        "~/global/vendor/switchery/switchery.min.js",
                        "~/global/vendor/intro-js/intro.js",
                        "~/global/vendor/toastr/toastr.js",
                        "~/global/vendor/formvalidation/formValidation.js",
                        "~/global/vendor/formvalidation/framework/bootstrap.js",
                        "~/global/vendor/matchheight/jquery.matchHeight-min.js",
                        "~/global/vendor/jquery-wizard/jquery-wizard.js",
                        "~/global/vendor/jquery-selective/jquery-selective.min.js",
                        "~/global/vendor/bootstrap-datepicker/bootstrap-datepicker.js",
                        "~/global/vendor/jt-timepicker/jquery.timepicker.min.js",
                        "~/global/vendor/datepair-js/datepair.min.js",
                        "~/global/vendor/datepair-js/jquery.datepair.min.js",
                        "~/global/js/core.js",
                        "~/assets/js/site.js",
                        "~/assets/js/sections/menu.js",
                        "~/assets/js/sections/menubar.js",
                        "~/global/js/configs/config-colors.js",
                        "~/assets/js/configs/config-tour.js",
                        "~/global/js/components/animsition.js",
                        "~/global/js/components/aspieprogress.js",
                        "~/global/js/components/asscrollable.js",
                        "~/global/js/components/icheck.js",
                        "~/global/js/components/slidepanel.js",
                        "~/global/js/components/switchery.js",
                        "~/global/js/components/jquery-wizard.js",
                        "~/global/js/components/matchheight.js",
                        "~/global/js/components/bootstrap-datepicker.js",
                        "~/global/js/components/jt-timepicker.js",
                        "~/global/js/components/datepair-js.js",
                        "~/global/js/components/toastr.js",
                        "~/global/vendor/bootstrap-markdown/bootstrap-markdown.js",
                        "~/global/vendor/marked/marked.js",
                        "~/global/vendor/to-markdown/to-markdown.js",
                        "~/assets/examples/js/forms/advanced.js",
                        "~/assets/examples/js/dashboard/team.js",
                        "~/Scripts/jquery.validate.js",
                        "~/Scripts/jquery.validate.unobtrusive.js",
                        "~/global/js/components/aspieprogress.js"));
        }
    }
}
