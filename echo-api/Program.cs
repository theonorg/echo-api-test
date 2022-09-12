using System.Net;
using echo_api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Console;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Logging.AddSimpleConsole(opts =>
    {
        opts.IncludeScopes = false;
        opts.TimestampFormat = "[yyyy-MM-dd HH:mm:ss] ";
        opts.ColorBehavior = LoggerColorBehavior.Disabled;
    });

builder.Services.AddCors(options =>
    {
        options.AddPolicy("CorsPolicy",
            builder => builder
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader());
    });

builder.Services.AddDbContext<EchoHistoryDb>(opt => opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.UseCors("CorsPolicy");

using (var context = app.Services.CreateScope().ServiceProvider.GetRequiredService<EchoHistoryDb>())
{
    if (context.Database.IsRelational())
    {
        context.Database.Migrate();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.MapGet("/echo/{message}", (string message) =>
// {
//     string logMessage = string.Format("Echoing message: {0}", message);
//     app.Logger.LogInformation(logMessage);
//     return Results.Ok(message);
// })
// .WithName("Echo");


app.MapGet("/echo/{message}", (string message, EchoHistoryDb db) =>
{
    string logMessage = string.Format("Echoing message: {0}", message);
    app.Logger.LogInformation(logMessage);
    db.EchoLogs.Add(new EchoHistory { Message = logMessage });
    db.SaveChanges();
    return Results.Ok(message);
})
.WithName("Echo");

app.MapGet("/log", (EchoHistoryDb db) =>
{
    return db.EchoLogs.ToList().OrderBy(x => x.EchoHistoryId);
})
.WithName("GetLog");

app.MapDelete("/log", (EchoHistoryDb db) =>
{
    int count = db.EchoLogs.Count();
    db.EchoLogs.RemoveRange(db.EchoLogs);
    return Results.Ok(count);
})
.WithName("ClearLog");

app.Run();