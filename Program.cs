using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.OpenApi.Models;
using TaskManagment.BackGroundService;
using TaskManagment.Extensions;
using TaskManagment.Middleware;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddProjectLogging();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

    // Add access token header
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Access token. Format: Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    // Add refresh token header
    c.AddSecurityDefinition("RefreshToken", new OpenApiSecurityScheme
    {
        Description = "Refresh token. Format: raw token string",
        Name = "X-Refresh-Token",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "RefreshToken"
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
                }
            },
            new List<string>()
        },
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "RefreshToken"
                }
            },
            new List<string>()
        }
    });
});

builder.Services.AddAuthentication("CustomToken")
    .AddScheme<AuthenticationSchemeOptions, DummyAuthenticationHandler>("CustomToken", options => { });

builder.Services.AddAuthorization();
builder.Services.AddHostedService<DailyEmailService>();
builder.Services.AddEndpointsApiExplorer()
                // .AddSwaggerGen()
                .AddControllerConfig()
                .AddAutoMapper()
                .AddDatabaseConfig(builder.Configuration)
                .AddScopedBusinnessService()
                .AddScopedRepository()
                .AddProjectHealthCheck();

var app = builder.Build();
app.ConfigureEnvironmennt();
app.Run();

