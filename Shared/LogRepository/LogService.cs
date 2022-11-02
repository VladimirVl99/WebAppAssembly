using MySql.Data.MySqlClient;
using WebAppAssembly.Shared.Entities.Exceptions;

namespace WebAppAssembly.Shared.LogRepository
{
    public class LogService
    {
        /// <summary>
        /// Format exception names for logs
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string FormatExceptionActionContent(object ex)
        {
            if (ex is HttpProcessException)
            {
                var e = (HttpProcessException)ex;
                return $"{nameof(HttpProcessException)}:\n\t{e.Source},\n\t{e.StatusCode} - {e.Message}";
            }
            else if (ex is MySqlException)
            {
                var e = (MySqlException)ex;
                return $"{nameof(MySqlException)}:\n\t{e.Source},\n\t{e.Number} - {e.Message}";
            }
            else if (ex is HttpRequestException)
            {
                var e = (HttpRequestException)ex;
                return $"{nameof(HttpRequestException)}:\n\t{e.Source},\n\t{e.StatusCode} - {e.Message}";
            }
            else if (ex is InvalidOperationException)
            {
                var e = (InvalidOperationException)ex;
                return $"{nameof(InvalidOperationException)}:\n\t{e.Source},\n\t{e.Message}";
            }
            else if (ex is Exception)
            {
                var e = (Exception)ex;
                return $"{nameof(Exception)}:\n\t{e.Source},\n\t{e.Message}";
            }
            return string.Empty;
        }
    }
}
