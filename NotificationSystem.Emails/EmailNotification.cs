using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using NotificationSystem.Emails.Properties;
using NotificationSystem.Queue.Data;

namespace NotificationSystem.Emails
{
    public class EmailNotification : IEmailNotification
    {
        private readonly Notification m_email;

        public EmailNotification(Notification entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            m_email = entity;
            ParseEntity();
        }

        public string To { get; private set; }
        public string From { get; private set; }
        public string Subject { get; private set; }
        public string Body { get; private set; }
        public string Name { get; private set; }
        public string FromName { get; private set; }

        private void ParseEntity()
        {
            To = m_email.To;
            From = Settings.Default.FromEmail;
            Subject = m_email.Subject;
            Body = m_email.Body;
            Name = m_email.From;
            FromName = Settings.Default.FromName;
        }

        public EmailValidationResult Validate()
        {
            var validationResult = new EmailValidationResult {
                IsValid = true
            };
            try
            {
                new MailAddress(To, Name);
            }
            catch (Exception ex)
            {
                validationResult.IsValid = false;
                validationResult.Messages.Add("To", ex.Message);
            }

            try
            {
                new MailAddress(From, FromName);
            }
            catch (Exception ex)
            {
                validationResult.IsValid = false;
                validationResult.Messages.Add("From", ex.Message);
            }

            if (string.IsNullOrWhiteSpace(Subject))
            {
                validationResult.IsValid = false;
                validationResult.Messages.Add("Subject", "Can not be empty");
            }

            if (string.IsNullOrWhiteSpace(Body))
            {
                validationResult.Messages.Add("Body", "Can not be empty");
            }

            return validationResult;
        }

        public MailMessage CreateMailNotification(Encoding encoding, bool isHtml)
        {
            var validationResult = Validate();
            if (!validationResult.IsValid) {
                throw new FormatException(string.Format("Email has invalid format. Details: {0}.", validationResult));
            }
            MailMessage message = new MailMessage(new MailAddress(From, FromName), new MailAddress(To, Name))
            {
                SubjectEncoding = encoding,
                Subject = Subject,
                IsBodyHtml = isHtml,
                BodyEncoding = encoding,
                Body = Body
            };
            return message;
        }
    }

    public class EmailValidationResult
    {
        public bool IsValid { get; set; }
        public Dictionary<string, string> Messages { get; set; }
        public EmailValidationResult()
        {
            Messages = new Dictionary<string, string>();
        }
    }
}
