namespace MvcMovie.Middlewares;
public class AddCabecalhoMiddleware
{
    private readonly RequestDelegate _next;
    private readonly string _personalToken;
    public AddCabecalhoMiddleware(RequestDelegate next, string personalToken)
    {
        _next = next;
        _personalToken = personalToken;
    }

    public async Task Invoke(HttpContext context, string PersonalToken)
    {
        context.Response.Headers.Add("personal-token", PersonalToken);

        // string headers = context.Response.Headers.ToString();
        
        await _next(context);
        // await context.Response.WriteAsync(descricao.ToString() + context.Response.Headers.ToString() ); //+ headers
    }
}