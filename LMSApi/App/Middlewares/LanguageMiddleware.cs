using System.Globalization;

namespace LMSApi.App.Middlewares
{
    public class LanguageMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration configuration;

        public LanguageMiddleware(RequestDelegate next, IConfiguration configuration)
        {
            _next = next;
            this.configuration = configuration;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var languageHeader = context.Request.Headers.Where(x => x.Key == "Accept-Language" || x.Key == "Language")
                .FirstOrDefault()
                .Value
                .FirstOrDefault();

            if (!string.IsNullOrEmpty(languageHeader))
            {
                Thread.CurrentThread.CurrentCulture = new CultureInfo(languageHeader);
                context.Items["Language"] = languageHeader;
            }
            else
            {
                var defaultLanguage = configuration["DefaultLanguage"];
                Thread.CurrentThread.CurrentCulture = new CultureInfo(defaultLanguage ?? "ar");
            }

            await _next(context);
        }
    }
}
