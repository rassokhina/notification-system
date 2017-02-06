using System.Data.Entity;
using NotificationSystem.Queue.Data;

namespace NotificationSystem.Queue {
    public class NotificationContext : DbContext {
        protected readonly string m_notificationTable = "Notification";
        protected readonly string m_notificationResultTable = "NotificationResult";

        public NotificationContext()
            : this("name=NotificationContext") {
        }

        public NotificationContext(string nameOrConnectionString)
            : base(nameOrConnectionString) {
        }

        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<NotificationResult> NotificationResults { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder) {
            modelBuilder
                .Entity<Notification>()
                .ToTable(m_notificationTable);

            modelBuilder
                .Entity<NotificationResult>()
                .ToTable(m_notificationResultTable);
        }
    }
}