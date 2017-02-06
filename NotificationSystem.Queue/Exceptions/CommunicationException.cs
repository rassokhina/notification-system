using System;

namespace NotificationSystem.Queue.Exceptions {
    [Serializable]
    public class CommunicationException : ApplicationException {
        public CommunicationException(string message) : base(message) {
        }

        public CommunicationException(string message, Exception inner)
            : base(message, inner) {
        }
    }
}