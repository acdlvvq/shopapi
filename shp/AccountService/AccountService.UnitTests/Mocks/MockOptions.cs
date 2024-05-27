using AccountService.Core.Interfaces;
using Moq;
using System.Text;

namespace AccountService.UnitTests.Mocks;

public static class MockOptions
{
    public static Mock<IJwtOptions> GetJwtOptions()
    {
        var key = "phreshboyswagaboba5phreshboyswagaboba5phreshboyswagaboba5phreshboyswagaboba5";
        var issuer = "aboba";
        var audience = "1234567890";
        var expires = 5;
        var refreshExpires = 20;

        var options = new Mock<IJwtOptions>();

        options
            .Setup(o => o.Key)
            .Returns(new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)));

        options
            .Setup(o => o.Issuer)
            .Returns(issuer);

        options
            .Setup(o => o.Audience)
            .Returns(audience);

        options
            .Setup(o => o.ExpiresIn)
            .Returns(expires);

        options
            .Setup(o => o.NewRefreshTokenExpirationTime)
            .Returns(DateTime.UtcNow.AddMinutes(refreshExpires));

        return options;
    }
}
