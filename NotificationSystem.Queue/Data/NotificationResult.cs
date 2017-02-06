using System;
using System.ComponentModel.DataAnnotations;
using NotificationSystem.Queue.Enums;

namespace NotificationSystem.Queue.Data {
    public class NotificationResult {
        [Key]
        public Guid Id { get; set; }

        public NotificationSendingStatus Status { get; set; }

        public DateTimeOffset SendingDateTime { get; set; }

        public string Description { get; set; }

        public Guid NotificationId { get; set; }

        public virtual Notification Notification { get; set; }
    }
}