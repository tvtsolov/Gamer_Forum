namespace Forum_System.Exceptions
{
    public class InvalidCredentialsException : ApplicationException
    {
        public InvalidCredentialsException(string message)
            : base(message)
        { }
    }
}
