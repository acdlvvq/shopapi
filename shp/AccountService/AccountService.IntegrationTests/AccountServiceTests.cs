using AccountService.Core.Models;
using AccountService.Presentation.Contracts;
using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace AccountService.IntegrationTests;

public class AccountServiceTests
{
    [Fact]
    public async Task LoginRequest_ReturnsTokens()
    {
        var app = new AccountServiceWebApplicationFactory();
        var registrationRequest = new RegistrationRequest(
            "aboba123@gmail.com", "aboba", "qwerty");
        var loginRequest = new LogInRequest(
            "aboba123@gmail.com", "qwerty");

        var client = app.CreateClient();

        var registrationResponse = await client.PostAsJsonAsync("/api/registration", registrationRequest);
        registrationResponse.EnsureSuccessStatusCode();

        var loginResponse = await client.PostAsJsonAsync("/api/login", loginRequest);
        loginResponse.EnsureSuccessStatusCode();
    }
}