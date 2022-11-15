

namespace ApiServerForTelegram.Entities.EExceptions
{
    public class InfoException : Exception
    {
        public InfoException() { }
        public InfoException(string message) : base(message) { }
        public InfoException(string message, Exception innerException) : base(message, innerException) { }
        public InfoException(string pathOfClass, string pathOfMethod, string typeOfException, string message)
            : this(message)
        {
            PathOfClass = pathOfClass;
            PathOfMethod = pathOfMethod;
            TypeOfException = typeOfException;
            _ = $"{pathOfClass}.{pathOfMethod}.{typeOfException}: {message}";
        }
        public InfoException(string pathOfClass, string typeOfException, string message)
            : this(message)
        {
            PathOfClass = pathOfClass;
            TypeOfException = typeOfException;
            _ = $"{pathOfClass}.{typeOfException}: {message}";
        }
        public InfoException(string pathOfClass, string pathOfMethod, string typeOfException, string pathOfVariable,
            ExceptionType type, string? message = null) : this(message ?? string.Empty)
        {
            PathOfClass = pathOfClass;
            PathOfMethod = pathOfMethod;
            TypeOfException = typeOfException;
            PathOfVariable = pathOfVariable;
            Type = type;

            _ = type switch
            {
                ExceptionType.Null => "can'n be null",
                ExceptionType.NullOrEmpty => "can'n be null or empty",
                _ => "unknown type of exception"
            };
            _ = $"{pathOfClass}.{pathOfMethod}.{typeOfException}: {pathOfVariable} {message}";
        }
        public InfoException(string pathOfClass, string typeOfException, string pathOfVariable,
            ExceptionType type, string? message = null) : this(message ?? string.Empty)
        {
            PathOfClass = pathOfClass;
            TypeOfException = typeOfException;
            PathOfVariable = pathOfVariable;
            Type = type;

            _ = type switch
            {
                ExceptionType.Null => "can'n be null",
                ExceptionType.NullOrEmpty => "can'n be null or empty",
                _ => "unknown type of exception"
            };
            _ = $"{pathOfClass}.{typeOfException}: {pathOfVariable} {message}";
        }

        public string? PathOfClass { get; set; }
        public string? PathOfMethod { get; set; }
        public string? TypeOfException { get; set; }
        public string? PathOfVariable { get; set; }
        public ExceptionType? Type { get; set; }
    }
}
