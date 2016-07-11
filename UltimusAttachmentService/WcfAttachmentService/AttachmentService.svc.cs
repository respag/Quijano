using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Web;
using System.Text;

namespace WcfAttachmentService
{
    public class AttachmentService : IAttachmentService
    {
        public bool UploadFile(string processName, string incident, string step, string fileName, byte[] fileData)
        {
            FileStream file = null;
            try
            {
                string filePath = string.Empty;
                string mainPath = MainPath();
                if (!Directory.Exists(mainPath))
                    Directory.CreateDirectory(mainPath);

                string processPath = Path.Combine(mainPath, processName);
                if (!Directory.Exists(processPath))
                    Directory.CreateDirectory(processPath);

                string incidentPath = Path.Combine(processPath, incident);
                if (!Directory.Exists(incidentPath))
                    Directory.CreateDirectory(incidentPath);

                filePath = Path.Combine(incidentPath, fileName);

                if (processName.Equals("Comunicacion") && step.Equals("GestionaCorreo"))
                {
                    string stepPath = Path.Combine(incidentPath, step);
                    if (!Directory.Exists(stepPath))
                        Directory.CreateDirectory(stepPath);

                    filePath = Path.Combine(stepPath, fileName);
                }
                
                file = File.Create(filePath);
                file.Write(fileData, 0, fileData.Length);
                file.Close();
                return true;
            }
            catch (Exception e)
            {
                WriteLog(e);
                return false;
            }
            finally
            {
                if (file != null)
                    file.Dispose();
            }
        }

        public string[] GetFileList(string processName, string incident, string step)
        {
            try
            {
                string filePath = string.Empty;
                if (processName.Equals("Comunicacion") && step.Equals("GestionaCorreo"))
                {
                    filePath = Path.Combine(MainPath(), processName, incident, step) + "\\";
                }
                else
                {
                    filePath = Path.Combine(MainPath(), processName, incident) + "\\";
                }

                if (!Directory.Exists(filePath))
                    return new string[0];

                return Directory.GetFiles(filePath).Select(s => s.Replace(filePath, "")).ToArray();
            }
            catch (Exception e)
            {
                WriteLog(e);
                return null;
            }
        }

        public byte[] GetFile(string processName, string incident, string step, string fileName)
        {
            try
            {
                string filePath = string.Empty;
                if (processName.Equals("Comunicacion") && step.Equals("GestionaCorreo"))
                {
                    filePath = Path.Combine(MainPath(), processName, incident, step, fileName);
                }
                else
                {
                    filePath = Path.Combine(MainPath(), processName, incident, fileName);
                }

                if (!File.Exists(filePath))
                    return null;

                return File.ReadAllBytes(filePath);
            }
            catch (Exception e)
            {
                WriteLog(e);
                return null;
            }
        }

        public bool DeleteFile(string processName, string incident, string step, string fileName)
        {
            try
            {
                string filePath = string.Empty;
                if (processName.Equals("Comunicacion") && step.Equals("GestionaCorreo"))
                {
                    filePath = Path.Combine(MainPath(), processName, incident, step, fileName);
                }
                else
                {
                    filePath = Path.Combine(MainPath(), processName, incident, fileName);
                }

                if (File.Exists(filePath))
                    File.Delete(filePath);

                return true;
            }
            catch (Exception e)
            {
                WriteLog(e);
                return false;
            }
        }

        public bool MoveFiles(string processName, string sourceIncident, string targetIncident)
        {
            try
            {
                string sourceIncidentPath = Path.Combine(MainPath(), processName, sourceIncident);
                string targetIncidentPath = Path.Combine(MainPath(), processName, targetIncident);
                if (!Directory.Exists(sourceIncidentPath) || Directory.Exists(targetIncidentPath))
                    return false;

                Directory.Move(sourceIncidentPath, targetIncidentPath);
                return true;
            }
            catch (Exception e)
            {
                WriteLog(e);
                return false;
            }
        }

        public bool DeleteFiles(string processName, string tmpDirectory)
        {
            try
            {
                string directoryPath = Path.Combine(MainPath(), processName, tmpDirectory);

                if (Directory.Exists(directoryPath))
                    Directory.Delete(directoryPath, true);

                return true;
            }
            catch (Exception e)
            {
                WriteLog(e);
                return false;
            }
        }

        private void WriteLog(Exception e)
        {
            const string applicationName = "UltimusAttachmentService";
            try
            {
                string logDirectoryPath = LogPath();
                if (!Directory.Exists(logDirectoryPath))
                    Directory.CreateDirectory(logDirectoryPath);

                string logFilePath = Path.Combine(logDirectoryPath, applicationName + "-" + DateTime.Now.ToString("yyyy-MM-dd") + ".txt");
                StreamWriter sw = new StreamWriter(logFilePath, true);
                string message = string.Format("{0} \r\n {1}", e.Message, e.StackTrace);
                sw.WriteLine(string.Format("\r\nFecha:{0} \r\n {1}", DateTime.Now, message));
                sw.Close();
                sw.Dispose();
            }
            catch (Exception e1)
            {
                Debug.Write(e1.Message);
            }
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

        private string MainPath()
        {
            return GetParameterValue("UltimusAttachmentPath");
        }

        private string LogPath()
        {
            return GetParameterValue("RutaLogGenerados");
        }

    }
}
