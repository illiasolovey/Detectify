using PupSearch.Filters;
using PupSearch.Models;
using PupSearch.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMemoryCache();
builder.Services.AddSingleton<AwsConfiguration>();
builder.Services.AddSingleton<CacheFilter>();
builder.Services.AddScoped<IStorageService, StorageService>();

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
