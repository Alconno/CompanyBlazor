global using Microsoft.AspNetCore.Components.Authorization;
using CompanyBlazor.Shared.Models;
using CompanyBlazor5.Client;
using CompanyBlazor5.Client.Handlers;
using CompanyBlazor5.Client.Services;
using CompanyBlazor5.Client.Services.DepartmentServices;
using CompanyBlazor5.Client.Services.EmployeeServices;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Radzen;
using static System.Net.Mime.MediaTypeNames;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddSingleton<ILocalStorageService, LocalStorageService>();

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthenticationStateProvider, JwtAuthenticationStateProvider>();

// DI
builder.Services.AddScoped<IDepartmentService, DepartmentService>()
                .AddScoped<IEmployeeService, EmployeeService>()
                .AddScoped<ILocalStorageService, LocalStorageService>();

// Radzen
builder.Services.AddScoped<DialogService>()
                .AddScoped<NotificationService>()
                .AddScoped<TooltipService>()
                .AddScoped<ContextMenuService>();


await builder.Build().RunAsync();

