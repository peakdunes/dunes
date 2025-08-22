using DUNES.UI.Filters;
using DUNES.UI.Infrastructure;
using DUNES.UI.Middleware;
using DUNES.UI.Services.Admin;
using DUNES.UI.Services.Inventory;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

var builder = WebApplication.CreateBuilder(args);

// Registro del filtro personalizado (usado en controladores)

//#####################
//FILTROS
//######################

//Filtro para mostrar los avisos informations
//builder.Services.AddScoped<TransferMiddlewareMessagesFilter>();

//filtro para cargar el menu de navegacion
builder.Services.AddScoped<BreadcrumbFilter>();

// MVC con filtros globales
builder.Services.AddControllersWithViews(options =>
{
   // options.Filters.Add<TransferMiddlewareMessagesFilter>();
    options.Filters.AddService<BreadcrumbFilter>();

    options.Filters.Add<UiExceptionFilter>(); // ← registro global de exepciones
});

//#####################
//FIN FILTROS
//######################




builder.Services.AddScoped<IMenuClientService, MenuClientService>();
builder.Services.AddScoped<IASNService, ASNService>();

// Necesario para el fallback de TempData si no hay Controller
builder.Services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();

// Cliente HTTP
builder.Services.AddHttpClient();

// Sesión
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(60); // tiempo de expiración de la sesión
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

var app = builder.Build();

// Manejo de errores
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// Middlewares fundamentales
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// Asegura que la sesión esté lista antes del middleware
app.UseSession();

// Middleware para validar el token (se ejecuta antes de Authorization)
app.UseMiddleware<TokenValidationMiddleware>();

// Autorización después del middleware personalizado
app.UseAuthorization();

// Rutas estáticas personalizadas (si usas .WithStaticAssets())
app.MapStaticAssets();

// Rutas MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();
