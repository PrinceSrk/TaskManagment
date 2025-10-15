using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using TaskManagment.Extensions;
using TaskManagment.Interfaces;
using TaskManagment.Models;
using TaskManagment.Repository;
using TaskManagment.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
    options.JsonSerializerOptions.WriteIndented = true;
});
builder.Services.AddScoped<IAuthService,AuthService>();
builder.Services.AddScoped<IAuthRepo,AuthRepo>();
builder.Services.AddScoped<ITaskService,TaskService>();
builder.Services.AddScoped<ITaskRepo,TaskRepo>();

builder.Services.AddDbContext<TaskContext>(opt =>
{
    opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();
app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.Run();

