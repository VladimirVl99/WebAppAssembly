namespace WebAppAssembly.Shared.Entities.Exceptions
{
    public class InfoException : Exception
    {
        public string? PathOfClass { get; set; }

        public string? PathOfMethod { get; set; }

        public string? TypeOfException { get; set; }

        public string? PathOfVariable { get; set; }

        public ExceptionType? Type { get; set; }


        public InfoException() { }

        public InfoException(string message) : base(message) { }

        public InfoException(string message, Exception innerException) : base(message, innerException) { }

        public InfoException(string pathOfClass, string pathOfMethod, string typeOfException, string message)
            : this($"{pathOfClass}.{pathOfMethod}.{typeOfException}: {message}")
        {
            PathOfClass = pathOfClass;
            PathOfMethod = pathOfMethod;
            TypeOfException = typeOfException;
        }

        public InfoException(string pathOfClass, string typeOfException, string message)
            : this($"{pathOfClass}.{typeOfException}: {message}")
        {
            PathOfClass = pathOfClass;
            TypeOfException = typeOfException;
        }

        public InfoException(string pathOfClass, string pathOfMethod, string typeOfException, string pathOfVariable,
            ExceptionType type, string? message = null) : this(MessageFormatter(pathOfClass, pathOfMethod,
                typeOfException, pathOfVariable, type, message))
        {
            PathOfClass = pathOfClass;
            PathOfMethod = pathOfMethod;
            TypeOfException = typeOfException;
            PathOfVariable = pathOfVariable;
            Type = type;
        }

        public InfoException(string pathOfClass, string typeOfException, string pathOfVariable,
            ExceptionType type, string? message = null) : this(MessageFormatter(pathOfClass, typeOfException,
                pathOfVariable, type, message))
        {
            PathOfClass = pathOfClass;
            TypeOfException = typeOfException;
            PathOfVariable = pathOfVariable;
            Type = type;
        }


        private static string MessageFormatter(string pathOfClass, string pathOfMethod, string typeOfException, string pathOfVariable,
            ExceptionType type, string? message)
        {
            var typeMessage = type switch
            {
                ExceptionType.Null => "can't be null",
                ExceptionType.NullOrEmpty => "can't be null or empty",
                _ => "unknown type of exception"
            };

            return $"{pathOfClass}.{pathOfMethod}.{typeOfException}: {pathOfVariable} {typeMessage}. {message}";
        }

        private static string MessageFormatter(string pathOfClass, string typeOfException, string pathOfVariable,
            ExceptionType type, string? message)
        {
            var typeMessage = type switch
            {
                ExceptionType.Null => "can't be null",
                ExceptionType.NullOrEmpty => "can't be null or empty",
                _ => "unknown type of exception"
            };

            return $"{pathOfClass}.{typeOfException}: {pathOfVariable} {typeMessage}. {message}";
        }
    }
}
