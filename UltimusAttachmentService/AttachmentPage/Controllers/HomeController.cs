using AttachmentPage.AttachmentService;
using AttachmentPage.Models;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;

namespace AttachmentPage.Controllers
{
    public class HomeController : Controller
    {
        Dictionary<string, string> parametros = new Dictionary<string, string>();

        #region -- PUBLIC --

        public ActionResult Index(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            LoadParameters();

            string processName = ProcessName();
            string incident = Incident();

            ViewData["process"] = processName;
            ViewData["incident"] = incident;
            ViewData["enableUpload"] = EnableUpload();
            ViewData["visible"] = VisibleTitleBar();
            ViewData["view"] = StyleView().ToString();
            ViewData["maxFiles"] = MaxFiles().ToString();
            ViewData["acceptOnlyImages"] = AcceptOnlyImages() ? "1" : "0";
            ViewData["attachmentMaxSize"] = (AttachmentMaxSize() * 1024 * 1024).ToString();
            ViewData["etapa"] = Etapa();

            List<Attachment> list = new List<Attachment>();
            if (!(string.IsNullOrEmpty(processName) || string.IsNullOrEmpty(incident)))
            {
                list = LoadFileList(processName, incident, Etapa(), EnableUpload());
            }

            return View(list);
        }

        [HttpPost]
        public virtual ActionResult UploadFile(string processName, string incident, string step, int attachmentMaxSize, int maxFiles)
        {
            bool ret = false;
            HttpPostedFileBase myFile = Request.Files["files[]"];

            if (myFile.ContentLength > attachmentMaxSize)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Content("El Adjunto no debe superar los " + (attachmentMaxSize / (1024 * 1024)).ToString() + "MB.");
            }

            try
            {
                AttachmentServiceClient client = new AttachmentServiceClient();
                if (client.GetFileList(processName, incident, step).Length >= maxFiles)
                {
                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    return Content("No pueden ser adjuntados mas de " + maxFiles.ToString() + " archivos");
                }

                byte[] data;
                using (Stream inputStream = myFile.InputStream)
                {
                    MemoryStream memoryStream = inputStream as MemoryStream;
                    if (memoryStream == null)
                    {
                        memoryStream = new MemoryStream();
                        inputStream.CopyTo(memoryStream);
                    }
                    data = memoryStream.ToArray();
                }

                ret = client.UploadFile(processName, incident, step, myFile.FileName, data);
            }
            catch (Exception e) 
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Content("No se pudo subir el archivo");
            }            

            Response.StatusCode = (int)HttpStatusCode.OK;
            return Json(new { result = "OK"}, JsonRequestBehavior.AllowGet);
        }

        public JsonResult LoadFiles(string processName, string incident, string step, bool enableUpload)
        {
            var list = LoadFileList(processName, incident, step, enableUpload);

            return Json(list, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetFile(string processName, string incident, string step, string fileName)
        {
            AttachmentServiceClient client = new AttachmentServiceClient();
            byte[] fileData = client.GetFile(processName, incident, step, fileName);

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

            return File(fileData, "application/octet-stream", fileName);
        }

        public ActionResult DeleteFile(string processName, string incident, string step, string fileName)
        {
            AttachmentServiceClient client = new AttachmentServiceClient();
            if (!client.DeleteFile(processName, incident, step, fileName))
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                return Content("No se pudo eliminar el archivo");
            }

            Response.StatusCode = (int)HttpStatusCode.OK;
            return Content("OK", "text/plain");
        }

        #endregion

        #region -- PRIVATE --

        private List<Attachment> LoadFileList(string processName, string incident, string step, bool enableUpload)
        {
            List<Attachment> list = new List<Attachment>();
            AttachmentServiceClient client = new AttachmentServiceClient();
            string[] files = client.GetFileList(processName, incident, step);

            files.ToList().ForEach(f => list.Add(new Attachment()
            {
                FileName = f,
                FileType = f.Substring(f.LastIndexOf(".") + 1),
                EnableDelete = false // enableUpload
            }));

            return list;
        }

        private void LoadParameters()
        {
            if (string.IsNullOrEmpty(Request.QueryString.ToString()))
                return;

            parametros = new Dictionary<string, string>();
            string[] parameters = DecryptString(Server.UrlDecode(Request.QueryString.ToString())).Split('&');
            foreach (string s in parameters)
            {
                string key = s.Substring(0, s.IndexOf("="));
                string value = s.Substring(key.Length + 1);
                parametros.Add(key, value);
            }
        }

        private int AttachmentMaxSize()
        {
            string attachmentMaxSize = string.Empty;
            if (parametros.TryGetValue("attachmentMaxSize", out attachmentMaxSize))
                return int.Parse(attachmentMaxSize);
            else
                return int.Parse(GetParameterValue("UltimusAttachmentMaxSize"));
        }

        private string ProcessName()
        {
            string processName = string.Empty;
            parametros.TryGetValue("process", out processName);
            return processName;
        }

        private string Incident()
        {
            string incident = string.Empty;
            parametros.TryGetValue("incident", out incident);
            return incident;
        }

        private bool EnableUpload()
        {
            string upload = string.Empty;
            parametros.TryGetValue("upload", out upload);
            return upload == "1";
        }

        /*
        private bool EnableDelete()
        {
            string delete = string.Empty;
            parametros.TryGetValue("delete", out delete);
            return delete == "1";
        }
        */

        private bool VisibleTitleBar()
        {
            string title = string.Empty;
            parametros.TryGetValue("title", out title);
            return title == "1";
        }

        private int StyleView()
        {
            string style = string.Empty;
            parametros.TryGetValue("style", out style);
            return string.IsNullOrEmpty(style) ? 1 : int.Parse(style);
        }

        private int MaxFiles()
        {
            string maxFiles = string.Empty;
            if(parametros.TryGetValue("maxFiles", out maxFiles))
                return int.Parse(maxFiles);
            else
                return int.MaxValue;
        }

        private bool AcceptOnlyImages()
        {
            string acceptOnlyImages = string.Empty;
            if (parametros.TryGetValue("acceptOnlyImages", out acceptOnlyImages))
                return acceptOnlyImages == "1";
            else
                return false;
        }
        public string Etapa()
        {
            string etapa = string.Empty;
            parametros.TryGetValue("etapa", out etapa);
            return etapa;
        }

        private string GetParameterValue(string parameterName)
        {
            const string pathKey = "SOFTWARE\\ParametrosProcesoUltimus";
            string value = string.Empty;

            RegistryKey rk = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, Environment.Is64BitOperatingSystem ? RegistryView.Registry64 : RegistryView.Registry32).OpenSubKey(pathKey);
            if (rk != null)
            {
                value = (string)rk.GetValue(parameterName);
                rk.Close();
            }
            return value;
        }

        private string DecryptString(string message)
        {
            const string key = "ultimusPanama";
            MD5CryptoServiceProvider HashProvider = null;
            TripleDESCryptoServiceProvider TDESAlgorithm = null;

            try
            {
                System.Text.UTF8Encoding UTF8 = new System.Text.UTF8Encoding();
                HashProvider = new MD5CryptoServiceProvider();
                byte[] TDESKey = HashProvider.ComputeHash(UTF8.GetBytes(key));
                TDESAlgorithm = new TripleDESCryptoServiceProvider() { Key = TDESKey, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 };
                byte[] DataToDecrypt = Convert.FromBase64String(message);
                return UTF8.GetString(TDESAlgorithm.CreateDecryptor().TransformFinalBlock(DataToDecrypt, 0, DataToDecrypt.Length));
            }
            catch
            {
                return string.Empty;
            }
            finally
            {
                TDESAlgorithm.Clear();
                HashProvider.Clear();
            }
        }

        #endregion
    }
}