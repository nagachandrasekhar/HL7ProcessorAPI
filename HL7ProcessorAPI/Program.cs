using Application.Services;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Messaging;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Configure Swagger
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "HealthCareAPI",
        Version = "v1",
        Description = "API for HealthCare Application"
    });
});
// Register DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register Repository
builder.Services.AddScoped<IHL7MessageRepository, HL7MessageRepository>();

// Register RabbitMQ Publisher
builder.Services.AddSingleton<RabbitMQPublisher>(sp => new RabbitMQPublisher("localhost"));

// Register HL7 Processor Service
builder.Services.AddScoped<HL7ProcessorService>();

var app = builder.Build();
// Enable Swagger UI in development environment
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "HealthCareAPI v1");
    });
}
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();