using System;
using NotificationSystem.Queue;
using NotificationSystem.Queue.Data;
using NotificationSystem.Queue.Exceptions;

namespace NotificationSystem.Emails
{
    public class EmailProcessor
    {
        private readonly INotificationService m_notificationService;

        public EmailProcessor(INotificationService notificationService)
        {
            if (notificationService == null)
            {
                throw new ArgumentNullException("notificationService");
            }
            m_notificationService = notificationService;
        }

        public void Process(Notification notification)
        {
            try {
                using (IEmailSender sender = new EmailSender()) {
                    var emailMessage = new EmailNotification(notification);
                    var validationResult = emailMessage.Validate();
                    if (validationResult.IsValid) {
                        sender.Send(emailMessage);
                    }
                    else {
                        m_notificationService.SetFailedResult(notification.Id, validationResult.ToString());
                    }
                    m_notificationService.SetSuccessResult(notification.Id);
                }
            }
            catch (CommunicationException ex) {
                m_notificationService.SetFailedResult(notification.Id, ex.ToString(), true);
            }
            catch (Exception ex)
            {
                m_notificationService.SetFailedResult(notification.Id, ex.ToString(), true);
            }
        }
    }
}
