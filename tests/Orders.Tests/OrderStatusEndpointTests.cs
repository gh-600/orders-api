using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc.Testing;

namespace Orders.Tests;

public class OrderStatusEndpointTests(WebApplicationFactory<Program> factory)
    : IClassFixture<WebApplicationFactory<Program>>
{
    private const string ExistingOrderId = "11111111-1111-1111-1111-111111111111";

    [Fact]
    public async Task Existing_order_returns_id_and_text_status()
    {
        using var client = factory.CreateClient();

        var response = await client.GetAsync($"/orders/{ExistingOrderId}/status");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        using var body = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.Equal(ExistingOrderId, body.RootElement.GetProperty("id").GetString());
        Assert.Equal("Created", body.RootElement.GetProperty("status").GetString());
    }

    [Fact]
    public async Task Malformed_id_returns_bad_request()
    {
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/orders/not-a-guid/status");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Unknown_id_returns_not_found()
    {
        using var client = factory.CreateClient();

        var response = await client.GetAsync("/orders/22222222-2222-2222-2222-222222222222/status");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task Repeated_queries_return_the_same_status()
    {
        using var client = factory.CreateClient();

        var firstResponse = await client.GetAsync($"/orders/{ExistingOrderId}/status");
        var secondResponse = await client.GetAsync($"/orders/{ExistingOrderId}/status");
        using var firstBody = JsonDocument.Parse(await firstResponse.Content.ReadAsStringAsync());
        using var secondBody = JsonDocument.Parse(await secondResponse.Content.ReadAsStringAsync());

        Assert.Equal("Created", firstBody.RootElement.GetProperty("status").GetString());
        Assert.Equal(
            firstBody.RootElement.GetProperty("status").GetString(),
            secondBody.RootElement.GetProperty("status").GetString());
    }
}
