using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ULA.Quijano.ProcesoLegal.UltimusIntegrationLayer;
using System.Web.Configuration;
using ULA.Quijano.ProcesoLegal.ControllersWebApi;
using ULA.Quijano.Model;
using ULA.Quijano.ProcesoLegal.Commons;

namespace ULA.Quijano.ProcesoLegal.Controllers
{
    public class AdministracionCorreosController : BaseController
    {
        // GET: AdministracionCorreos
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult AdministracionCorreos()
        {
            InitControllers();

            Crypt crypt = new Crypt();
            string attachmentFolder = string.IsNullOrEmpty(Incident) || Incident == "0" ? "_tmp_" + UserID.Replace("/", "_") : Incident;
            ViewBag.AttachmentParam = Server.UrlEncode(crypt.EncryptString("process=" + Process + "&incident=" + attachmentFolder + "&upload=1&delete=1&title=0&style=1&etapa=" + ViewBag.Step));

            if (ViewBag.Step == "RespondeCorreo")
            {
                ViewBag.AttachmentParamRO = Server.UrlEncode(crypt.EncryptString("process=" + Process + "&incident=" + attachmentFolder + "&upload=0&delete=0&title=0&style=1&etapa=GestionaCorreo"));
            }
            else
            {
                ViewBag.AttachmentParamRO = "";
            }

            //ViewBag.Step = "RespondeCorreo";
            //Aqui verificar si es abogado, para mostrar el formulario correspondiente
            UltimusIntegrationClient ult = new UltimusIntegrationClient();
            bool esMiembro; 
            string error = String.Empty;
            ult.IsMemberOfGroup(ViewBag.UserID, WebConfigurationManager.AppSettings["GrupoUltimusAsistente"].ToString(), out esMiembro, out error);
            ViewBag.Abogado = esMiembro;
            ViewBag.FirmaRespuestaCorreo = WebConfigurationManager.AppSettings["FirmaRespuestaCorreo"].ToString().Replace(Environment.NewLine, "");
                     
            return View();
        }        
    }
}