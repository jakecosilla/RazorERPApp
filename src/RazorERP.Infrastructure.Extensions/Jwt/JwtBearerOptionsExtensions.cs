using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using RazorERP.Infrastructure.Configurations;
using System.Text;

namespace RazorERP.Infrastructure.Extensions.Jwt
{
    public static class JwtBearerOptionsExtensions
    {
        public static void Configure(this IOptions<JwtOptions> jwtOptions, JwtBearerOptions options)
        {
            var jwtSettings = jwtOptions.Value;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                ValidateIssuerSigningKey = true,
                ValidIssuer = jwtSettings.Issuer,
                ValidAudience = jwtSettings.Audience,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSettings.Key))
            };
        }
    }
}
