using System.Net.Mail;
using System.Text;

namespace NotificationSystem.Emails
{
    public interface IEmailNotification
    {
        MailMessage CreateMailNotification(Encoding encoding, bool isHtml);
    }
}
