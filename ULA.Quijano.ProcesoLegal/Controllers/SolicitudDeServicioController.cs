using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ULA.Quijano.ProcesoLegal.Commons;
using System.Configuration;
using System.Web.Configuration;

namespace ULA.Quijano.ProcesoLegal.Controllers
{
    public class SolicitudDeServicioController : BaseController
    {
        // GET: SolicitudDeServicio
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult SolicitudDeServicio()
        {
            InitControllers();
          //  ViewBag.Menu = null;
            ViewBag.CurrentTab = "Datos del Cliente";
            CargarAttachmentParam();
            return View();
        }

        public ActionResult DatosDeSolicitud()
        {
            InitControllers();
           // ViewBag.Menu = null;
            ViewBag.CurrentTab = "Datos de Solicitud";
            CargarAttachmentParam();
            return View();
        }

        public ActionResult SolicitudDeMesaDeOperacion()
        {
            InitControllers();

            if (ViewBag.Process == "Mensajeria" && ViewBag.Step == "ResuelveSolicitud")
            {
                return Redirect(GenerarEnlaceMensajeria(ViewBag.Process, ViewBag.Incident));
            }

            ViewBag.CurrentTab = "Solicitud Mesa de Operacion";
            CargarResolucionAttachmentParam();
            CargarMensajeriaAttachmentParam();
            return View();
        }

        public ActionResult InstruccionesOtrosDepartamentos()
        {
            InitControllers();
            //ViewBag.Menu = null;
            ViewBag.CurrentTab = "Instrucciones a Otros Departamentos";
            return View();
        }

        public ActionResult PerfilDeCliente()
        {            
            return View();
        }

        public ActionResult EvaluacionDeRiesgo()
        {
            return View();
        }

        public ActionResult Error()
        {

            InitControllers();
            return View();
        }

        private void CargarAttachmentParam()
        {
            Crypt crypt = new Crypt();
            string attachmentFolder = (string.IsNullOrEmpty(Incident) || Incident == "0" ? "_tmp_" + UserID.Replace("/", "_") : Incident) + (" (" + ViewBag.CurrentTab + ")");
            ViewBag.AttachmentParam = Server.UrlEncode(crypt.EncryptString("process=" + Process + "&incident=" + attachmentFolder + "&upload=1&delete=1&title=0&style=1&etapa=" + ViewBag.Step));
        }

        private void CargarResolucionAttachmentParam()
        {
            Crypt crypt = new Crypt();
            ViewBag.AttachmentParam = Server.UrlEncode(crypt.EncryptString("process=" + Process + "&incident=" + Incident + " (Resolucion)&upload=0&delete=0&title=0&style=1&etapa=" + ViewBag.Step));
        }

        private void CargarMensajeriaAttachmentParam()
        {
            Crypt crypt = new Crypt();
            string upload = (ViewBag.Step == "SolicitarServicio" || ViewBag.Step == "MesaOperaciones") ? "1" : "0";
            string attachmentFolder = (string.IsNullOrEmpty(Incident) || Incident == "0" ? "_tmp_" + UserID.Replace("/", "_") : Incident);
            ViewBag.MensajeriaAttachmentParam = Server.UrlEncode(crypt.EncryptString("process=" + Process + "&incident=" + attachmentFolder + "&upload=" + upload + "&delete=" + upload + "&title=0&style=1&etapa=" + ViewBag.Step + "&maxFiles=3&attachmentMaxSize=5&acceptOnlyImages=1"));
        }

        public ActionResult Completado()
        {
            return View();
        }

        public string GenerarEnlaceMensajeria(string process, string incident)
        {
            return WebConfigurationManager.AppSettings["UrlMensajeriaInternet"] + "?" + Server.UrlEncode(new Crypt().EncryptString("x=" + Guid.NewGuid() + "&process=" + process + "&y=" + Guid.NewGuid() + "&incident=" + incident));
        }

    }
}