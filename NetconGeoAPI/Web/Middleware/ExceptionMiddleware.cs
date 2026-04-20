namespace NetconGeoAPI.Web.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                context.Response.ContentType = "application/json";
                context.Response.StatusCode = 500;

                var response = new
                {
                    code = 500,
                    reason = "Internal Server Error",
                    message = "general fail",//ex.Message,
                    status = "Internal Server Error",
                    timestamp = DateTime.UtcNow,
                    exception = ex.Message
                };

                await context.Response.WriteAsJsonAsync(response);
            }
        }
    }
}
