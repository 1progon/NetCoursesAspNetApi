using System.Text.Json.Serialization;
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


app.UseCors(b => b.WithOrigins("http://localhost:4200"));


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
else if (app.Environment.IsProduction())
{
}


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();