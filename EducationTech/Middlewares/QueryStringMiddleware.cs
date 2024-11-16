namespace EducationTech.Middlewares;

public class QueryStringMiddleware
{
    private readonly RequestDelegate _next;

    public QueryStringMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var queryCollection = context.Request.Query.ToDictionary(
            kvp => kvp.Key.Replace("[]", ""),
            kvp => kvp.Value);

        context.Request.Query = new QueryCollection(queryCollection);
        await _next(context);
    }
}
