using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ULA.Quijano.Internet.AttachmentService;
using Ultimus.UtilityLayer;

namespace ULA.Quijano.Internet.Mensajeria
{
    public partial class Imagen : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string fileName = Server.UrlDecode(Request.QueryString["i"]);
                string resolucion = Server.UrlDecode(Request.QueryString["r"]);
                using (AttachmentServiceClient client = new AttachmentServiceClient())
                {
                    byte[] fileData = client.GetFile(Process(), Incident().ToString() + (resolucion == "1" ? " (Resolucion)" : ""), "ResuelveSolicitud", fileName);
                    if (fileData != null)
                    {
                        Response.ClearContent();
                        Response.Clear();
                        Response.ContentType = "application/octet-stream";
                        Response.AddHeader("Content-Disposition", "attachment; filename=\"" + fileName + "\";");
                        Response.BinaryWrite(fileData);
                        Response.Flush();
                        Response.End();
                    }
                }
            }
            catch (Exception ex)
            {
                new LogFilecs().EscribirArchivo("Quijano.Internet", ex);
            }
        }

        private string Process()
        {
            return (string)Session["process"];
        }

        private int Incident()
        {
            return (int)Session["incident"];
        }

    }
}