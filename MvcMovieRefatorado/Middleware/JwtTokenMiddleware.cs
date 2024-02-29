// using MvcMovie.Auth;

// namespace MvcMovie.Middlewares;

// public class JwtTokenMiddleware
// {
//     private readonly IAuthService _authService;
//     private readonly RequestDelegate _next;

//     public JwtTokenMiddleware(IAuthService authService, RequestDelegate next)
//     {
//         _next = next;
//         _authService = authService;
//     }

//     public async Task InvokeAsync(HttpContext context, RequestDelegate next)
//     {
//         string token = (string)context.Items["PersonalToken"];
//         Console.WriteLine($"{token}");
    
//         if (token != null)
//         {
//             var cookieOptions = new CookieOptions
//             {
//                 Expires = DateTime.UtcNow.AddMinutes(30),
//                 HttpOnly = true,
//                 Secure = true,
//                 SameSite = SameSiteMode.Strict
//             };

//             context.Response.Cookies.Append("JwtToken", token, cookieOptions);
//         }

//         await next(context);
//     }
// }

using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
namespace MvcMovie.Middlewares;

public class JwtTokenMiddleware
{
    private readonly RequestDelegate _next;

    public JwtTokenMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext context)
{
    if (context.Request.Cookies.ContainsKey("JwtToken"))
    {
        string token = context.Request.Cookies["JwtToken"];

        // context.Response.Headers.Add("ip", "teste-ip")

        context.Request.Headers["Authentication"] = "Bearer " + token;
    }

    await _next(context);
}
}
