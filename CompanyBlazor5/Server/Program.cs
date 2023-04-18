using CompanyBlazor.Shared.Models;
using CompanyBlazor5.Server;
using CompanyBlazor5.Server.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Net.Http.Headers;
using System.Security.Claims;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// DB
builder.Services.AddDbContext<CompanyBlazorDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);

builder.Services.AddSession();


builder.Services.AddResponseCompression(options =>
{
    options.Providers.Add<BrotliCompressionProvider>();
    options.EnableForHttps = true;
});
builder.Services.Configure<BrotliCompressionProviderOptions>(options =>
{
    options.Level = System.IO.Compression.CompressionLevel.Optimal;
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = "JWT_OR_COOKIE";
    options.DefaultChallengeScheme = "JWT_OR_COOKIE";
    options.DefaultSignInScheme = "JWT_OR_COOKIE";
})
.AddCookie(options =>
{
    options.LoginPath = "/signin-google";
    options.ExpireTimeSpan = TimeSpan.FromDays(1);
})
.AddGoogle(options =>
 {
     options.ClientId= builder.Configuration["Authentication:Google:ClientId"];
     options.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"];
     options.Scope.Add("profile");
     options.Events.OnCreatingTicket= context =>
     {
         string picuri = context.User.GetProperty("picture").GetString();
         context.Identity.AddClaim(new Claim("picture", picuri));
         return Task.CompletedTask;
     };
 })
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Issuer"],
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new
                    SymmetricSecurityKey
                    (Encoding.UTF8.GetBytes
                    (builder.Configuration["Jwt:Key"]))
    };
})
.AddPolicyScheme("JWT_OR_COOKIE", "JWT_OR_COOKIE", options =>
{
    options.ForwardDefaultSelector = context =>
    {
        string authorization = context.Request.Headers[HeaderNames.Authorization];
        if (!string.IsNullOrEmpty(authorization) && authorization.StartsWith("Bearer "))
            return JwtBearerDefaults.AuthenticationScheme;

        return CookieAuthenticationDefaults.AuthenticationScheme;
    };
});


builder.Services.AddCors(policy =>
{
    policy.AddPolicy("_myAllowSpecificOrigins", builder =>
     builder.WithOrigins("http://localhost:7094", "https://localhost:7094", "https://accounts.google.com")
      .SetIsOriginAllowed((host) => true) // this for using localhost address
      .AllowAnyMethod()
      .AllowAnyHeader()
      .AllowCredentials());
});


var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseSession();
app.Use(async (context, next) =>
{
    var path = context.Request.Path;
    if (path.Value.Contains("/swagger/", StringComparison.OrdinalIgnoreCase))
    {
        if (!context.User.Identity.IsAuthenticated)
        {
            context.Response.Redirect("/account");
            return;
        }
    }

    await next();
});

app.UseCors("_myAllowSpecificOrigins");

app.UseMiddleware<ErrorHandlingMiddleware>();
app.UseDeveloperExceptionPage();
app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
