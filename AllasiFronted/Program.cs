using AllasiFrontend.Components;
using Progra3_Frontend.Services;
using Progra3_Frontend.Services.Auth;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents(options =>
    {
        options.DetailedErrors = true;
    });


builder.Services.ConfigureHttpClientDefaults(httpBuilder =>
{
    httpBuilder.ConfigureHttpClient(client =>
    {
        client.BaseAddress = new Uri("http://localhost:8080/Servicios-1.0-SNAPSHOT/api/");
    });
});

builder.Services.AddHttpClient<ProductosRS>();
builder.Services.AddHttpClient<MarcasRS>();
builder.Services.AddHttpClient<CategoriasRS>();
builder.Services.AddHttpClient<ImpuestosRS>();
builder.Services.AddHttpClient<AlcoholImpuestoRS>();
builder.Services.AddHttpClient<ImagenesRS>();
builder.Services.AddHttpClient<ClientesRS>();
builder.Services.AddHttpClient<AdminsRS>();
builder.Services.AddHttpClient<RecetasRS>();
builder.Services.AddHttpClient<PedidosRS>();
builder.Services.AddHttpClient<AuthService>();


builder.Services.AddHttpClient<IAuthService, AuthService>();

builder.Services.AddScoped<CarritoStateService>();
builder.Services.AddScoped<ConfirmDialogService>();
builder.Services.AddScoped<ToastService>();


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
