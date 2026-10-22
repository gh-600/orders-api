using Orders.Application;
using Orders.Infrastructure;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IOrderStatusLookup, InMemoryOrderStatusLookup>();
builder.Services.AddScoped<GetOrderStatusQueryHandler>();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");
app.MapGet("/orders/{id}/status", (string id, GetOrderStatusQueryHandler queryHandler) =>
{
    if (!Guid.TryParse(id, out var orderId))
    {
        return Results.BadRequest();
    }

    var result = queryHandler.Handle(new GetOrderStatusQuery(orderId));
    return result is null
        ? Results.NotFound()
        : Results.Ok(new { id = result.Id, status = result.Status.ToString() });
});

app.Run();

public partial class Program;
