using System.Diagnostics;

namespace NetconGeoAPI.Web.Middleware
{
    public class LoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string _logPath = "logs/api.log";
        private static readonly object _lock = new();

        public LoggingMiddleware(RequestDelegate next)
        {
            _next = next;
            if (!Directory.Exists("logs")) Directory.CreateDirectory("logs");
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var sw = Stopwatch.StartNew();
            var requestPath = $"{context.Request.Method} {context.Request.Scheme}{context.Request.Host}{context.Request.QueryString}";

            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                LogToFile($"[ERROR] {requestPath} | Message: {ex.Message}");
                throw;
            }
            finally
            {
                sw.Stop();
                LogToFile($"[INFO] {requestPath} | Status: {context.Response.StatusCode} | Time: {sw.ElapsedMilliseconds}ms");
            }
        }

        private void LogToFile(string message)
        {
            lock (_lock)
            {
                var info = new FileInfo(_logPath);
                if (info.Exists && info.Length > 1024 * 1024)
                { // Rotação 1MB
                    File.Move(_logPath, $"logs/log_{DateTime.Now:yyyyMMddHHmmss}.log");
                }
                File.AppendAllText(_logPath, $"{DateTime.Now:G} | {message}{Environment.NewLine}");
            }
        }
    }
}
