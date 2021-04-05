using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Utilities.Logger;
using XChange.Api.DTO;
using XChange.Api.Services.Interfaces;

namespace XChange.Api.Services.Concretes
{
    public class EmailService : IEmailService
    {
        private static string ModuleName = "EmailService";
        private readonly EmailConfiguration _emailConfiguration;

        public EmailService(EmailConfiguration emailConfiguration)
        {
            _emailConfiguration = emailConfiguration;
        }

        public void SendEmail(Message message)
        {
            var emailMessage = CreateEmailMessage(message);
            Send(emailMessage);
        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            //emailMessage.From.Add(new MailboxAddress(_emailConfiguration.From));
            emailMessage.From.Add(MailboxAddress.Parse(_emailConfiguration.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            //emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };
            //emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = string.Format("<h2 style='color:red;'>{0}</h2>", message.Content) };

            string FilePath = Directory.GetCurrentDirectory() + "\\Utility\\WelcomeEmailTemplate.html";
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();

            MailText = MailText.Replace("[content]" , message.Content);

            var builder = new BodyBuilder();
            builder.HtmlBody = MailText;
            emailMessage.Body = builder.ToMessageBody();

            return emailMessage;
        }


        private void Send(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_emailConfiguration.UserName, _emailConfiguration.Password);
                    client.Send(mailMessage);
                }
                catch
                {
                    new Logger().LogError(ModuleName, "Send Email", "Error  sending email" + "\n");
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }

    }
}
