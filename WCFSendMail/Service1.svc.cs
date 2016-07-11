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
    public class Service1 : IService1
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
                                    string SmtpServer,string Subject,string UserName)
        {
            Email em = new Email();
            bool send = false;
            List<string> result = new List<string>();
            
            try
            {
                string[] list = MailTo.Split(',');
                List<string> mailto = new List<string>(list);

                if(!String.IsNullOrEmpty(Attach))
                {
                    string[] list2 = Attach.Split(',');
                    List<string> attach = new List<string>(list2);
                }                

                em.FromEmail = FromEmail;
                em.MailTo = mailto;
                em.Subject = Subject;
                em.Body = Body;
                //em.Attach = attach;
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

        public void TestCorreo()
        {
            var fromAddress = new MailAddress("riosuke9965@gmail.com", "From Test");
            var toAddress = new MailAddress("angelinhojavier@outlook.com", "To Angel");
            const string fromPassword = "%&#admin#&%";
            const string subject = "Subject";
            const string body = "Body";

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)                
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = body
            })
            {
                smtp.Send(message);
            }
        }
    }
}
