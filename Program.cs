using System.Reflection;
using Microsoft.OpenApi.Models;
using PupSearch.Filters;
using PupSearch.Models;
using PupSearch.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<AwsConfiguration>();
builder.Services.AddSingleton<CacheFilter>();
builder.Services.AddScoped<IStorageService, StorageService>();
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
