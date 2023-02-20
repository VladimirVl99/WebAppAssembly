using MySql.Data.MySqlClient;
using WebAppAssembly.Shared.Entities.Exceptions;

namespace WebAppAssembly.Shared.LogRepository
{
    /// <summary>
    /// Class for working with logs.
    /// </summary>
    public static class LogService
    {
        /// <summary>
        /// Format exception names for logs
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string FormatExceptionActionContent(object ex)
            => ex switch
            {
                HttpProcessException hpe => $"{nameof(HttpProcessException)}:\n\t{hpe.Source},\n\t{hpe.StatusCode} - {hpe.Message}",
                MySqlException mse => $"{nameof(MySqlException)}:\n\t{mse.Source},\n\t{mse.Number} - {mse.Message}",
                HttpRequestException hre => $"{nameof(HttpRequestException)}:\n\t{hre.Source},\n\t{hre.StatusCode} - {hre.Message}",
                InvalidOperationException ioe => $"{nameof(InvalidOperationException)}:\n\t{ioe.Source},\n\t{ioe.Message}",
                Exception e => $"{nameof(Exception)}:\n\t{e.Source},\n\t{e.Message}",
                _ => "Unknown exception."
            };
    }
}
