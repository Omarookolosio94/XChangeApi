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

        public void SendEmailWithPDF(Message message , string fileName)
        {
            var emailMessage = CreateEmailMessage(message , fileName);
            Send(emailMessage);
        }


        public async Task SendEmailAsync(Message message)
        {
            var mailMessage = CreateEmailMessage(message);
            await SendAsync(mailMessage);
        }

        private MimeMessage CreateEmailMessage(Message message , string fileName = "")
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(MailboxAddress.Parse(_emailConfiguration.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;

            string FilePath = Directory.GetCurrentDirectory() + "\\Utility\\WelcomeEmailTemplate.html";
            StreamReader str = new StreamReader(FilePath);
            string MailText = str.ReadToEnd();
            str.Close();

            MailText = MailText.Replace("[content]" , message.Content).Replace("[heading]", message.Subject); ;

            var builder = new BodyBuilder();

            if (message.Attachments != null)
            {
                var stream = new MemoryStream(message.Attachments);
                builder.Attachments.Add(fileName, stream , ContentType.Parse("application/pdf"));
            }

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
                    return;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }


        private async Task SendAsync(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync(_emailConfiguration.SmtpServer, _emailConfiguration.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.AuthenticateAsync(_emailConfiguration.UserName, _emailConfiguration.Password);

                    await client.SendAsync(mailMessage);
                }
                catch
                {
                    new Logger().LogError(ModuleName, "Send Email Async", "Error  sending email async" + "\n");
                    return;
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
        }
    }
}
