namespace RazorPage.Middleware
{
    public class JwtTokenMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<JwtTokenMiddleware> _logger;

        public JwtTokenMiddleware(RequestDelegate next, ILogger<JwtTokenMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Check if there's a JWT token in session
            var jwtToken = context.Session.GetString("jwt_token");

            if (!string.IsNullOrWhiteSpace(jwtToken))
            {
                // Add the token to the Authorization header if it's not null
                context.Request.Headers["Authorization"] = $"Bearer {jwtToken}";
            }

            // Continue the pipeline
            await _next(context);
        }
    }

}
