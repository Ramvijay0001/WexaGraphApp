using Neo4j.Driver;
using WexaGraph.Api.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle


var uri = builder.Configuration["COGNODB_URI"];
var username = builder.Configuration["COGNODB_USERNAME"];
var password = builder.Configuration["COGNODB_PASSWORD"];

if (string.IsNullOrWhiteSpace(uri))
    throw new InvalidOperationException("COGNODB_URI is not configured.");

if (string.IsNullOrWhiteSpace(username))
    throw new InvalidOperationException("COGNODB_USERNAME is not configured.");

if (string.IsNullOrWhiteSpace(password))
    throw new InvalidOperationException("COGNODB_PASSWORD is not configured.");

builder.Services.AddSingleton<IDriver>(_ =>
    GraphDatabase.Driver(
        uri,
        AuthTokens.Basic(username, password)));

builder.Services.AddSingleton<CognoDbService>();
builder.Services.AddSingleton<SeedService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("Angular", policy =>
    {
        policy
            .WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("Angular");

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
