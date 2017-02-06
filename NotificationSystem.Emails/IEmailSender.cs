using System;

namespace NotificationSystem.Emails
{
    public interface IEmailSender : IDisposable
    {
        void Send(IEmailNotification message);
    }
}
