using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;

namespace AspNetIdentity.WebApi.Services
{
    public class EmailService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            await Task.Run(() => ConfigEmail(message));
        }

        private void ConfigEmail(IdentityMessage message)
        {
            var mail = new MailMessage("houssem.dellai@live.com",
                                       "houssem.dellai@gmail.com",
                                       message.Subject,
                                       message.Body);

            var client = new SmtpClient("smtp.live.com", 587)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential("houssem.dellai@live.com", "password")
            };

            client.Send(mail);

            // Use NuGet to install SendGrid (Basic C# client lib) 
            //var myMessage = new SendGridMessage();

            //myMessage.AddTo(message.Destination);
            //myMessage.From = new System.Net.Mail.MailAddress("taiseer@bitoftech.net", "Taiseer Joudeh");
            //myMessage.Subject = message.Subject;
            //myMessage.Text = message.Body;
            //myMessage.Html = message.Body;

            //var credentials = new NetworkCredential(ConfigurationManager.AppSettings["emailService:Account"],
            //                                        ConfigurationManager.AppSettings["emailService:Password"]);

            //// Create a Web transport for sending email.
            //var transportWeb = new Web(credentials);

            //// Send the email.
            //if (transportWeb != null)
            //{
            //    await transportWeb.DeliverAsync(myMessage);
            //}
            //else
            //{
            //    //Trace.TraceError("Failed to create Web transport.");
            //    await Task.FromResult(0);
            //}
        }
    }
}
