using System.Web;
using System.Web.Optimization;

namespace AttachmentPage
{
    public class BundleConfig
    {
        // Para obtener más información acerca de Bundling, consulte http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            "~/Scripts/jquery-2.1.0.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/Scripts/jquery.min").Include(
            "~/Scripts/jquery.min.js"));

            //FILE UPLOAD
            bundles.Add(new ScriptBundle("~/Scripts/JQuery.FileUpload/jquery.fileupload").Include(
            "~/Scripts/JQuery.FileUpload/jquery.fileupload.js"));

            bundles.Add(new ScriptBundle("~/Scripts/JQuery.FileUpload/jquery.iframe-transport").Include(
            "~/Scripts/JQuery.FileUpload/jquery.iframe-transport.js"));

            bundles.Add(new ScriptBundle("~/Scripts/JQuery.FileUpload/jquery.fileupload-ui").Include(
            "~/Scripts/JQuery.FileUpload/jquery.fileupload-ui.js"));

            bundles.Add(new ScriptBundle("~/Scripts/JQuery.FileUpload/jquery.fileupload-process").Include(
            "~/Scripts/JQuery.FileUpload/jquery.fileupload-process.js"));

            bundles.Add(new ScriptBundle("~/Scripts/JQuery.FileUpload/jquery.ui.widget").Include(
            "~/Scripts/JQuery.FileUpload/jquery.ui.widget.js"));

            bundles.Add(new ScriptBundle("~/Scripts/JQuery.FileUpload/jquery.fileupload-image").Include(
            "~/Scripts/JQuery.FileUpload/jquery.fileupload-image.js"));

            bundles.Add(new ScriptBundle("~/Scripts/JQuery.FileUpload/jquery.fileupload-audio").Include(
            "~/Scripts/JQuery.FileUpload/jquery.fileupload-audio.js"));

            bundles.Add(new ScriptBundle("~/Scripts/JQuery.FileUpload/jquery.fileupload-video").Include(
            "~/Scripts/JQuery.FileUpload/jquery.fileupload-video.js"));

            bundles.Add(new ScriptBundle("~/Scripts/JQuery.FileUpload/jquery.fileupload-validate").Include(
            "~/Scripts/JQuery.FileUpload/jquery.fileupload-validate.js"));


            bundles.Add(new ScriptBundle("~/Scripts/jquery.knob").Include(
            "~/Scripts/jquery.knob.js"));

            //END FILE UPLOAD

            //LOAD IMAGE
            bundles.Add(new ScriptBundle("~/Scripts/LoadImage/load-image").Include(
                "~/Scripts/LoadImage/load-image.js"));

            bundles.Add(new ScriptBundle("~/Scripts/LoadImage/load-image-ios").Include(
                "~/Scripts/LoadImage/load-image-ios.js"));

            bundles.Add(new ScriptBundle("~/Scripts/LoadImage/load-image-orientation").Include(
                "~/Scripts/LoadImage/load-image-orientation.js"));

            bundles.Add(new ScriptBundle("~/Scripts/LoadImage/load-image-meta").Include(
                "~/Scripts/LoadImage/load-image-meta.js"));

            bundles.Add(new ScriptBundle("~/Scripts/LoadImage/load-image-exif").Include(
                "~/Scripts/LoadImage/load-image-exif.js"));

            bundles.Add(new ScriptBundle("~/Scripts/LoadImage/load-image-exif-map").Include(
                "~/Scripts/LoadImage/load-image-exif-map.js"));



            //PLUPLOAD
            bundles.Add(new ScriptBundle("~/Scripts/jquery.ui.plupload/jquery.plupload").Include(
                "~/Scripts/jquery.ui.plupload/jquery.plupload.js"));

            bundles.Add(new ScriptBundle("~/Scripts/canvas-to-blob.min").Include(
                "~/Scripts/canvas-to-blob.min.js"));

            bundles.Add(new ScriptBundle("~/Scripts/jquery.blueimp-gallery.min").Include(
                "~/Scripts/jquery.blueimp-gallery.min.js"));

            bundles.Add(new ScriptBundle("~/Scripts/tmpl.min").Include(
                "~/Scripts/tmpl.min.js"));

            bundles.Add(new ScriptBundle("~/Scripts/load-image.all.min").Include(
                "~/Scripts/load-image.all.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/ajaxlogin").Include(
                "~/Scripts/app/ajaxlogin.js"));

            bundles.Add(new ScriptBundle("~/Scripts/bootstrap.min").Include(
            "~/Scripts/bootstrap.min.js"));

            bundles.Add(new ScriptBundle("~/Scripts/bootstrap-filestyle.min").Include(
            "~/Scripts/bootstrap-filestyle.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/ajaxlogin").Include(
                "~/Scripts/app/ajaxlogin.js"));

            // Utilice la versión de desarrollo de Modernizr para desarrollar y obtener información sobre los formularios. De este modo, estará
            // preparado para la producción y podrá utilizar la herramienta de creación disponible en http://modernizr.com para seleccionar solo las pruebas que necesite.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/Site.css",
                "~/Content/TodoList.css"));

            bundles.Add(new StyleBundle("~/Content/bootstrap/css/bootstrap.min").Include("~/Content/bootstrap/css/bootstrap.min.css"));
            bundles.Add(new StyleBundle("~/Content/fileupload/css/jquery.fileupload").Include("~/Content/fileupload/css/jquery.fileuploa.css"));
            bundles.Add(new StyleBundle("~/Content/fileupload/css/jquery.fileupload-ui").Include("~/Content/fileupload/css/jquery.fileupload-ui.css"));
            bundles.Add(new StyleBundle("~/Content/blueimp-gallery.min").Include("~/Content/blueimp-gallery.min.css"));
            

            bundles.Add(new StyleBundle("~/Content/themes/base/css").Include(
                        "~/Content/themes/base/jquery.ui.core.css",
                        "~/Content/themes/base/jquery.ui.resizable.css",
                        "~/Content/themes/base/jquery.ui.selectable.css",
                        "~/Content/themes/base/jquery.ui.accordion.css",
                        "~/Content/themes/base/jquery.ui.autocomplete.css",
                        "~/Content/themes/base/jquery.ui.button.css",
                        "~/Content/themes/base/jquery.ui.dialog.css",
                        "~/Content/themes/base/jquery.ui.slider.css",
                        "~/Content/themes/base/jquery.ui.tabs.css",
                        "~/Content/themes/base/jquery.ui.datepicker.css",
                        "~/Content/themes/base/jquery.ui.progressbar.css",
                        "~/Content/themes/base/jquery.ui.theme.css"));

            bundles.Add(new ScriptBundle("~/Scripts/Attachment")
             .Include("~/Scripts/Attachment.js"));

            System.Web.Optimization.BundleTable.EnableOptimizations = false;
        }
    }
}