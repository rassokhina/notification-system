using System;
using System.Collections.Generic;
using NotificationSystem.Queue.Data;

namespace NotificationSystem.Queue {
    public interface INotificationService {
        IReadOnlyCollection<Notification> DequeueNotifiactions(int size);
        Guid EnqueueNotification(string to);

        void SetSuccessResult(Guid notificationId);
        void SetFailedResult(Guid id, string details, bool error = false);
    }
}