using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using RazorERP.Core.Application;
using RazorERP.Infrastructure.Configurations;
using RazorERP.Infrastructure.Middlewares.Global;
using RazorERP.Infrastructure.Persistence;
using System.Text;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var tokenPolicy = "token";
var rateLimitOptions = new RateLimitOptions();
builder.Configuration.GetSection(RateLimitOptions.RateLimit).Bind(rateLimitOptions);


var services = builder.Services;
services
    .AddApplication()
    .AddInfrastructurePersistence()
    .AddInfrastructureServices()
    .AddRateLimiter(_ => _
        .AddTokenBucketLimiter(policyName: tokenPolicy, options =>
        {
            options.TokenLimit = rateLimitOptions.TokenLimit;
            options.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
            options.QueueLimit = rateLimitOptions.QueueLimit;
            options.ReplenishmentPeriod = TimeSpan.FromSeconds(rateLimitOptions.ReplenishmentPeriod);
            options.TokensPerPeriod = rateLimitOptions.TokensPerPeriod;
            options.AutoReplenishment = rateLimitOptions.AutoReplenishment;
        }));


services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.Jwt));
services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    var jwtOptions = builder.Configuration.GetSection(JwtOptions.Jwt).Get<JwtOptions>();
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtOptions.Issuer,
        ValidAudience = jwtOptions.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtOptions.Key))
    };
});

services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireRole("Admin"));
    options.AddPolicy("User", policy => policy.RequireRole("User"));
});


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "RazorERP API", Version = "v1" });

    // Add JWT authentication to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                },
                Scheme = "oauth2",
                Name = "Bearer",
                In = ParameterLocation.Header,
            },
            new List<string>()
        }
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "RazorERP API v1");
    });
}

app.UseMiddleware<GlobalExceptionHandlingMiddleware>();

app.UseCors();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseRateLimiter();

app.MapControllers();

app.Run();
