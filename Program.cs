using System.Reflection;
using Amazon.Runtime;
using AWS.Logger;
using AWS.Logger.SeriLog;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.OpenApi.Models;
using PupSearch.Filters;
using PupSearch.Models;
using PupSearch.Services;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<AwsConfiguration>();
builder.Services.AddScoped<IStorageService, StorageService>();
var cacheDurationSeconds = builder.Configuration.GetValue<double>("Caching:DurationSeconds");
var cacheDuration = TimeSpan.FromSeconds(cacheDurationSeconds);
builder.Services.AddScoped<CacheFilter>(provider => new CacheFilter(provider.GetRequiredService<IMemoryCache>(), cacheDuration));
builder.Services.AddSingleton<LoggingFilter>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(opts =>
{
    opts.SwaggerDoc("v1", new OpenApiInfo
    {
        Version = "v1",
        Title = "PupSearch API",
        Description = "ASP.NET Core 6 Web API backend for PupSearch Project.",
        Contact = new OpenApiContact
        {
            Name = "Github repository",
            Url = new Uri("https://github.com/illiasolovey/PupSearch"),
        },
        License = new OpenApiLicense
        {
            Name = "MIT License",
            Url = new Uri("https://github.com/illiasolovey/PupSearch/blob/main/LICENSE")
        }
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    opts.IncludeXmlComments(xmlPath);
});
var awsAccessKeyId = builder.Configuration["Aws:AccessKey"];
var awsSecretAccessKey = builder.Configuration["Aws:SecretAccessKey"];
var awsCredentials = new BasicAWSCredentials(awsAccessKeyId, awsSecretAccessKey);
var awsConfiguration = new AWSLoggerConfig
{
    LogGroup = builder.Configuration["Serilog:LogGroup"],
    Region = builder.Configuration["Aws:Region"],
    Credentials = awsCredentials
};
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.AWSSeriLog(awsConfiguration)
    .CreateLogger();
builder.Logging.AddSerilog(logger);

var corsConfig = builder.Configuration.GetSection("Cors");
string cors = corsConfig["PolicyName"] ?? throw new Exception("Cors policy name is not specified.");
string corsOrigin = corsConfig["Origin"] ?? throw new Exception("Cors policy origin is not specified.");

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: cors, policy =>
    {
        policy.WithOrigins(corsOrigin).AllowAnyHeader().AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(cors);
app.UseAuthorization();
app.MapControllers();
app.Run();
