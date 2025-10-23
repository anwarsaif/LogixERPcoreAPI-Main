using DocumentFormat.OpenXml.Spreadsheet;
using Logix.Application.Common;
using Logix.Application.Interfaces.IRepositories;
using Logix.Application.Interfaces.IServices;
using Logix.Application.Interfaces.IServices.HR;
using Logix.Application.Services;
using Logix.Application.Services.HR;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Hosting;
using MimeKit;
using System.Collections.Generic;
using System.Net.Mail;
using System.Threading.Tasks;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace Logix.Application.Helpers
{
    public interface IEmailAppHelper
    {
        Task SendEmailAsync(string toEmail, string subject, string body, long mailServerId = 1);
        Task SendEmailWithAttachmentAsync(string toEmail, string subject, string body, long mailServerId = 1, List<string> attachments = null);
        Task SendEmailWithAttachmentAsync(List<string> toEmails, string subject, string mailbody, long mailServerId = 1, List<string> attachments = null);

    }
    public class EmailAppHelper : IEmailAppHelper
    {
        private readonly IMainRepositoryManager mainRepositoryManager;
        private readonly IAccRepositoryManager accRepositoryManager;
        private readonly ICurrentData _session;
        private readonly IWebHostEnvironment _hostingEnvironment;


        public EmailAppHelper(IMainRepositoryManager mainRepositoryManager, IAccRepositoryManager accRepositoryManager, ICurrentData session, IWebHostEnvironment hostingEnvironment)
        {

            this.mainRepositoryManager = mainRepositoryManager;
            this.accRepositoryManager = accRepositoryManager;
            _session = session;
            _hostingEnvironment = hostingEnvironment;
        }
        public async Task SendEmailAsync(string toEmail, string subject, string mailbody, long mailServerId = 1)
        {
            try
            {
                if (IsValidEmail(toEmail))
                {
                  
                }
                long faciltitId = 1;

                if (_session.FacilityId > 0)
                {
                    faciltitId = _session.FacilityId;
                }
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
                    var facility = await accRepositoryManager.AccFacilityRepository.GetById(faciltitId);
                    if (facility != null)
                    {
                        body = body.Replace("CompanyName", facility.FacilityName);
                        body = body.Replace("CompanyAddress", facility.FacilityAddress);
                        body = body.Replace("CompanyPhone", facility.FacilityPhone);
                    }
                }
                catch (Exception)
                {
                    throw;
                }

                SmtpClient _smtpClient = new();
                string _fromEmail = ""; string _smtpServer = ""; int _smtpPort = 0; string _username = ""; string _password = "";

                if (mailServerId == 0)
                    mailServerId = 1;
                var mailServer = await mainRepositoryManager.SysMailServerRepository.GetOne(x => x.Id == mailServerId);

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
                //message.To.Add(new MailboxAddress("", toEmail));
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
                catch (SmtpCommandException)
                {
                    // Handle the exception or log the error message
                    throw;
                }
                finally
                {
                    await _smtpClient.DisconnectAsync(true);
                }
            }
            catch (Exception)
            {

                throw;
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
                var facility = await accRepositoryManager.AccFacilityRepository.GetById(_session.FacilityId);
                if (facility != null)
                {
                    body = body.Replace("CompanyName", facility.FacilityName);
                    body = body.Replace("CompanyAddress", facility.FacilityAddress);
                    body = body.Replace("CompanyPhone", facility.FacilityPhone);
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
            var mailServer = await mainRepositoryManager.SysMailServerRepository.GetOne(x => x.Id == mailServerId);

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
        public async Task SendEmailWithAttachmentAsync(List<string> toEmails, string subject, string mailbody, long mailServerId = 1, List<string> attachments = null)
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
                var facility = await accRepositoryManager.AccFacilityRepository.GetById(_session.FacilityId);
                if (facility != null)
                {
                    body = body.Replace("CompanyName", facility.FacilityName);
                    body = body.Replace("CompanyAddress", facility.FacilityAddress);
                    body = body.Replace("CompanyPhone", facility.FacilityPhone);
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

            var mailServer = await mainRepositoryManager.SysMailServerRepository.GetOne(x => x.Id == mailServerId);

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

            // Loop through the list of emails and add them to the "To" field
            foreach (var email in toEmails)
            {
                message.To.Add(new MailboxAddress("", email));
            }

            message.Subject = subject;

            var bodyBuilder = new BodyBuilder();
            bodyBuilder.HtmlBody = body;

            // Add attachments if present
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
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email; 
            }
            catch
            {
                return false; 
            }
        }

    }


}