using DUNES.UI.Filters;
using DUNES.UI.Middleware;
using DUNES.UI.Services.Admin;

var builder = WebApplication.CreateBuilder(args);

// Registro del filtro personalizado (usado en controladores)
builder.Services.AddScoped<TransferMiddlewareMessagesFilter>();

builder.Services.AddScoped<IMenuClientService, MenuClientService>();

// MVC con filtros globales
builder.Services.AddControllersWithViews(options =>
{
    options.Filters.Add<TransferMiddlewareMessagesFilter>();
});

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
