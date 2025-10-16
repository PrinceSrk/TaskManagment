using AutoMapper;
using TaskManagment.Extensions;

var builder = WebApplication.CreateBuilder(args);

builder.Logging.AddProjectLogging();

builder.Services.AddEndpointsApiExplorer()
                .AddSwaggerGen()
                .AddControllerConfig()
                .AddAutoMapper()
                .AddDatabaseConfig(builder.Configuration)
                .AddScopedBusinnessService()
                .AddScopedRepository()
                .AddProjectHealthCheck();

var app = builder.Build();
app.ConfigureEnvironmennt();
app.Run();

