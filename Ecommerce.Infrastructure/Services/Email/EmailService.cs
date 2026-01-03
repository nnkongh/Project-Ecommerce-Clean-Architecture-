using Ecommerce.Application.DTOs.EmailMessage;
using Ecommerce.Application.Interfaces;
using Ecommerce.Infrastructure.Exceptions;
using Ecommerce.Infrastructure.Interfaces.Authentication;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace Ecommerce.Infrastructure.Services.Email
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfiguration;

        public EmailService(IOptions<EmailConfiguration> opt) 
        { 
            _emailConfiguration = opt.Value ?? throw new InvalidOperationException("Emailconfig not load"); 
        }
        public async Task SendEmail(Message msg)
        {
            var mimeMessage = new MimeMessage();
            mimeMessage.From.Add(new MailboxAddress("",_emailConfiguration.FromAddress));
            mimeMessage.To.AddRange(msg.To.Select(x => MailboxAddress.Parse(x)));
            mimeMessage.Subject = msg.Subject;
            mimeMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = msg.Body };

            using (var client = new SmtpClient())
            {
                try
                {
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.ConnectAsync(_emailConfiguration.SmtpServer, _emailConfiguration.Port, true);
                    await client.AuthenticateAsync(_emailConfiguration.Username, _emailConfiguration.Password);
                    await client.SendAsync(mimeMessage);
                }catch(SmtpCommandException e)
                {
                    throw new EmailSenderException("Error sending email", e);
                }
                finally
                {
                    await client.DisconnectAsync(true);
                }
            }
        }
    }
}
