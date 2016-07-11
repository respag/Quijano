using System;
using System.Linq;
using System.Web.Mvc;
using ULA.Quijano.ProcesoLegal.Commons;
using System.Configuration;
using System.Web.Configuration;
using System.Collections.Generic;
using Ultimus.UtilityLayer;
using ULA.Quijano.ProcesoLegal.UltimusIntegrationLayer;

namespace ULA.Quijano.ProcesoLegal.Controllers
{
    public class HomeController : BaseController
    {
        LogFilecs log = new LogFilecs();
        string ApplicationName = "HomeController";

        public ActionResult Index()
        {
            Ultimus.UtilityLayer.Crypt crypt = new Ultimus.UtilityLayer.Crypt();

            ActionResult ret = null;
            string url = "";
            try
            {
                LeerTarea();
                GeneradorMenu miObj = new GeneradorMenu();
                ICollection<ULA.Quijano.ProcesoLegal.StepFormsList> Menulist = miObj.GetFormsByProcessAndStep(Tarea.Process, Tarea.Step);
                url = (from m in Menulist
                       select m.FormPath).First();

                if (Tarea != null)
                {
                    //if (Tarea.Process == "Comunicacion")
                    //{
                    //    if (Tarea.Step == "GestionaCorreo" || Tarea.Step == "RespondeCorreo")
                    //    {
                    //        url = WebConfigurationManager.AppSettings["URL.AdminCorreo"];
                    //    }
                    //}

                    //if (Tarea.Process == "Mensajeria")
                    //{
                    //    if (Tarea.Step == "SolicitarServicio" || Tarea.Step == "MesaOperaciones")
                    //    {
                    //        url = WebConfigurationManager.AppSettings["URL.SolicitudMesaOperacion"];
                    //    }
                    //}

                    string QueryString = "{0}?";

                    QueryString = string.Concat(QueryString, "UserID={1}");
                    QueryString = string.Concat(QueryString, "&TaskID={2}");
                    QueryString = string.Concat(QueryString, "&Process={3}");
                    QueryString = string.Concat(QueryString, "&Step={4}");
                    QueryString = string.Concat(QueryString, "&Incident={5}");
                    QueryString = string.Concat(QueryString, "&JobFunction={6}");
                    QueryString = string.Concat(QueryString, "&UserFullName={7}");
                    QueryString = string.Concat(QueryString, "&UserEmail={8}");
                    QueryString = string.Concat(QueryString, "&Supervisor={9}");
                    QueryString = string.Concat(QueryString, "&SupervisorFullName={10}");
                    QueryString = string.Concat(QueryString, "&TaskStatus={11}");
                    if (Tarea.Incident != 0)
                    {
                        string valor;
                        string error;
                        using (UltimusIntegrationClient client = new UltimusIntegrationClient())
                        {
                            client.GetNodeValue(Tarea.User, Tarea.TaskId, "TaskData.Global.CodigoSolicitud", out valor, out error);
                        }
                        QueryString = string.Concat(QueryString, "&sol_co_solicitud=", valor);
                    }
                    else
                    {
                        QueryString = string.Concat(QueryString, "&sol_co_solicitud=0");
                    }

                   // QueryString = string.Concat(QueryString, "&sol_co_solicitud=0");

                    //Para ver si está creando el objeto Tarea
                    log.EscribirArchivo(ApplicationName, String.Format("Propiedades de Tarea: {0},{1},{2},{3},{4},{5},{6},{7},{8},{9}",
                        Tarea.User,Tarea.TaskId, Tarea.Process, Tarea.Step,Tarea.Incident.ToString(),
                        Tarea.JobFunction, Tarea.UserEmail, Tarea.Supervisor,Tarea.SupervisorFullName,Tarea.Status.ToString()));
                    
                    ret = Redirect(string.Format(QueryString,
                        url,
                        crypt.EncryptString(Tarea.User),
                        crypt.EncryptString(Tarea.TaskId),
                        crypt.EncryptString(Tarea.Process),
                        crypt.EncryptString(Tarea.Step),
                        crypt.EncryptString(Tarea.Incident.ToString()),
                        crypt.EncryptString(Tarea.JobFunction),
                        crypt.EncryptString(Tarea.UserFullName),
                        crypt.EncryptString(Tarea.UserEmail),
                        crypt.EncryptString(Tarea.Supervisor),
                        crypt.EncryptString(Tarea.SupervisorFullName),
                        crypt.EncryptString(Tarea.Status.ToString())
                        ));
                }
                else
                    throw new Exception();
            }
            catch (Exception ex)
            {
                log.EscribirArchivo(ApplicationName, ex);
                return View("_Error");
            }
            return ret;
        }


        public ActionResult PruebaLayout()
        {
            return View();
        }
                
        public ActionResult Error()
        {

            InitControllers();
            return View();
        }

        public string Encrypt(int id)
        {
            return new Ultimus.UtilityLayer.Crypt().EncryptString(id.ToString());
        }

        public ActionResult Logo()
        {

            string logo = ConfigurationManager.AppSettings["ULA.LogoClient"].ToString();
            string baseclient = ConfigurationManager.AppSettings["ULA.LogoClient.baseclient"].ToString();

            var content = System.IO.File.ReadAllBytes(Server.MapPath("~/images/" + logo));

            return base.File(content, baseclient);
        }

    }
}
