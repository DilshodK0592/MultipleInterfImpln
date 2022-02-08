var builder = WebApplication.CreateBuilder();
builder.Services.AddTransient<IHelloService, RuHelloService>();
builder.Services.AddTransient<IHelloService, EnHelloService>();

var app = builder.Build();
app.UseMiddleware<HelloMiddleware>();
app.Run();


interface IHelloService
{
    string Message { get; }
}

class RuHelloService: IHelloService
{
    public string Message => "Привет Метанит.Ком";
}

class EnHelloService: IHelloService
{
    public string Message => "Hello METANIT.COM";
}

class HelloMiddleware
{
    readonly IEnumerable<IHelloService> helloServices;

    public HelloMiddleware(RequestDelegate _, IEnumerable<IHelloService> helloServices)
    {
        this.helloServices = helloServices;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var service1 = helloServices.FirstOrDefault().Message;
        var service2 = helloServices.Last().Message;
        context.Response.ContentType = "text/html; charset=utf-8";
        string responseText = "";
        foreach (var service in helloServices)
        {
            responseText += $"<h3>{service.Message}</h3>";
        }
        await context.Response.WriteAsync(responseText);
        await context.Response.WriteAsync($"<h3>{service1}</h3>");
        await context.Response.WriteAsync(service2);
    }
}