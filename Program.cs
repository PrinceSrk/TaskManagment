using TaskManagment.BackGroundService;
using TaskManagment.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddProjectLogging();

builder.Services.AddAuthenticationExtension()
                .AddAuthorization()
                .AddEndpointsApiExplorer()
                .AddControllerConfig()
                .AddAutoMapper()
                .AddSwaggerExtensions()
                .AddHostedService<DailyEmailService>()
                .AddDatabaseConfig(builder.Configuration)
                .AddScopedBusinnessService()
                .AddScopedRepository()
                .AddProjectHealthCheck();

var app = builder.Build();
app.ConfigureEnvironmennt();
app.Run();

