using System;
using System.Collections.Generic;
using System.Linq;
using Common.Logging;
using NotificationSystem.Queue.Data;
using NotificationSystem.Queue.Enums;

namespace NotificationSystem.Queue {
    public class NotificationService : INotificationService {

        private static readonly ILog m_log = LogManager.GetLogger<NotificationService>();
        private readonly NotificationContext m_notificationContext;

        public NotificationService(NotificationContext notificationContext) {
            if (notificationContext == null) {
                throw new ArgumentNullException("notificationContext");
            }
            m_notificationContext = notificationContext;
        }

        public IReadOnlyCollection<Notification> DequeueNotifiactions(int size) {
            Notification[] result = m_notificationContext.Notifications
                .Where(x => x.Status != NotificationStatus.Sended
                    && x.Status != NotificationStatus.Error
                    && x.SendingDateTime <= DateTimeOffset.UtcNow)
                .OrderBy(x => x.SendingDateTime)
                .Take(size)
                .ToArray();
            return result;
        }

        public void SetSuccessResult(Guid notificationId) {
            var notificationResult = new NotificationResult {
                Id = Guid.NewGuid(),
                NotificationId = notificationId,
                SendingDateTime = DateTimeOffset.UtcNow,
                Status = NotificationSendingStatus.Success
            };
            m_notificationContext.NotificationResults.Add(notificationResult);
            var notification = m_notificationContext.Notifications.Find(notificationId);
            notification.Status = NotificationStatus.Sended;
            m_notificationContext.SaveChanges();
        }

        public void SetFailedResult(Guid id, string details, bool error = false) {
            try {
                Notification failed = m_notificationContext.Notifications.Find(id);
                if (error) {
                    failed.SendingDateTime = DateTimeOffset.UtcNow.AddMinutes(5);
                    failed.Status = NotificationStatus.Error;
                }
                else {
                    failed.Status = NotificationStatus.Error;
                }                
                NotificationResult notificationResult = new NotificationResult {
                    Id = Guid.NewGuid(),
                    NotificationId = id,
                    SendingDateTime = DateTimeOffset.UtcNow,
                    Description = details,
                    Status = NotificationSendingStatus.Failed
                };
                m_notificationContext.NotificationResults.Add(notificationResult);
                m_notificationContext.SaveChanges();
            }
            catch (Exception ex) {
                m_log.Error(m => m(Resources.Exception_UnexpectedError, ex.ToString()));
            }
        }

        public Guid EnqueueNotification(string to){
            Notification notification = new Notification {
                To = to,
                QueueDateTime = DateTimeOffset.UtcNow,
                SendingDateTime = DateTimeOffset.UtcNow
            };
            m_notificationContext.Notifications.Add(notification);
            m_notificationContext.SaveChanges();
            return notification.Id;
        }
     
    }
}