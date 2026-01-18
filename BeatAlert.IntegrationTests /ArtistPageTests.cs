using System;
using System.Net;
using System.Threading.Tasks;
using BeatAlert.IntegrationTests.Infrastructure;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace BeatAlert.IntegrationTests;

public class ArtistPageTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly CustomWebApplicationFactory _factory;

    public ArtistPageTests(CustomWebApplicationFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task Get_ArtistPage_WithAuth_Debug()
    {
        var client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });

        var response = await client.GetAsync("/Artist/1");
        var html = await response.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        // If you're still in the generic error page, dump the HTML
        if (html.Contains("Could not load artist details.", StringComparison.OrdinalIgnoreCase) ||
            html.Contains("Something went wrong", StringComparison.OrdinalIgnoreCase))
        {
            throw new Exception(html);
        }

        // Only assert the name after the page is clearly not the error state
        Assert.Contains("Test Artist", html);
    }
}