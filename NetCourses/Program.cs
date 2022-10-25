using System.Text.Json.Serialization;
using Microsoft.AspNetCore.HttpOverrides;
using NetCourses.Data;
using NetCourses.JsonConverters;

var builder = WebApplication.CreateBuilder(args);


// Get connection string from appsettings.json
var connectionString = builder.Configuration.GetConnectionString("AppDbContext");


// Db Context
builder.Services.AddNpgsql<AppDbContext>(connectionString);


// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(o =>
    {
        o.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
        o.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter());
        o.JsonSerializerOptions.Converters.Add(new TimeOnlyJsonConverter());
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();

app.UseForwardedHeaders(new ForwardedHeadersOptions
{
    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
});

app.UseCors(b =>
    b.WithOrigins("http://localhost:4200", "https://dotnetcase.com")
        .AllowAnyMethod()
        .AllowAnyHeader());


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