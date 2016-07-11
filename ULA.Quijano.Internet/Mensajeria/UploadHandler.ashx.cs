using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.SessionState;
using ULA.Quijano.Internet.AttachmentService;

namespace ULA.Quijano.Internet.Mensajeria
{
    /// <summary>
    /// Summary description for UploadHandler
    /// </summary>
    public class UploadHandler : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                context.Response.ContentType = "text/plain";
                HttpFileCollection files = context.Request.Files;
                if (files.Count > 0)
                {
                    HttpPostedFile file = files[0];
                    byte[] fileData = null;
                    using (var binaryReader = new BinaryReader(file.InputStream))
                    {
                        fileData = binaryReader.ReadBytes(file.ContentLength);
                    }

                    using (AttachmentServiceClient client = new AttachmentServiceClient())
                    {
                        if (!client.UploadFile(Process(), Incident().ToString() + " (Resolucion)", "ResuelveSolicitud", file.FileName, fileData))
                            context.Response.Write("0");
                    }
                }

                context.Response.Write("1");
            }
            catch
            {
                context.Response.Write("0");
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }

        private string Process()
        {
            return (string)HttpContext.Current.Session["process"];
        }

        private int Incident()
        {
            return (int)HttpContext.Current.Session["incident"];
        }

    }
}