using MvcMovie.Auth;

namespace MvcMovie.Middlewares;

public class JwtTokenMiddleware
{
    private readonly IAuthService _authService;

    public JwtTokenMiddleware(IAuthService authService)
    {
        _authService = authService;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        string token = (string)context.Items["PersonalToken"];
        Console.WriteLine($"{token}");
        

        if (token != null)
        {
            var cookieOptions = new CookieOptions
            {
                Expires = DateTime.UtcNow.AddMinutes(30),
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict
            };

            context.Response.Cookies.Append("JwtToken", token, cookieOptions);
        }

        await next(context);
    }
}