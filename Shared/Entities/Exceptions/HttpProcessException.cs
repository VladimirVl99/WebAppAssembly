using System.Net;

namespace WebAppAssembly.Shared.Entities.Exceptions
{
    [Serializable]
    public class HttpProcessException : Exception
    {
        public HttpStatusCode StatusCode { get; private set; }

        public HttpProcessException() { }
        public HttpProcessException(string message) : base(message) { }
        public HttpProcessException(string message, Exception innerException) : base(message, innerException) { }
        public HttpProcessException(HttpStatusCode statusCode, string message)
            : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
