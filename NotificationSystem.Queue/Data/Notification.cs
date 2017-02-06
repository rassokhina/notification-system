using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NotificationSystem.Queue.Enums;

namespace NotificationSystem.Queue.Data {
    public class Notification {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        [StringLength(128)]
        [Required]
        public string To { get; set; }

        [StringLength(78)]
        [Required]
        public string Subject { get; set; }

        public string Body { get; set; }

        public string From { get; set; }

        public DateTimeOffset QueueDateTime { get; set; }

        public DateTimeOffset SendingDateTime { get; set; }

        public NotificationStatus Status { get; set; }

    }
}