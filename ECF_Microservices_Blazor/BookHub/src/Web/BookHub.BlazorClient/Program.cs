using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using BookHub.BlazorClient;
using BookHub.BlazorClient.Services;
using Blazored.LocalStorage;
using System.Net.Http;


var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiBaseUrl = builder.Configuration["ApiBaseUrl"] ?? "http://localhost:5265";

builder.Services.AddBlazoredLocalStorage();

// HttpClient injecté pour toutes les requêtes vers le Gateway
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(apiBaseUrl) });

builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<AuthStateProvider>();

builder.Services.AddScoped<IBookService, BookService>();

builder.Services.AddScoped<ILoanService, LoanService>();


await builder.Build().RunAsync();
