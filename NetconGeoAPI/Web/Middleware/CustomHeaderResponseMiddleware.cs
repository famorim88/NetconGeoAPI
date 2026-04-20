using System.Diagnostics;

namespace NetconGeoAPI.Web.Middleware
{
    public class CustomHeaderResponseMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomHeaderResponseMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var stopwatch = Stopwatch.StartNew();

            var requestId = Guid.NewGuid().ToString();

            context.Response.OnStarting(() =>
            {
                stopwatch.Stop();

                // Injeta o ID da Requisição
                context.Response.Headers.Append("X-Request-Id", requestId);

                // Injeta o Tempo de Resposta (formatado em milissegundos)
                context.Response.Headers.Append("X-Response-Time", $"{stopwatch.ElapsedMilliseconds}ms");

                return Task.CompletedTask;
            });

            await _next(context);
        }
    }
}
