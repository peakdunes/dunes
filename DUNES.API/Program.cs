

using DUNES.API.Data;
using DUNES.API.Models.Masters;
using DUNES.API.Repositories.Auth;
using DUNES.API.Repositories.B2B.Common.Queries;
using DUNES.API.Repositories.Inventory.ASN.Queries;
using DUNES.API.Repositories.Inventory.Common.Queries;
using DUNES.API.Repositories.Inventory.PickProcess.Queries;
using DUNES.API.Repositories.Inventory.PickProcess.Transactions;
using DUNES.API.Repositories.Masters;
using DUNES.API.RepositoriesWMS.Inventory.Common.Queries;
using DUNES.API.RepositoriesWMS.Inventory.Transactions;
using DUNES.API.RepositoriesWMS.Masters;
using DUNES.API.Services.Auth;
using DUNES.API.Services.B2B.Common.Queries;
using DUNES.API.Services.Inventory.ASN.Queries;
using DUNES.API.Services.Inventory.Common.Queries;
using DUNES.API.Services.Inventory.PickProcess.Queries;
using DUNES.API.Services.Inventory.PickProcess.Transactions;
using DUNES.API.Services.Masters;
using DUNES.API.ServicesWMS.Inventory.Common.Queries;
using DUNES.API.ServicesWMS.Inventory.Transactions;
using DUNES.API.ServicesWMS.Masters;
using DUNES.API.Utils.Logging;
using DUNES.API.Utils.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebSockets;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System;

using System.Text;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;
using AutoMapper;
using System.Reflection;
using DUNES.API.Profiles;

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

    c.TagActionsBy(api =>
    {
        var controllerName = api.GroupName ?? api.ActionDescriptor.RouteValues["controller"];

        return controllerName switch
        {
            "CommonQueryINV" => new[] { "Inventory - Common Queries" },
            "PickProcessINV" => new[] { "Inventory - Pick Process" },
            "CommonQueryASNINV" => new[] { "Inventory ASN - Common queries" },
            "MasterInventory" => new[] { "Inventory Item Master - CRUD" },
            "ConsignmentCallType" => new[] { "Inventory Calls - CRUD" },
            "TzebB2bInventoryType" => new[] { "Inventory Types - CRUD" },
            "WmsCompanyclient" => new[] { "Company Clients - CRUD" },

            

            "TzebFaultCodes" => new[] { "B2B - Fault Codes CRUD" },
            "TrepairActionsCodes" => new[] { "B2B - Action Codes CRUD" },
            "TzebWorkCodesTargets" => new[] { "B2B - Work Target Codes CRUD" },

            "TransactionWMSINV" => new[] { "WMS - Transactions" },


            "CommonQueryB2B" => new[] { "B2B - Common queries" },
            "CommonQueryWMSMaster" => new[] { "WMS Master Tables - Common queries" },

            


            "CommonQueryWMSINV" => new[] { "WMS Inventory - Common queries" },
            
            _ => new[] { controllerName }
        };
    });
});


//SERVICES

//Mapper


//AUTHENTICATION SERVICES
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICurrentUser, CurrentUser>();

builder.Services.AddHttpContextAccessor();


builder.Services.AddScoped<IMenuRepository, MenuRepository>();
builder.Services.AddScoped<IMenuService, MenuService>();


//save error exception log in database table dbk_mvc_logs_api
builder.Services.AddScoped<LogHelper>();

//B2B SERVICES
//Query services and repository
builder.Services.AddScoped<ICommonQueryB2BRepository, CommonQueryB2BRepository>();
builder.Services.AddScoped<ICommonQueryB2BService, CommonQueryB2BService>();

//INV SERVICES

builder.Services.AddScoped<ICommonQueryASNINVRepository, CommonQueryASNINVRepository>();
builder.Services.AddScoped<ICommonQueryASNINVService, CommonQueryASNINVService>();

builder.Services.AddScoped<ICommonQueryPickProcessINVRepository, CommonQueryPickProcessINVRepository>();
builder.Services.AddScoped<ICommonQueryPickProcessINVService, CommonQueryPickProcessINVService>();

builder.Services.AddScoped<ITransactionsPickProcessINVRepository, TransactionsPickProcessINVRepository>();
builder.Services.AddScoped<ITransactionsPickProcessINVService, TransactionsPickProcessINVService>();

builder.Services.AddScoped<ICommonQueryINVRepository, CommonQueryINVRepository>();
builder.Services.AddScoped<ICommonQueryINVService, CommonQueryINVService>();

//generic service for master tables
builder.Services.AddScoped(typeof(IMasterRepository<>), typeof(MasterRepository<>));

//para usar en el CRUD de las tablas maestras
builder.Services.AddScoped(typeof(IMasterService<,>), typeof(MasterService<,>));

//WMS MASTER SERVICES

builder.Services.AddScoped<ICommonQueryWMSMasterRepository, CommonQueryWMSMasterRepository>();
builder.Services.AddScoped<ICommonQueryWMSMasterService, CommonQueryWMSMasterService>();



//WMS INV SERVICES

builder.Services.AddScoped<ICommonQueryWMSINVRepository, CommonQueryWMSINVRepository>();
builder.Services.AddScoped<ICommonQueryWMSINVService, CommonQueryWMSINVService>();
builder.Services.AddScoped<ITransactionsWMSINVService, TransactionsWMSINVService>();
builder.Services.AddScoped<ITransactionsWMSINVRepository, TransactionsWMSINVRepository>();


// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
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

app.UseAuthentication();

app.UseAuthorization();
app.MapControllers();
app.Run();
