using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using Ultimus.Utilitarios;
using System.Net;
using System.Net.Mail;

namespace WCFSendMail
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class SendMail : IService1
    {
        public string GetData(int value)
        {
            return string.Format("You entered: {0}", value);
        }

        public CompositeType GetDataUsingDataContract(CompositeType composite)
        {
            if (composite == null)
            {
                throw new ArgumentNullException("composite");
            }
            if (composite.BoolValue)
            {
                composite.StringValue += "Suffix";
            }
            return composite;
        }

        public List<string> EnviarCorreo(string Attach, string Body, string Domain, string FromEmail, bool IsBodyHtml, bool IsSend, string MailTo, string Password, int SmtpPort,
                                    string SmtpServer, string Subject, string UserName)
        {
            Email em = new Email();
            bool send = false;
            List<string> result = new List<string>();

            try
            {
                string[] list = MailTo.Split(',');
                List<string> mailto = new List<string>(list);
                List<string> attach = new List<string>();

                if (!String.IsNullOrEmpty(Attach))
                {
                    string[] list2 = Attach.Split(',');
                    attach = new List<string>(list2);
                }

                em.FromEmail = FromEmail;
                em.MailTo = mailto;
                em.Subject = Subject;
                em.Body = Body;
                em.Attach = attach;
                em.Domain = Domain;
                em.Password = Password;
                em.UserName = UserName;
                em.SmtpPort = SmtpPort;
                em.SmtpServer = SmtpServer;

                em.SendMail();

            }
            catch (Exception ex)
            {
                result.Add(ex.Message);
            }

            return result;
        }

    }
}
