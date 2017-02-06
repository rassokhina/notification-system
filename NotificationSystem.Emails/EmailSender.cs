using System.Net;
using System.Net.Mail;
using System.Text;
using NotificationSystem.Emails.Properties;
using NotificationSystem.Queue.Exceptions;

namespace NotificationSystem.Emails {
    public class EmailSender : IEmailSender {
        private readonly SmtpClient m_smtpClient;

        public EmailSender() {
            m_smtpClient = new SmtpClient(Settings.Default.SmtpServer, Settings.Default.SmtpPort) {
                EnableSsl = Settings.Default.SmtpOverSsl
            };
            if (!string.IsNullOrWhiteSpace(Settings.Default.SmtpUsername)
                && !string.IsNullOrWhiteSpace(Settings.Default.SmtpPassword)) {
                m_smtpClient.Credentials = new NetworkCredential(Settings.Default.SmtpUsername, Settings.Default.SmtpPassword);
            }
        }

        public void Send(IEmailNotification message) {
            try {
                var email = message.CreateMailNotification(Encoding.UTF8, true);
                m_smtpClient.Send(email);
            }
            catch (SmtpException ex) {
                throw new CommunicationException(ex.Message, ex);
            }
        }

        public void Dispose() {
            if (m_smtpClient != null) {
                m_smtpClient.Dispose();
            }
        }
    }
}