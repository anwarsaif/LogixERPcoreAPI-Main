//using Microsoft.Extensions.Options;
//using System.Net;
//using System.Net.Mail;
//using Logix.Application.Interfaces.IServices;
//using Microsoft.AspNetCore.Http;
//using System.Collections.Generic;
//using System.Threading.Tasks;
//using System.Net.Http;
//using MailKit.Net.Smtp;
//using Logix.Application.Common;
using Logix.Application.Common;
using Logix.Application.Interfaces.IServices;
using MailKit.Net.Smtp;
using MimeKit;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Logix.MVC.Helpers
{
    public interface IEmailService
    {
        Task SendEmailAsync(string toEmail, string subject, string body, long mailServerId = 1);
        Task SendEmailWithAttachmentAsync(string toEmail, string subject, string body, long mailServerId = 1, List<string> attachments = null);
    }


    public class EmailService : IEmailService
    {
        private readonly IMainServiceManager _serviceManager;
        private readonly IAccServiceManager _accServiceManager;
        private readonly ICurrentData _session;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public EmailService(IMainServiceManager serviceManager,
            IAccServiceManager accServiceManager,
            ICurrentData session,
            IWebHostEnvironment hostingEnvironment,
            string smtpServer = "smtp.gmail.com",
            int smtpPort = 465,
            string username = "noreply@logix-erp.com",
            string password = "Taha@452010",
            string fromEmail = "noreply@logix-erp.com")
        {
            _serviceManager = serviceManager;
            _accServiceManager = accServiceManager;
            _session = session;
            _hostingEnvironment = hostingEnvironment;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string mailbody, long mailServerId = 1)
        {
            var body = string.Empty;
            var xfile = Path.Combine(_hostingEnvironment.WebRootPath, "newsletter", "newsletter.html");

            using (var reader = new StreamReader(xfile))
            {
                body = await reader.ReadToEndAsync();
            }

            body = body.Replace("MessageTitle", subject);
            body = body.Replace("MessageBody", mailbody);

            try
            {
                var facility = await _accServiceManager.AccFacilityService.GetById(_session.FacilityId);
                if (facility.Succeeded)
                {
                    body = body.Replace("CompanyName", facility.Data.FacilityName);
                    body = body.Replace("CompanyAddress", facility.Data.FacilityAddress);
                    body = body.Replace("CompanyPhone", facility.Data.FacilityPhone);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            SmtpClient _smtpClient = new();
            string _fromEmail = ""; string _smtpServer = ""; int _smtpPort = 0; string _username = ""; string _password = "";

            if (mailServerId == 0)
                mailServerId = 1;

            var mailServer = _serviceManager.SysMailServerService.GetOne(x => x.Id == mailServerId).Result.Data;
            if (mailServer != null)
            {
                _smtpServer = mailServer.SmtpServer ?? "";
                _smtpPort = Int32.Parse(mailServer.SmtpPort ?? "");
                _username = mailServer.Username ?? "";
                _password = mailServer.Password ?? "";
                _fromEmail = mailServer.Username ?? "";
                _smtpClient = new SmtpClient();
            }

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("", _fromEmail));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = body;

            message.Body = bodyBuilder.ToMessageBody();

            try
            {
                await _smtpClient.ConnectAsync(_smtpServer, _smtpPort, true); // Connect with SSL/TLS
                await _smtpClient.AuthenticateAsync(_username, _password);
                await _smtpClient.SendAsync(message);
            }
            catch (SmtpCommandException ex)
            {
                // Handle the exception or log the error message
                throw ex;
            }
            finally
            {
                await _smtpClient.DisconnectAsync(true);
            }
        }

        public async Task SendEmailWithAttachmentAsync(string toEmail, string subject, string mailbody, long mailServerId = 1, List<string> attachments = null)
        {
            var body = string.Empty;
            var xfile = Path.Combine(_hostingEnvironment.WebRootPath, "newsletter", "newsletter.html");

            using (var reader = new StreamReader(xfile))
            {
                body = await reader.ReadToEndAsync();
            }

            body = body.Replace("MessageTitle", subject);
            body = body.Replace("MessageBody", mailbody);

            try
            {
                var facility = await _accServiceManager.AccFacilityService.GetById(_session.FacilityId);
                if (facility.Succeeded)
                {
                    body = body.Replace("CompanyName", facility.Data.FacilityName);
                    body = body.Replace("CompanyAddress", facility.Data.FacilityAddress);
                    body = body.Replace("CompanyPhone", facility.Data.FacilityPhone);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            SmtpClient _smtpClient = new();
            string _fromEmail = ""; string _smtpServer = ""; int _smtpPort = 0; string _username = ""; string _password = "";

            if (mailServerId == 0)
                mailServerId = 1;

            var mailServer = _serviceManager.SysMailServerService.GetOne(x => x.Id == mailServerId).Result.Data;
            if (mailServer != null)
            {
                _smtpServer = mailServer.SmtpServer ?? "";
                _smtpPort = Int32.Parse(mailServer.SmtpPort ?? "");
                _username = mailServer.Username ?? "";
                _password = mailServer.Password ?? "";
                _fromEmail = mailServer.Username ?? "";
                _smtpClient = new SmtpClient();
            }

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("", _fromEmail));
            message.To.Add(new MailboxAddress("", toEmail));
            message.Subject = subject;

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = body;

            if (attachments != null)
            {
                foreach (var attachmentPath in attachments)
                {
                    var path = Path.Combine(_hostingEnvironment.WebRootPath, attachmentPath);
                    bodyBuilder.Attachments.Add(path);
                }
            }
            message.Body = bodyBuilder.ToMessageBody();

            try
            {
                await _smtpClient.ConnectAsync(_smtpServer, _smtpPort, true); // Connect with SSL/TLS
                await _smtpClient.AuthenticateAsync(_username, _password);
                await _smtpClient.SendAsync(message);
            }
            catch (SmtpCommandException ex)
            {
                // Handle the exception or log the error message
                throw ex;
            }
            finally
            {
                await _smtpClient.DisconnectAsync(true);
            }
        }
    
    }
}

//public class EmailService : IEmailService
//{
//    private readonly System.Net.Mail.SmtpClient _smtpClient;
//    private readonly string _fromEmail;
//    private readonly IMainServiceManager _serviceManager;
//    private readonly IAccServiceManager _accServiceManager;
//    private readonly ICurrentData _session;
//    private readonly IWebHostEnvironment _hostingEnvironment;

//    public EmailService(IMainServiceManager serviceManager, IAccServiceManager accServiceManager, ICurrentData session, IWebHostEnvironment hostingEnvironment)
//    {
//        _serviceManager = serviceManager;
//        _accServiceManager = accServiceManager;
//        _session = session;
//        _hostingEnvironment = hostingEnvironment;

//        //    var mailServer = _serviceManager.SysMailServerService.GetOne(x => x.Id == 1).Result.Data;
//        //    _smtpClient = new SmtpClient
//        //    {
//        //        Host = mailServer.SmtpServer,
//        //        Port = mailServer.SmtpPort,
//        //        EnableSsl = mailServer.Ssl,
//        //        DeliveryMethod = SmtpDeliveryMethod.Network,
//        //        UseDefaultCredentials = false,
//        //        Credentials = new NetworkCredential(mailServer.Username, mailServer.Password)
//        //    };
//        //    _fromEmail = mailServer.Username;
//        //}
//        //var mailServer = _serviceManager.SysMailServerService.GetOne(x => x.Id == 1).Result.Data;
//        _smtpClient = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587)
//        {
//            Host = "smtp.gmail.com",
//            Port = 587,
//            EnableSsl = true,
//            DeliveryMethod = SmtpDeliveryMethod.Network,
//            UseDefaultCredentials = false,
//            Credentials = new NetworkCredential("rahalshamiri@gmail.com", "Raheeb123@")
//        };
//        _fromEmail = "rahalshamiri@gmail.com";
//    }

//    public async Task SendEmailAsync(string toEmail, string subject, string mailbody, List<string>? attachments = null)
//    {
//        var body = string.Empty;
//        var xfile = Path.Combine(_hostingEnvironment.WebRootPath, "newsletter", "newsletter.html");

//        using (var reader = new StreamReader(xfile))
//        {
//            body = await reader.ReadToEndAsync();
//        }

//        body = body.Replace("MessageTitle", subject);
//        body = body.Replace("MessageBody", mailbody);

//        try
//        {
//            var facility = await _accServiceManager.AccFacilityService.GetById(_session.FacilityId);
//            if (facility.Succeeded)
//            {
//                body = body.Replace("CompanyName", facility.Data.FacilityName);
//                body = body.Replace("CompanyAddress", facility.Data.FacilityAddress);
//                body = body.Replace("CompanyPhone", facility.Data.FacilityPhone);
//            }
//        }
//        catch (Exception ex)
//        {
//            // Handle the exception
//        }

//        var message = new MailMessage();
//        message.From = new MailAddress(_fromEmail);
//        message.To.Add(new MailAddress(toEmail));
//        message.Subject = subject;
//        message.Body = body;
//        message.IsBodyHtml = true;

//        if (attachments != null)
//        {
//            foreach (var attachmentPath in attachments)
//            {
//                var attachment = new Attachment(attachmentPath);
//                message.Attachments.Add(attachment);
//            }
//        }

//        await _smtpClient.SendMailAsync(message);
//    }
//}







//public class EmailService : IEmailService
//{

//}        private readonly System.Net.Mail.SmtpClient _smtpClient;
//    private readonly string _fromEmail;

//    public EmailService(string smtpServer= "smtp.gmail.com", int smtpPort = 587, string username= "rahalshamiri@gmail.com", string password= "Raheeb123@", string fromEmail = "rahalshamiri@gmail.com", bool EnableSsl=true)
//    {
//        _smtpClient = new System.Net.Mail.SmtpClient(smtpServer, smtpPort)
//        {

//            Host= smtpServer,
//            Port= smtpPort,
//            UseDefaultCredentials = false,
//            DeliveryMethod=SmtpDeliveryMethod.Network,
//            Credentials = new NetworkCredential(username, password),
//            EnableSsl = EnableSsl

//        };
//        _fromEmail = fromEmail;
//    }

//    public async Task SendEmailAsync(string toEmail, string subject, string body, List<string> attachments = null)
//    {
//        var message = new MailMessage
//        {
//            From = new MailAddress(_fromEmail),
//            Subject = subject,
//            Body = body,
//            IsBodyHtml = true
//        };
//        message.To.Add(toEmail);

//        if (attachments != null)
//        {
//            foreach (var attachmentPath in attachments)
//            {
//                var attachment = new Attachment(attachmentPath);
//                message.Attachments.Add(attachment);
//            }
//        }


//        try
//        {
//            await _smtpClient.SendMailAsync(message);
//        }
//        catch (SmtpException ex)
//        {
//            // Handle the exception or log the error message
//            throw ex;
//        }
//    }
//}
//public class EmailService : IEmailService
//{
//    private readonly System.Net.Mail.SmtpClient _smtpClient;
//    private readonly string _fromEmail;
//    private readonly IMainServiceManager _serviceManager;
//    private readonly IAccServiceManager _accServiceManager;
//    private readonly ICurrentData _session;
//    private readonly IWebHostEnvironment _hostingEnvironment;

//    public EmailService(IMainServiceManager serviceManager, IAccServiceManager accServiceManager, ICurrentData session, IWebHostEnvironment hostingEnvironment)
//    {
//        _serviceManager = serviceManager;
//        _accServiceManager = accServiceManager;
//        _session = session;
//        _hostingEnvironment = hostingEnvironment;

//        //    var mailServer = _serviceManager.SysMailServerService.GetOne(x => x.Id == 1).Result.Data;
//        //    _smtpClient = new SmtpClient
//        //    {
//        //        Host = mailServer.SmtpServer,
//        //        Port = mailServer.SmtpPort,
//        //        EnableSsl = mailServer.Ssl,
//        //        DeliveryMethod = SmtpDeliveryMethod.Network,
//        //        UseDefaultCredentials = false,
//        //        Credentials = new NetworkCredential(mailServer.Username, mailServer.Password)
//        //    };
//        //    _fromEmail = mailServer.Username;
//        //}
//        //var mailServer = _serviceManager.SysMailServerService.GetOne(x => x.Id == 1).Result.Data;
//        _smtpClient = new System.Net.Mail.SmtpClient("smtp.gmail.com", 587)
//        {
//            Host = "smtp.gmail.com",
//            Port = 587,
//            EnableSsl = true,
//            DeliveryMethod = SmtpDeliveryMethod.Network,
//            UseDefaultCredentials = false,
//            Credentials = new NetworkCredential("rahalshamiri@gmail.com", "Raheeb123@")
//        };
//        _fromEmail = "rahalshamiri@gmail.com";
//    }

//    public async Task SendEmailAsync(string toEmail, string subject, string mailbody, List<string>? attachments = null)
//    {
//        var body = string.Empty;
//        var xfile = Path.Combine(_hostingEnvironment.WebRootPath, "newsletter", "newsletter.html");

//        using (var reader = new StreamReader(xfile))
//        {
//            body = await reader.ReadToEndAsync();
//        }

//        body = body.Replace("MessageTitle", subject);
//        body = body.Replace("MessageBody", mailbody);

//        try
//        {
//            var facility = await _accServiceManager.AccFacilityService.GetById(_session.FacilityId);
//            if (facility.Succeeded)
//            {
//                body = body.Replace("CompanyName", facility.Data.FacilityName);
//                body = body.Replace("CompanyAddress", facility.Data.FacilityAddress);
//                body = body.Replace("CompanyPhone", facility.Data.FacilityPhone);
//            }
//        }
//        catch (Exception ex)
//        {
//            // Handle the exception
//        }

//        var message = new MailMessage();
//        message.From = new MailAddress(_fromEmail);
//        message.To.Add(new MailAddress(toEmail));
//        message.Subject = subject;
//        message.Body = body;
//        message.IsBodyHtml = true;

//        if (attachments != null)
//        {
//            foreach (var attachmentPath in attachments)
//            {
//                var attachment = new Attachment(attachmentPath);
//                message.Attachments.Add(attachment);
//            }
//        }

//        await _smtpClient.SendMailAsync(message);
//    }
//}

