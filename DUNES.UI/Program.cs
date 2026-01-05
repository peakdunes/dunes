//funcionando
using DUNES.UI.Infrastructure;
using DUNES.UI.Interfaces.Print;
using DUNES.UI.Middleware;
using DUNES.UI.Services.Admin;
using DUNES.UI.Services.Inventory.ASN;
using DUNES.UI.Services.Inventory.Common;
using DUNES.UI.Services.Inventory.PickProcess;
using DUNES.UI.Services.Print;

using DUNES.UI.Services.WMS.Masters.Cities;
using DUNES.UI.Services.WMS.Masters.ClientCompanies;
using DUNES.UI.Services.WMS.Masters.Companies;
using DUNES.UI.Services.WMS.Masters.CompaniesClientDivision;
using DUNES.UI.Services.WMS.Masters.CompaniesContract;
using DUNES.UI.Services.WMS.Masters.Countries;
using DUNES.UI.Services.WMS.Masters.StatesCountries;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

var builder = WebApplication.CreateBuilder(args);

// Registro del filtro personalizado (usado en controladores)

//#####################
//FILTROS
//######################

//Filtro para mostrar los avisos informations
//builder.Services.AddScoped<TransferMiddlewareMessagesFilter>();



// MVC con filtros globales
builder.Services.AddControllersWithViews(options =>
{


    options.Filters.Add<UiExceptionFilter>(); // ← registro global de exepciones
});

//#####################
//FIN FILTROS
//######################




builder.Services.AddScoped<IMenuClientUIService, MenuClientUIService>();
builder.Services.AddScoped<IASNUIService, ASNUIService>();
builder.Services.AddScoped<IPickProcessUIService, PickProcessUIService>();
builder.Services.AddScoped<ICommonINVUIService, CommonINVUIService>();


builder.Services.AddScoped<ICompaniesWMSUIService, CompaniesWMSUIService>();

builder.Services.AddScoped<IClientCompaniesWMSUIService, ClientCompaniesWMSUIService>();

builder.Services.AddScoped<ICompaniesClientDivisionWMSUIService, CompaniesClientDivisionWMSUIService>();

builder.Services.AddScoped<ICompaniesClientContractWMSUIService, CompaniesClientContractWMSUIService>();
 
builder.Services.AddScoped<ICountriesWMSUIService, CountriesWMSUIService>();

builder.Services.AddScoped<IStatesCountriesWMSUIService, StatesCountriesWMSUIService>();

builder.Services.AddScoped<ICitiesWMSUIService, CitiesWMSUIService>();


builder.Services.AddScoped<IPdfDocumentService, PdfDocumentService>();
builder.Services.AddScoped<IPdfService, PdfService>();

// Necesario para el fallback de TempData si no hay Controller
builder.Services.AddSingleton<ITempDataProvider, CookieTempDataProvider>();

// Cliente HTTP
builder.Services.AddHttpClient("DUNES_API", (sp, client) =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    client.BaseAddress = new Uri(config["ApiSettings:BaseUrl"]!);
});

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
