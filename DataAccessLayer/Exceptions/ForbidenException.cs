namespace DataAccessLayer.Exceptions
{
    public class ForbidenException: Exception
    {
        public ForbidenException(string message): base(message)
        {
        }

        public ForbidenException(string message, Exception innerException): base(message, innerException)
        {
        }

        public ForbidenException()
        {
        }
    }
}
