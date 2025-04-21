using Business.GenericRepository.BaseRep;
using Business.ServiceSettings;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Business.GenericRepository.ConcManager
{
    public class MailManager : IMailService
    {
        private readonly MailSettings _mailSettings;

        public MailManager(IOptions<MailSettings> mailSettings)
        {
            _mailSettings = mailSettings.Value;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {

            var client = new SmtpClient(_mailSettings.Host, _mailSettings.Port)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(_mailSettings.UserName, _mailSettings.Password)
            };
            var mailMessage = new MailMessage
            {

                From = new MailAddress(_mailSettings.SenderEmail),
                Subject = subject,
                Body = message,
                IsBodyHtml = false
            };

            mailMessage.To.Add(new MailAddress(email));

            return client.SendMailAsync(mailMessage);

        }



        /*return client.SendMailAsync(
            new MailMessage(from : _mailSettings.SenderEmail ,
                           to : email,
                           subject,
                   message));*/

    }
}
