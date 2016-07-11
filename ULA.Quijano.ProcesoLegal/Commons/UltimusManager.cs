using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Configuration;
using System.Xml.Linq;
using ULA.Quijano.ProcesoLegal.UltimusIntegrationLayer;

namespace ULA.Quijano.ProcesoLegal.Commons
{
    public class UltimusManager
    {
        public static bool Autenticate(string Usuario, string Contrasena)
        {
            bool ret = false;
            using (UltimusIntegrationClient client = new UltimusIntegrationClient())
            {
                bool autenticado;
                string error;
                if (client.Autenticate(Usuario, Contrasena, out autenticado, out error))
                    ret = autenticado;
            }
            return ret;
        }

        public static string GetSignature(string Usuario)
        {
            string ret = string.Empty;
            using (UltimusIntegrationClient client = new UltimusIntegrationClient())
            {
                string urlFirma;
                string error;
                string urlBase = WebConfigurationManager.AppSettings["ULA.Signature.RepositoryUrl"];
                if (client.GetSignature(Usuario, urlBase, out urlFirma, out error))
                    ret = urlFirma;
            }
            return ret;
        }

        public static ULA.Quijano.ProcesoLegal.UltimusIntegrationLayer.UltimusIncident GetTask(string UserID, string TaskID)
        {
            ULA.Quijano.ProcesoLegal.UltimusIntegrationLayer.UltimusIncident ret = new ULA.Quijano.ProcesoLegal.UltimusIntegrationLayer.UltimusIncident();
            string error;
            using (UltimusIntegrationClient client = new UltimusIntegrationClient())
            {
                if (!client.GetTask(UserID, TaskID, out ret, out error))
                    throw new Exception(UserID + "__" + TaskID + "__" + error);
            }
            return ret;
        }

        public static void GetNodeValue(string UserID, string TaskID, string node, out string value)
        {
            string error;
            using (UltimusIntegrationClient client = new UltimusIntegrationClient())
            {
                if (!client.GetNodeValue(UserID, TaskID, node, out value, out error))
                    throw new Exception(UserID + "__" + TaskID + "__" + error);
            }
        }

        public static int CompleteTask(string UserID, string TaskID, string memo, string summary, List<NodeVariables> NodeVariablesList)
        {
            int ret;
            ULA.Quijano.ProcesoLegal.UltimusIntegrationLayer.UltimusIncident ui;
            string error;
            using (UltimusIntegrationClient client = new UltimusIntegrationClient())
            {
                if (client.GetTask(UserID, TaskID, out ui, out error))
                {
                    string xml = ui.XmlData;


                    if (!client.CompleteTaskWithVariables(UserID, TaskID, memo, summary, NodeVariablesList, out ret, out error))
                        throw new Exception(error);
                }
                else
                    throw new Exception(error);
            }
            return ret;
        }

        public static ULA.Quijano.ProcesoLegal.UltimusIntegrationLayer.UltimusIncident GetTaskByFilters(string UserID, string Process, string Step, int incident)
        {
            ULA.Quijano.ProcesoLegal.UltimusIntegrationLayer.UltimusIncident ui;
            string error;
            using (UltimusIntegrationClient client = new UltimusIntegrationClient())
            {
                if (client.GetTaskByFilters(UserID, Process, Step, incident, out ui, out error))
                {
                    return ui;
                }
                else
                    throw new Exception(error);
            }
        }

    }
}