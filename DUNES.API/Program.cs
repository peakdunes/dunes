

using AutoMapper;
using DUNES.API.Auth.Authorization;
using DUNES.API.ControllersWMS.Masters.CompaniesContract;
using DUNES.API.ControllersWMS.Masters.CompanyClientDivision;
using DUNES.API.Data;
using DUNES.API.Data.Interceptors;
using DUNES.API.Models.Configuration;
using DUNES.API.Models.Masters;
using DUNES.API.Profiles;
using DUNES.API.Repositories.B2B.Common.Queries;
using DUNES.API.Repositories.Inventory.ASN.Queries;
using DUNES.API.Repositories.Inventory.ASN.Transactions;
using DUNES.API.Repositories.Inventory.Common.Queries;
using DUNES.API.Repositories.Inventory.Common.Transactions;
using DUNES.API.Repositories.Inventory.PickProcess.Queries;
using DUNES.API.Repositories.Inventory.PickProcess.Transactions;
using DUNES.API.Repositories.Masters;
using DUNES.API.Repositories.WebService.Queries;
using DUNES.API.Repositories.WebService.Transactions;
using DUNES.API.RepositoriesWMS.Auth;
using DUNES.API.RepositoriesWMS.Inventory.Common.Queries;
using DUNES.API.RepositoriesWMS.Inventory.Transactions;
using DUNES.API.RepositoriesWMS.Masters;
using DUNES.API.RepositoriesWMS.Masters.Cities;
using DUNES.API.RepositoriesWMS.Masters.ClientCompanies;
using DUNES.API.RepositoriesWMS.Masters.Companies;
using DUNES.API.RepositoriesWMS.Masters.CompaniesClientDivision;
using DUNES.API.RepositoriesWMS.Masters.CompaniesContract;
using DUNES.API.RepositoriesWMS.Masters.Countries;
using DUNES.API.RepositoriesWMS.Masters.Locations;
using DUNES.API.RepositoriesWMS.Masters.Racks;
using DUNES.API.RepositoriesWMS.Masters.StateCountries;
using DUNES.API.Services.Auth;
using DUNES.API.Services.B2B.Common.Queries;
using DUNES.API.Services.Inventory.ASN.Queries;
using DUNES.API.Services.Inventory.ASN.Transactions;
using DUNES.API.Services.Inventory.Common.Queries;
using DUNES.API.Services.Inventory.PickProcess.Queries;
using DUNES.API.Services.Inventory.PickProcess.Transactions;
using DUNES.API.Services.Masters;
using DUNES.API.Services.WebService.Queries;
using DUNES.API.Services.WebService.Transactions;
using DUNES.API.ServicesWMS.Auth;
using DUNES.API.ServicesWMS.Inventory.Common.Queries;
using DUNES.API.ServicesWMS.Inventory.Transactions;
using DUNES.API.ServicesWMS.Masters;
using DUNES.API.ServicesWMS.Masters.Cities;
using DUNES.API.ServicesWMS.Masters.ClientCompanies;
using DUNES.API.ServicesWMS.Masters.Companies;
using DUNES.API.ServicesWMS.Masters.CompaniesClientDivision;
using DUNES.API.ServicesWMS.Masters.CompaniesContract;
using DUNES.API.ServicesWMS.Masters.Countries;
using DUNES.API.ServicesWMS.Masters.Locations;
using DUNES.API.ServicesWMS.Masters.Racks;
using DUNES.API.ServicesWMS.Masters.StateCountries;
using DUNES.API.Utils.Logging;
using DUNES.API.Utils.Middlewares;
using DUNES.API.Utils.TraceProvider;
using DUNES.Shared.DTOs.Auth;
using DUNES.Shared.DTOs.Masters;
using DUNES.Shared.DTOs.WMS;
using DUNES.Shared.Interfaces.AuditContext;
using DUNES.Shared.Interfaces.RequestInfo;
using FluentValidation;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebSockets;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
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

//GET HTTP Information SERVICE (Auditory)

builder.Services.AddHttpContextAccessor();

builder.Services.AddScoped<IRequestInfo, RequestInfo>();
builder.Services.AddScoped<IAuditContext, AuditContext>();
builder.Services.AddScoped<AuditSaveChangesInterceptor>();




//DBK database conection
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

//DBKWMS database conecton
builder.Services.AddDbContext<appWmsDbContext>((sp, options) =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultWMSConnection"));
    //interceptor do auditory insert record for Insert, Update and Delete transactions
    options.AddInterceptors(sp.GetRequiredService<AuditSaveChangesInterceptor>());
});

//User administration with Identity
builder.Services.AddDbContext<IdentityDbContext>(options =>
options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultWMSConnection")));
//options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

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

            //#########
            //WMS AUTH
            //#########
            "UserConfiguration" => new[] { "AUTH User Configuration" },
            "Auth" => new[] { "AUTH User Authentication" },
            
            //#########
            //INV
            //#########

            "CommonQueryINV" => new[] { "Inventory - Common Queries" },
            "PickProcessINV" => new[] { "Inventory - Pick Process" },
            "CommonQueryASNINV" => new[] { "Inventory ASN - Common queries" },


            //#########
            //INV WMS
            //#########
            "TransactionWMSINV" => new[] { "WMS - Transactions" },
            "CommonQueryWMSINV" => new[] { "WMS Inventory - Common queries" },

            //#########
            //WMS MASTERS
            //#########
            "ClientCompaniesWMS" => new[] { "WMS - Client Companies" },

            "StatesCountriesWMS" => new[] { "WMS - States (Countries)" },

            "CitiesWMS" => new[] { "WMS - Cities" },

            "CountriesWMS" => new[] { "WMS - Countries" },

            "WmsCompanyclient" => new[] { "WMS Company Clients - CRUD" },

            "CompanyClientDivisionWMS" => new[] { "WMS Company Clients Division - CRUD" },

            "CompaniesContractWMS" => new[] { "WMS Company Clients Contract - CRUD" },

            "LocationsWMS" => new[] { "WMS Locations - CRUD" },

            "RacksWMS" => new[] { "WMS Racks - CRUD" },

            "BinsWMS" => new[] { "WMS Bins - CRUD" },

            



            //#########
            //MASTERS
            //#########
            "MasterInventory" => new[] { "Inventory Item Master - CRUD" },
            "ConsignmentCallType" => new[] { "Inventory Calls - CRUD" },
            "TzebB2bInventoryType" => new[] { "Inventory Types - CRUD" },
           
            "mvcGeneralParameters" => new[] { "General Parameters - CRUD" },
           




            //#########
            //B2B
            //#########
            "TzebFaultCodes" => new[] { "B2B - Fault Codes CRUD" },
            "TrepairActionsCodes" => new[] { "B2B - Action Codes CRUD" },
            "TzebWorkCodesTargets" => new[] { "B2B - Work Target Codes CRUD" },
            "CommonQueryB2B" => new[] { "B2B - Common queries" },
            "CommonQueryWMSMaster" => new[] { "WMS Master Tables - Common queries" },
            
            _ => new[] { controllerName }
        };
    });
});


//SERVICES



//AUTHENTICATION SERVICES
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ICurrentUser, CurrentUser>();
builder.Services.AddScoped<IUserConfigurationService, UserConfigurationService>();


builder.Services.AddScoped<IUserConfigurationRepository, UserConfigurationRepository>();
builder.Services.AddScoped<IAuthPermissionRepository, AuthPermissionRepository>();
builder.Services.AddScoped<IAuthPermissionService, AuthPermissionService>();
//builder.Services.AddScoped<RequiresPermissionFilter>();



builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ITraceProvider, TraceProvider>();



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


builder.Services.AddScoped<ITransactionsASNINVRepository, TransactionsASNINVRepository>();
builder.Services.AddScoped<ITransactionsASNINVService, TransactionsASNINVService>();

builder.Services.AddScoped<ITransactionsCommonINVRepository, TransactionsCommonINVRepository>();



builder.Services.AddScoped<ICommonQueryINVRepository, CommonQueryINVRepository>();
builder.Services.AddScoped<ICommonQueryINVService, CommonQueryINVService>();

//generic service for master tables
builder.Services.AddScoped(typeof(IMasterRepository<>), typeof(MasterRepository<>));

//para usar en el CRUD de las tablas maestras
builder.Services.AddScoped(typeof(IMasterService<,>), typeof(MasterService<,>));

//VALIDATOR SERVICES
builder.Services.AddFluentValidationAutoValidation();
builder.Services.AddScoped<IValidator<WMSClientCompaniesDTO>, ClientCompaniesWMSAPIValidator>();
builder.Services.AddScoped<IValidator<WMSCompanyClientDivisionDTO>, CompaniesClientDivisionWMSAPIValidator>();

builder.Services.AddScoped<IValidator<WMSCompaniesContractDTO>, CompaniesContractWMSAPIValidator>();
builder.Services.AddScoped<IValidator<WMSLocationsDTO>, LocationsWMSAPIValidator>();
builder.Services.AddScoped<IValidator<UserConfigurationUpdateDto>, UserConfigurationValidator>();


//WMS MASTER SERVICES

builder.Services.AddScoped<ICommonQueryWMSMasterRepository, CommonQueryWMSMasterRepository>();
builder.Services.AddScoped<ICommonQueryWMSMasterService, CommonQueryWMSMasterService>();

builder.Services.AddScoped<ICompaniesWMSAPIRepository, CompaniesWMSAPIRepository>();

builder.Services.AddScoped<IClientCompaniesWMSAPIRepository, ClientCompaniesWMSAPIRepository>();
builder.Services.AddScoped<IClientCompaniesWMSAPIService, ClientCompaniesWMSAPIService>();
builder.Services.AddScoped<ICompaniesWMSAPIService, CompaniesWMSAPIService>();

builder.Services.AddScoped<ICommandCompaniesClientDivisionWMSAPIRepository, CommandCompaniesClientDivisionWMSAPIRepository>();
builder.Services.AddScoped<IQueryCompaniesClientDivisionWMSAPIRepository, QueryCompaniesClientDivisionWMSAPIRepository>();
builder.Services.AddScoped<ICompaniesClientDivisionWMSAPIService, CompaniesClientDivisionWMSAPIService>();

builder.Services.AddScoped<ICommandCompaniesContractWMSAPIRepository, CommandCompaniesContractWMSAPIRepository>();
builder.Services.AddScoped<IQueryCompaniesContractWMSAPIRepository, QueryCompaniesContractWMSAPIRepository>();
builder.Services.AddScoped<ICompaniesContractWMSAPIService, CompaniesContractWMSAPIService>();



builder.Services.AddScoped<ICountriesWMSAPIRepository, CountriesWMSAPIRepository>();
builder.Services.AddScoped<ICountriesWMSAPIService, CountriesWMSAPIService>();


builder.Services.AddScoped<IRacksWMSAPIRepository, RacksWMSAPIRepository>();
builder.Services.AddScoped<IRacksWMSAPIService, RacksWMSAPIService>();


builder.Services.AddScoped<IStateCountriesWMSAPIRepository, StateCountriesWMSAPIRepository>();
builder.Services.AddScoped<IStateCountriesWMSAPIService, StateCountriesWMSAPIService>();

builder.Services.AddScoped<ICitiesWMSAPIRepository, CitiesWMSAPIRepository>();
builder.Services.AddScoped<ICitiesWMSAPIService, CitiesWMSAPIService>();

builder.Services.AddScoped<ILocationsWMSAPIRepository, LocationsWMSAPIRepository>();
builder.Services.AddScoped<ILocationsWMSAPIService, LocationsWMSAPIService>();


//WEB SERVICE SERVICES

builder.Services.AddScoped<ICommonQueryWebServiceRepository, CommonQueryWebServiceRepository>();
builder.Services.AddScoped<ITransactionsWebServiceRepository, TransactionsWebServiceRepository>();


builder.Services.AddScoped<ICommonQueryWebServiceService, CommonQueryWebServiceService>();
builder.Services.AddScoped<ITransactionsWebServiceService, TransactionsWebServiceService>();



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


// Middleware: obtiene informacion de la transaccion 
app.Use(async (ctx, next) =>
{
    //esta variable "reqInfo" toma los datos claves del request 
    //(Method, Path, Query) para dejarlos en un servicio scoped (IRequestInfo)
    //para que por ID podamos mostrarlos en cualquier parte del API

    var reqInfo = ctx.RequestServices.GetRequiredService<IRequestInfo>();
    reqInfo.Path = ctx.Request.Path.Value;
    reqInfo.Method = ctx.Request.Method;
    reqInfo.Query = ctx.Request.QueryString.Value;

    // TraceId: toma del header o genera
    var traceId = ctx.Request.Headers["X-Trace-Id"].FirstOrDefault() ?? Guid.NewGuid().ToString();
    ctx.Items["TraceId"] = traceId; // disponible para el resto del pipeline

    // Escribir el header justo antes de enviar la respuesta (evita el error de headers read-only)
    ctx.Response.OnStarting(() =>
    {
        if (!ctx.Response.Headers.ContainsKey("X-Trace-Id"))
            ctx.Response.Headers.Append("X-Trace-Id", traceId);
        return Task.CompletedTask;
    });


    await next();
});

//escribe log de transacciones usando Serilog, y aqui en opts adicionamos 
//TraceId UserName CLienteIp y queryString al archivo de log
app.UseSerilogRequestLogging(opts =>
{

    //este es el formato del log que muestra en cada linea del archivo.
    opts.MessageTemplate =
        "HTTP {RequestMethod} {RequestPath}{QueryString} responded {StatusCode} in {Elapsed:0.0000} ms | TraceId={TraceId} User={UserName} Ip={ClientIp}";

    opts.EnrichDiagnosticContext = (diag, http) =>
    {
        var traceId = http.Items.TryGetValue("TraceId", out var t) ? t as string : null;
        var user = http.User?.Identity?.Name ?? "anonymous";
        var ip = http.Connection.RemoteIpAddress?.ToString() ?? "n/a";
        var qs = http.Request.QueryString.HasValue ? http.Request.QueryString.Value : "";

        diag.Set("TraceId", traceId ?? "n/a");
        diag.Set("UserName", user);
        diag.Set("ClientIp", ip);
        diag.Set("QueryString", qs);
    };
});



app.UseSwagger();
app.UseSwaggerUI();



app.UseRouting();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseAuthentication();

app.UseMiddleware<CompanyContextMiddleware>();

app.UseAuthorization();
app.MapControllers();


app.Run();
