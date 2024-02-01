namespace Forum_System.Exceptions
{
    public class DuplicateEntityException : ApplicationException
    {
        public DuplicateEntityException(string message)
            : base(message)
        {}
    }
}
