using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DiagnoseVirtual.Service.Services
{
    public class EmailService
    {
        private readonly string USERNAME = "qipixel2@gmail.com";
        private readonly string PASSWORD = "qipixel2019";
        private readonly string HOST = "smtp.gmail.com";
        public void SendEmail(MimeMessage message)
        {
            var client = new SmtpClient();
            client.Connect(HOST, 465, true);
            client.Authenticate(USERNAME, PASSWORD);
            client.Send(message);
            client.Disconnect(true);
            client.Dispose();
        }
    }
}
