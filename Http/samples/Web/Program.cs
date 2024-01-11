using System;
using AppCoreNet.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services
       .AddAuthentication(
           options =>
           {
               options.DefaultScheme = "cookie";
               options.DefaultChallengeScheme = "oidc";
           })
       .AddCookie(
           "cookie",
           options =>
           {
               options.Cookie.Name = "mvccode";

               options.Events.OnSigningOut = async e =>
               {
                   //await e.HttpContext.RevokeUserRefreshTokenAsync();
               };
           })
       .AddOpenIdConnect(
           "oidc",
           options =>
           {
               options.Authority = "https://demo.duendesoftware.com";

               options.ClientId = "interactive.confidential.short";
               options.ClientSecret = "secret";

               // code flow + PKCE (PKCE is turned on by default)
               options.ResponseType = "code";
               options.UsePkce = true;

               options.Scope.Clear();
               options.Scope.Add("openid");
               options.Scope.Add("profile");
               options.Scope.Add("email");
               options.Scope.Add("offline_access");
               options.Scope.Add("api");

               // not mapped by default
               options.ClaimActions.MapJsonKey("website", "website");

               // keeps id_token smaller
               options.GetClaimsFromUserInfoEndpoint = true;
               options.SaveTokens = true;

               options.TokenValidationParameters = new TokenValidationParameters
               {
                   NameClaimType = "name",
                   RoleClaimType = "role"
               };
           });

// add OAuth HTTP client authentication for OpenID connect scheme
builder.Services
       .AddHttpClientAuthentication()
       .AddOAuthClientForScheme(
           c => c.OpenIdConnect(
               o =>
               {
                   o.Scope = "api";
               }))
       .AddOpenIdConnect();

// add HTTP client with OAuth client authentication
builder.Services
       .AddHttpClient(
           "api-client",
           client =>
           {
               client.BaseAddress = new Uri("https://demo.duendesoftware.com/api/");
           })
       .AddOAuthClientAuthentication();

// add HTTP client with OAuth user authentication
builder.Services
       .AddHttpClient(
           "api-user-client",
           client =>
           {
               client.BaseAddress = new Uri("https://demo.duendesoftware.com/api/");
           })
       .AddOpenIdConnectAuthentication();

WebApplication app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();