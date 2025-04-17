using Anuncia_Picos.Components;
using System.Net.Http;

var builder = WebApplication.CreateBuilder(args);

// Registra o HttpClient
builder.Services.AddHttpClient();

// Adiciona os componentes do Blazor
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

var app = builder.Build();

// Configura o pipeline de requisição HTTP
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts(); // Política de segurança para HSTS (HTTP Strict Transport Security)
}

app.UseHttpsRedirection();  // Redireciona para HTTPS
app.UseStaticFiles();       // Para servir arquivos estáticos como JS, CSS, imagens

app.UseRouting();

app.UseAntiforgery(); // Adiciona o middleware antiforgery

// Mapeia os componentes Razor para as rotas
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();