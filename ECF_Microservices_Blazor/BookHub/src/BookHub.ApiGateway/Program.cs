using BookHub.Shared.DTOs;
using BookHubGateway.Controllers;
using BookHubGateway.Infrastructure.HttpClients;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Ajouter les services pour les controllers avec JSON options
builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
        options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
    });


// Swagger / OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new() { Title = "BookHub Gateway API", Version = "v1" });
});

// CORS : autoriser toutes les origines pour tests
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// HttpClientFactory (optionnel, utile si tu veux injecter HttpClient dans tes RestClients)
builder.Services.AddHttpClient();

var app = builder.Build();

// Middleware Swagger en développement
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "BookHub Gateway API V1");
    });
}

// Middleware HTTPS
app.UseHttpsRedirection();

// Middleware CORS
app.UseCors();

// Middleware Authorization (si tu ajoutes Auth plus tard)
app.UseAuthorization();

// Mapping des controllers
app.MapControllers();

app.Run();
