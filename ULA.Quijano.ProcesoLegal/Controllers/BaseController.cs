using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using System.Xml.Linq;
using ULA.Quijano.ProcesoLegal.Commons;

namespace ULA.Quijano.ProcesoLegal.Controllers
{
    public class BaseController : Controller
    {
        private static Logger logger = LogManager.GetLogger("BaseController");

        private string _IDLogger;

        public string IDLogger
        {
            get { return _IDLogger; }
            set { _IDLogger = string.Format("INCIDENT {0}|", value); }
        }

        public string UserID
        {
            get
            {
                string ret = string.Empty;
                if (!string.IsNullOrEmpty(Request.QueryString["UserID"]))
                    ret = new Crypt().DecryptString(Request.QueryString["UserID"]);
                return ret;
            }
        }

        public string TaskID
        {
            get
            {
                string ret = string.Empty;
                if (!string.IsNullOrEmpty(Request.QueryString["TaskID"]))
                    ret = new Crypt().DecryptString(Request.QueryString["TaskID"]);
                return ret;
            }
        }

        public string Process
        {
            get
            {
                string ret = string.Empty;
                if (!string.IsNullOrEmpty(Request.QueryString["Process"]))
                    ret = new Crypt().DecryptString(Request.QueryString["Process"]);
                return ret;
            }
        }

        public string Step
        {
            get
            {
                string ret = string.Empty;
                if (!string.IsNullOrEmpty(Request.QueryString["Step"]))
                    ret = new Crypt().DecryptString(Request.QueryString["Step"]);
                return ret;
            }
        }

        public string Incident
        {
            get
            {
                string ret = string.Empty;
                if (!string.IsNullOrEmpty(Request.QueryString["Incident"]))
                    ret = new Crypt().DecryptString(Request.QueryString["Incident"]);
                return ret;
            }
        }

        public string JobFunction
        {
            get
            {
                string ret = string.Empty;
                if (!string.IsNullOrEmpty(Request.QueryString["JobFunction"]))
                    ret = new Crypt().DecryptString(Request.QueryString["JobFunction"]);
                return ret;
            }
        }

        public string UserFullName
        {
            get
            {
                string ret = string.Empty;
                if (!string.IsNullOrEmpty(Request.QueryString["UserFullName"]))
                    ret = new Crypt().DecryptString(Request.QueryString["UserFullName"]);
                return ret;
            }
        }

        public string UserEmail
        {
            get
            {
                string ret = string.Empty;
                if (!string.IsNullOrEmpty(Request.QueryString["UserEmail"]))
                    ret = new Crypt().DecryptString(Request.QueryString["UserEmail"]);
                return ret;
            }
        }

        public string Supervisor
        {
            get
            {
                string ret = string.Empty;
                if (!string.IsNullOrEmpty(Request.QueryString["Supervisor"]))
                    ret = new Crypt().DecryptString(Request.QueryString["Supervisor"]);
                return ret;
            }
        }

        public string SupervisorFullName
        {
            get
            {
                string ret = string.Empty;
                if (!string.IsNullOrEmpty(Request.QueryString["SupervisorFullName"]))
                    ret = new Crypt().DecryptString(Request.QueryString["SupervisorFullName"]);
                return ret;
            }
        }

        public string TaskStatus
        {
            get
            {
                string ret = string.Empty;
                if (!string.IsNullOrEmpty(Request.QueryString["TaskStatus"]))
                    ret = new Crypt().DecryptString(Request.QueryString["TaskStatus"]);
                return ret;
            }
        }

        public string sol_codigo_temp
        {
            get
            {
                string ret = string.Empty;
                if (!string.IsNullOrEmpty(Request.QueryString["sol_codigo_temp"]))
                    ret = new Crypt().DecryptString(Request.QueryString["sol_codigo_temp"]);
                return ret;
            }
        }

        public string sol_co_solicitud
        {
            get
            {
                string ret = string.Empty;
                if (!string.IsNullOrEmpty(Request.QueryString["sol_co_solicitud"]))
                    ret = new Crypt().DecryptString(Request.QueryString["sol_co_solicitud"]);
                return ret;
            }
        }

        public string sol_co_cliente_principal
        {
            get
            {
                string ret = string.Empty;
                if (!string.IsNullOrEmpty(Request.QueryString["sol_co_cliente_principal"]))
                    ret = new Crypt().DecryptString(Request.QueryString["sol_co_cliente_principal"]);
                return ret;
            }
        }

        public ULA.Quijano.ProcesoLegal.UltimusIntegrationLayer.UltimusIncident Tarea
        {
            get
            {
                if (Session["Tarea"] == null)
                    return null;
                else
                    return Session["Tarea"] as ULA.Quijano.ProcesoLegal.UltimusIntegrationLayer.UltimusIncident;
            }
            set
            {
                Session["Tarea"] = value;
            }
        }

        public void LeerTarea()
        {
            try
            {
                if (Request.Cookies["UserID"] != null && Request.Cookies["TaskID"] != null)
                {
                    Tarea = UltimusManager.GetTask(Request.Cookies["UserID"].Value, Request.Cookies["TaskID"].Value);
                    BorrarCookie("UserID");
                    BorrarCookie("TaskID");
                    logger.Trace(string.Format("SerializeObject TareaJson={0}", JsonConvert.SerializeObject(Tarea)));
                    if (Tarea == null)
                        throw new Exception("TaskID or UserID is null____" + Request.Cookies["UserID"].Value + "____" + Request.Cookies["TaskID"].Value);
                }
                else
                    throw new Exception("TaskID or UserID is null");
            }
            catch (Exception ex)
            {
                logger.Trace("Error TareaJson=" + ex.ToString());
            }
        }

        public ActionResult ErrorGenerico
        {
            get { return View("Error"); }
        }

        private void BorrarCookie(string cookie)
        {
            HttpCookie myCookie = Request.Cookies.Get(cookie);
            if (myCookie != null)
            {
                Request.Cookies.Remove(myCookie.Name);
                myCookie.Expires = DateTime.Now.AddMonths(-1);
                Response.Cookies.Add(myCookie);
            }
        }

        public RedirectResult RedirectEncrypted(string url)
        {
            Crypt crypt = new Crypt();
            return Redirect(string.Format("{0}?UserID={1}&TaskID={2}&Process={3}&Step={4}&Incident={5}", url, UserID, TaskID, Process, Step, Incident));
        }

        public RedirectToRouteResult RedirectToActionEncrypted(string ation, string controller)
        {
            Crypt crypt = new Crypt();
            return RedirectToAction(ation, controller, new { UserID = crypt.EncryptString(UserID), TaskID = crypt.EncryptString(TaskID), Process = crypt.EncryptString(Process), Step = crypt.EncryptString(Step), Incident = crypt.EncryptString(Incident) });
        }

        public void InitControllers()
        {
            ViewBag.IpAdress = Commons.Utilities.GetIpAdress();
            ViewBag.MachineName = Dns.GetHostName();
            ViewBag.ActivityType = "WORKFLOW";
            ViewBag.HostResponse = Commons.Utilities.GetIpAdress();

            ViewBag.Incident = Incident;

            if(Session["_TEMP_INCIDENT"] == null)
                Session["_TEMP_INCIDENT"] = Guid.NewGuid();

            ViewBag.TempIncident = Session["_TEMP_INCIDENT"].ToString();

            ViewBag.UserID = UserID;
            ViewBag.TaskId = TaskID;
            ViewBag.Process = Process;
            ViewBag.Step = Step;
            ViewBag.TaskStatus = TaskStatus;

            ViewBag.JobFunction = JobFunction;
            ViewBag.UserFullName = UserFullName;
            ViewBag.UserEmail = UserEmail;
            ViewBag.Supervisor = Supervisor;
            ViewBag.SupervisorFullName = SupervisorFullName;



            #region Variables of process

            if (Request.QueryString["sol_co_solicitud"] != null && Request.QueryString["sol_co_solicitud"] != "")
                ViewBag.sol_co_solicitud = Request.QueryString["sol_co_solicitud"].ToString();
            else
                ViewBag.sol_co_solicitud = 0;

            if (Request.QueryString["sucursal"] != null)
                ViewBag.sucursal = Request.QueryString["sucursal"].ToString();
            else
                ViewBag.sucursal = 0;

            if (Request.QueryString["cl_co_cliente"] != null)
                ViewBag.cl_co_cliente = Request.QueryString["cl_co_cliente"].ToString();
            else
                ViewBag.cl_co_cliente = 0;


            #endregion

            ViewBag.Pipe = "|";

            try
            {
                Boolean vLocal = false;

                Boolean.TryParse(WebConfigurationManager.AppSettings["ComplementBPM.local"].ToString(), out vLocal);
                
                ////Cambio provisorio
                //var Proceso="";
                //var Etapa="";

                if (vLocal)
                {

                    StepFormsList[] Menulist = new StepFormsList[1];

                    Menulist[0] = new StepFormsList { FormName = "LOCALHOST / TEST" };

                    ViewBag.Menu = Menulist;

                }
                else
                {
                    //if (Process == "" && Step == "")
                    //{
                    //    Proceso = "ServiciosLegales";
                    //    Etapa = "RegistrarSolicitud";
                    //    GeneradorMenu obj = new GeneradorMenu();
                    //    ICollection<ULA.Quijano.ProcesoLegal.StepFormsList> Menulist = obj.GetFormsByProcessAndStep(Proceso, Etapa);//(Process, Step);
                    //    ViewBag.Menu = Menulist;
                    //}
                   
                    GeneradorMenu obj2 = new GeneradorMenu();
                    ICollection<ULA.Quijano.ProcesoLegal.StepFormsList> Menulist2 = obj2.GetFormsByProcessAndStep(Process, Step);

                    ViewBag.Menu = Menulist2;
                }
            }
            catch (Exception ex)
            {

            }

            string QueryString = "?";

            QueryString = string.Concat(QueryString, "UserID={0}");
            QueryString = string.Concat(QueryString, "&TaskID={1}");
            QueryString = string.Concat(QueryString, "&Process={2}");
            QueryString = string.Concat(QueryString, "&Step={3}");
            QueryString = string.Concat(QueryString, "&Incident={4}");
            QueryString = string.Concat(QueryString, "&JobFunction={5}");
            QueryString = string.Concat(QueryString, "&UserFullName={6}");
            QueryString = string.Concat(QueryString, "&UserEmail={7}");
            QueryString = string.Concat(QueryString, "&Supervisor={8}");
            QueryString = string.Concat(QueryString, "&SupervisorFullName={9}");
            QueryString = string.Concat(QueryString, "&TaskStatus={10}");


            ViewBag.QueryEncripted = string.Format(QueryString,
                Request.QueryString["UserID"],
                Request.QueryString["TaskID"],
                Request.QueryString["Process"],
                Request.QueryString["Step"],
                Request.QueryString["Incident"],
                Request.QueryString["JobFunction"],
                Request.QueryString["UserFullName"],
                Request.QueryString["UserEmail"],
                Request.QueryString["Supervisor"],
                Request.QueryString["SupervisorFullName"],
                Request.QueryString["TaskStatus"]);

            #region Variables of process

            if (Request.QueryString["sol_co_solicitud"] != null)
                ViewBag.QueryEncripted = string.Concat(ViewBag.QueryEncripted, "&sol_co_solicitud=", ViewBag.sol_co_solicitud);

            if (Request.QueryString["sucursal"] != null)
                ViewBag.QueryEncripted = string.Concat(ViewBag.QueryEncripted, "&sucursal=", ViewBag.sucursal);

            #endregion

            logger.Trace("ViewBag.QueryEncripted=" + ViewBag.QueryEncripted);
        }

    }
}