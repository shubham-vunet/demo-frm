using Serilog;
using Serilog.Enrichers.Span;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) => configuration.ReadFrom.Configuration(context.Configuration).Enrich.WithSpan());
builder.Logging.ClearProviders();
builder.Logging.AddSerilog();

builder.Services.AddControllers();

var app = builder.Build();
// app.UseSerilogRequestLogging();

app.MapControllers();

app.Run();
