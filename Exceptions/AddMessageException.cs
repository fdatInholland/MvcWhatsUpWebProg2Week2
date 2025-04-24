namespace MvcWhatsUp.Exceptions
{
    public class AddMessageException : Exception
    {
        public AddMessageException() : base("Message not added.") { }

        public AddMessageException(string message) : base(message) { }

        public AddMessageException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
