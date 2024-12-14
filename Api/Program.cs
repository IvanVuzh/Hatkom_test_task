using Microsoft.EntityFrameworkCore;
using FluentValidation;

using Api.Database;
using Api.DTOs.Validation;
using Api.Middleware;
using Api.Controllers;
using FluentValidation.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ProjectContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("default")));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddValidatorsFromAssemblyContaining<CreateProjectDTOValidator>();
builder.Services.AddValidatorsFromAssemblyContaining<TimeLogDTOValidator>();

builder.Services.AddTransient<ProjectController>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseMiddleware<ValidationErrorFormatterMiddleware>();

app.MapControllers();

app.Run();
