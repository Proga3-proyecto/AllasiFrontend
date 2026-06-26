using AllasiFrontend.Components;
using Progra3_Frontend.Services;
using Progra3_Frontend.Services.Auth;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddHttpClient();
builder.Services.AddScoped<ProductosRS>();
builder.Services.AddScoped<MarcasRS>();
builder.Services.AddScoped<CategoriasRS>();
builder.Services.AddScoped<ImpuestosRS>();
builder.Services.AddScoped<AlcoholImpuestoRS>();
builder.Services.AddScoped<ImagenesRS>();
builder.Services.AddScoped<ClientesRS>();
builder.Services.AddScoped<AdminsRS>();
builder.Services.AddScoped<RecetasRS>();
builder.Services.AddScoped<PedidosRS>();
builder.Services.AddScoped<CarritoStateService>();

builder.Services.AddHttpClient<IAuthService, AuthService>(client =>
{
    client.BaseAddress = new Uri("http://localhost:8080/Servicios-1.0-SNAPSHOT/api/");
});
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddScoped<CustomAuthenticationStateProvider>();
builder.Services.AddScoped<AuthenticationStateProvider>(sp => sp.GetRequiredService<CustomAuthenticationStateProvider>());

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
