using Microsoft.AspNetCore.DataProtection;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

// ✅ Leer el ambiente desde appsettings.json base
var ambiente = builder.Configuration["Ambiente"] ?? "Development";

// ✅ Cargar el appsettings correspondiente al ambiente
builder.Configuration
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile($"appsettings.{ambiente}.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

// ✅ Aplicar el ambiente al host
builder.Environment.EnvironmentName = ambiente;

builder.Services.AddControllersWithViews();

builder.Services.AddHttpContextAccessor();

// ✅ Persistir claves de Data Protection en disco (evita fallos de antiforgery token
// tras reinicios del App Pool, común en hosting compartido)
var carpetaKeys = Path.Combine(Directory.GetCurrentDirectory(), "DataProtection-Keys");
if (!Directory.Exists(carpetaKeys))
    Directory.CreateDirectory(carpetaKeys);

builder.Services.AddDataProtection()
    .PersistKeysToFileSystem(new DirectoryInfo(carpetaKeys))
    .SetApplicationName("NCOSystems");

// ✅ Sesión en memoria (reemplaza TempData por cookies)
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
    options.IdleTimeout = TimeSpan.FromMinutes(60);
});

builder.Services.Configure<Microsoft.AspNetCore.Builder.IISServerOptions>(options =>
{
    options.MaxRequestBodySize = 104857600; // 100 MB
});

var app = builder.Build();

// Carpeta Documento
var carpetaDocumento = Path.Combine(Directory.GetCurrentDirectory(), "Documento");
if (!Directory.Exists(carpetaDocumento))
    Directory.CreateDirectory(carpetaDocumento);

app.UseStaticFiles();
app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(carpetaDocumento),
    RequestPath = new PathString("/Documento")
});

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// ✅ HTTPS comentado para MonsterASP (lo maneja el servidor)
// app.UseHttpsRedirection();

app.UseRouting();

// ✅ Sesión debe ir antes de Authorization
app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Personal}/{action=Index}/{id?}");

app.Run();