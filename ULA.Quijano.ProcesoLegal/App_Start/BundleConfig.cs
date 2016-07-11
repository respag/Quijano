using System.Web;
using System.Web.Optimization;

namespace ULA.Quijano.ProcesoLegal
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {

            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            "~/Scripts/jquery-{version}.js"));

            //FILE UPLOAD
            bundles.Add(new ScriptBundle("~/bundles/fileupload").Include(
            "~/Scripts/JQuery.FileUpload/jquery.fileupload.js"));
            
            bundles.Add(new ScriptBundle("~/bundles/iframe-transport").Include(
            "~/Scripts/JQuery.FileUpload/jquery.iframe-transport.js"));

            bundles.Add(new ScriptBundle("~/bundles/fileupload-ui").Include(
            "~/Scripts/JQuery.FileUpload/jquery.fileupload-ui.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery.ui.widget").Include(
            "~/Scripts/JQuery.FileUpload/jquery.ui.widget.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery.knob").Include(
            "~/Scripts/JQuery.FileUpload/jquery.knob.js"));

            //END FILE UPLOAD

            //PLUPLOAD
            bundles.Add(new ScriptBundle("~/bundles/jquery.ui.plupload").Include(
            "~/Scripts/jquery.ui.plupload/jquery.ui.plupload.js"));

            //

            bundles.Add(new ScriptBundle("~/bundles/Modernizr").Include(
            "~/Scripts/modernizr-2.8.3.js"));            

            //bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
            //"~/Scripts/jquery-1.11.1.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery-migrate").Include(
                        "~/Scripts/jquery-migrate-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                        "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                        "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/maskedinput").Include("~/Scripts/jquery.maskedinput-{version}.js"));
            bundles.Add(new ScriptBundle("~/bundles/moneymask").Include("~/Scripts/jquery.moneymask.js"));
            bundles.Add(new ScriptBundle("~/bundles/jquery-dataTables").Include("~/Scripts/jquery.dataTables.js").Include("~/Scripts/jquery.dataTables.fnFilterClear.js"));
            bundles.Add(new ScriptBundle("~/bundles/toastr").Include("~/Scripts/toastr.js"));
            bundles.Add(new ScriptBundle("~/bundles/moment").Include("~/Scripts/moment.js").Include("~/Scripts/moment-with-langs.js"));
            bundles.Add(new ScriptBundle("~/bundles/bonsai").Include("~/Scripts/jquery.qubit.js").Include("~/Scripts/jquery.bonsai.js"));

            bundles.Add(new ScriptBundle("~/bundles/autoNumeric").Include("~/Scripts/autoNumeric/autoNumeric-{version}.js"));

            bundles.Add(new ScriptBundle("~/Jqwidgets/scripts").Include(
                "~/jqwidgets/jqxcore.js",
                "~/jqwidgets/jqxdata.js",
                "~/jqwidgets/jqxbuttons.js",
                "~/jqwidgets/jqxscrollbar.js",
                "~/jqwidgets/jqxmenu.js",
                "~/jqwidgets/jqxcheckbox.js",
                "~/jqwidgets/jqxlistbox.js",
                "~/jqwidgets/jqxdropdownlist.js",
                "~/jqwidgets/jqxdropdownbutton.js",
                "~/jqwidgets/jqxgrid.js",
                "~/jqwidgets/jqxgrid.columnsresize.js",
                "~/jqwidgets/jqxgrid.edit.js",
                "~/jqwidgets/jqxgrid.sort.js",
                "~/jqwidgets/jqxgrid.selection.js",
                "~/jqwidgets/jqxgrid.grouping.js",
                "~/jqwidgets/jqxgrid.pager.js",
                "~/jqwidgets/jqxnotification.js",
                "~/jqwidgets/jqxvalidator.js",
                "~/jqwidgets/globalization/globalize.js"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include("~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include("~/Content/bootstrap.css"));

            bundles.Add(new StyleBundle("~/Content/dashboard").Include("~/Content/dashboard.css"));
            bundles.Add(new StyleBundle("~/Content/awesome").Include("~/Content/font-awesome.css"));
            bundles.Add(new StyleBundle("~/Content/toastr").Include("~/Content/toastr.css"));
            bundles.Add(new StyleBundle("~/Content/bootstrap-datepicker").Include("~/Content/bootstrap-datepicker.css"));
            bundles.Add(new StyleBundle("~/Content/jquery-dataTables").Include("~/Content/jquery.dataTables.css"));
            bundles.Add(new StyleBundle("~/Content/bonsai").Include("~/Content/jquery.bonsai.css"));

            bundles.Add(new StyleBundle("~/Content/bootstrap-switch").Include("~/Content/bootstrap-switch.css"));
            bundles.Add(new StyleBundle("~/Scripts/bootstrap-switch-js").Include("~/Scripts/bootstrap-switch.js"));

            bundles.Add(new StyleBundle("~/Jqwidgets/styles").Include(
                        "~/jqwidgets/styles/jqx.base.css",
                        "~/jqwidgets/styles/jqx.ui-sunny.css",
                        "~/jqwidgets/styles/jqx.energyblue.css",
                        "~/jqwidgets/styles/jqx.darkblue.css",
                        "~/jqwidgets/styles/jqx.ui-start.css",
                        "~/jqwidgets/styles/jqx.ui-le-frog.css"));

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


            bundles.Add(new StyleBundle("~/Scripts/jquery-ui").Include("~/Scripts/ jquery-ui-1.9.0.min.js"));
           

            #region Ultimus Scripts

            bundles.Add(new ScriptBundle("~/Scripts/Ultimus-Framework")
             .Include("~/Ultimus.Framework/Ultimus.Global.js",
             "~/Ultimus.Framework/Ultimus.UI.js"));

            # region SCRIPTS -- Cotizacion Prestamo
            bundles.Add(new ScriptBundle("~/Scripts/Ultimus-Cotizacion")
             .Include("~/Ultimus.Framework/Ultimus.API.CotizacionPrestamo.js"));
            # endregion

            # region SCRIPTS -- Busqueda Solicitud
            bundles.Add(new ScriptBundle("~/Scripts/Ultimus-Busqueda")
               .Include("~/Ultimus.Framework/Ultimus.API.BusquedaSolicitud.js"));
            # endregion

            # region SCRIPTS -- Solicitantes

            bundles.Add(new ScriptBundle("~/Scripts/Ultimus-Solicitantes")
            .Include("~/Ultimus.Framework/Ultimus.API.Solicitantes.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Ultimus-SolicitantesInfoLaboral")
            .Include("~/Ultimus.Framework/Ultimus.API.Solicitantes.InfoLaboral.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Ultimus-SolicitantesReferencia")
            .Include("~/Ultimus.Framework/Ultimus.API.Solicitantes.ReferenciasPersonales.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Ultimus-SolicitantesCompromisos")
            .Include("~/Ultimus.Framework/Ultimus.API.Solicitantes.Compromisos.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Ultimus-SolicitantesDatosGenerales")
            .Include("~/Ultimus.Framework/Ultimus.API.Solicitantes.DatosGenerales.js"));

            # endregion

            # region SCRIPTS -- Calculo Prestamo

            bundles.Add(new ScriptBundle("~/Scripts/Ultimus-CalculoPrestamo")
             .Include("~/Ultimus.Framework/Ultimus.API.CalculoPrestamo.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Ultimus-CalculoHipoteca")
             .Include("~/Ultimus.Framework/Ultimus.API.CalculoPrestamoHipoteca.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Ultimus-GarantiaHipoteca")
               .Include("~/Ultimus.Framework/Ultimus.API.CalculoPrestamoHipotecaGarantia.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Ultimus-CalculoPersonal")
               .Include("~/Ultimus.Framework/Ultimus.API.CalculoPrestamoPersonal.js"));


            bundles.Add(new ScriptBundle("~/Scripts/Ultimus-CalculoAuto")
            .Include("~/Ultimus.Framework/Ultimus.API.CalculoPrestamoAuto.js"));

            # endregion

            # region SCRIPTS -- Posicion Cliente

            bundles.Add(new ScriptBundle("~/Scripts/Ultimus-PosicionCliente")
       .Include("~/Ultimus.Framework/Ultimus.API.PosicionCliente.js"));

            # endregion

            # region SCRIPTS -- TDC

            bundles.Add(new ScriptBundle("~/Scripts/Ultimus-TDC").Include("~/Ultimus.Framework/Ultimus.API.PreOfertaTDC.js"));

            # endregion

            #endregion

            /*QUIJANO & ASOCIADOS*/

            # region SCRIPTS -- QUIJANO & ASOCIADOS

            bundles.Add(new ScriptBundle("~/Scripts/Ultimus-SolicitudDeMesaDeOperacion")
             .Include("~/Ultimus.Framework/Ultimus.API.SolicitudDeMesaDeOperacion.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Ultimus-PerfilDeCliente")
             .Include("~/Ultimus.Framework/Ultimus.API.PerfilDeCliente.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Ultimus-AdministrarCorreos")
             .Include("~/Ultimus.Framework/Ultimus.API.AdministrarCorreos.js"));           

            bundles.Add(new ScriptBundle("~/Scripts/Ultimus-AdministrarCorreosAbogado")
             .Include("~/Ultimus.Framework/Ultimus.API.AdministrarCorreosAbogado.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Ultimus-SolicitudDeServicio")
             .Include("~/Ultimus.Framework/Ultimus.API.SolicitudDeServicio.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Ultimus-DatosDeSolicitud")
             .Include("~/Ultimus.Framework/Ultimus.API.DatosDeSolicitud.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Ultimus-InsOtrosDepartamentos")
             .Include("~/Ultimus.Framework/Ultimus.API.InstruccionesOtrosDepartamentos.js"));

            bundles.Add(new ScriptBundle("~/Scripts/Ultimus-EvaluacionDelCliente")
             .Include("~/Ultimus.Framework/Ultimus.API.EvaluacionDelCliente.js"));

            System.Web.Optimization.BundleTable.EnableOptimizations = false;

            # endregion
        }
    }
}