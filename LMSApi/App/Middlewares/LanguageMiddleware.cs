namespace LMSApi.App.Middlewares
{
    public class LanguageMiddleware
    {
        private readonly RequestDelegate _next;

        public LanguageMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var languageHeader = context.Request.Headers["Language"].FirstOrDefault();

            if (!string.IsNullOrEmpty(languageHeader))
            {
                context.Items["Language"] = languageHeader;
            }

            await _next(context);
        }
    }
}
