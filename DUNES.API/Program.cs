

using DUNES.API.Data;
using DUNES.API.Repositories.Auth;
using DUNES.API.Repositories.B2B.Common.Queries;
using DUNES.API.Repositories.Inventory.Common.Queries;
using DUNES.API.Repositories.Masters;
using DUNES.API.Services.Auth;
using DUNES.API.Services.B2B.Common.Queries;
using DUNES.API.Services.Inventory.Common.Queries;
using DUNES.API.Services.Masters;
using DUNES.API.Utils.Logging;
using DUNES.API.Utils.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebSockets;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.Reflection;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);


Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .CreateLogger();

builder.Host.UseSerilog();


//DBK database conection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//DBKWMS database conecton
builder.Services.AddDbContext<appWmsDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultWMSConnection")));

//User administration with Identity
builder.Services.AddDbContext<IdentityDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//authentication services

builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    .AddEntityFrameworkStores<IdentityDbContext>()
    .AddDefaultTokenProviders();



builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "DUNES.API",
            ValidAudience = "DUNES.API",
            IssuerSigningKey = new SymmetricSecurityKey(
           Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"]))
        };
 
});



// Add services to the container.



builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        //Convierte los nombres de las propiedades a camelCase (ej: processDate en vez de ProcessDate) — esto es lo esperado por JSON estándar
        options.JsonSerializerOptions.PropertyNamingPolicy = System.Text.Json.JsonNamingPolicy.CamelCase;
        //Evita errores por ciclos de referencia al serializar relaciones (útil si en algún modelo tienes navegación entre entidades)
        options.JsonSerializerOptions.ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
        //asegura que nada se omita, incluso si está null, false, 0, etc. — necesario para que ProcessDate, Warnings y Error aparezcan siempre
        options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.Never;
    });



builder.Services.AddEndpointsApiExplorer();

//para mostrar swagger

//builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(c =>
{
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);


    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "DUNES.API",
        Version = "v1",
        Description = "API to repair process and inventory administration",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "Soporte DUNES",
            Email = "herledy.lopez@peaktech.com"
        }
    });

    // JWT Authentication
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter 'Bearer' [space] and then your valid token.\n\nExample: Bearer eyJhbGciOiJIUzI1..."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});


//SERVICES


//AUTHENTICATION SERVICES
builder.Services.AddScoped<IAuthService, AuthService>();

builder.Services.AddScoped<IMenuRepository, MenuRepository>();
builder.Services.AddScoped<IMenuService, MenuService>();


//save error exception log in database table dbk_mvc_logs_api
builder.Services.AddScoped<LogHelper>();

//B2B SERVICES
//Query services and repository
builder.Services.AddScoped<ICommonQueryB2BRepository, CommonQueryB2BRepository>();
builder.Services.AddScoped<ICommonQueryB2BService, CommonQueryB2BService>();

//INV SERVICES

builder.Services.AddScoped<ICommonQueryINVRepository, CommonQueryINVRepository>();
builder.Services.AddScoped<ICommonQueryINVService, CommonQueryINVService>();

//generic service for master tables
builder.Services.AddScoped(typeof(IMasterRepository<>), typeof(MasterRepository<>));

//para usar en el CRUD de las tablas maestras
builder.Services.AddScoped(typeof(IMasterService<>), typeof(MasterService<>));




// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "DUNES.API v1");
        c.RoutePrefix = "docs"; // 👉 la doc estará en /docs
        c.DocumentTitle = "DUNES.API Docs";
        c.InjectStylesheet("/swagger-ui/custom.css"); // CSS custom (lo haremos ahora)
    });
}


app.UseSwagger();
app.UseSwaggerUI();

app.UseSerilogRequestLogging();
app.UseRouting();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseAuthorization();
app.MapControllers();
app.Run();
