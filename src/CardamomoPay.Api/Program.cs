using System.IdentityModel.Tokens.Jwt;
using System.Text;
using CardamomoPay.Api.Domain;
using CardamomoPay.Api.Endpoints;
using CardamomoPay.Api.Seguridad;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// --- Base de datos: SQLite dentro del contenedor, estado limpio en cada arranque ---
var rutaDb = builder.Configuration.GetConnectionString("Sqlite") ?? "Data Source=cardamomo.db";
builder.Services.AddDbContext<CardamomoDbContext>(o => o.UseSqlite(rutaDb));

// --- JWT ---
var opcionesJwt = new OpcionesJwt();
builder.Configuration.GetSection("Jwt").Bind(opcionesJwt);
// Secreto obligatorio en producción; valor de desarrollo solo como respaldo local.
if (string.IsNullOrWhiteSpace(opcionesJwt.Secreto))
    opcionesJwt.Secreto = "desarrollo-local-cambiar-en-produccion-0123456789";
builder.Services.AddSingleton(opcionesJwt);
builder.Services.AddScoped<ServicioToken>();

// --- Banderas (valores por variable de entorno) ---
var banderas = new Banderas();
builder.Configuration.GetSection("Banderas").Bind(banderas);
builder.Services.AddSingleton(banderas);

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(o =>
    {
        // Conservar los nombres de claim originales del token (ej. "sub") sin remapearlos.
        o.MapInboundClaims = false;
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = false,
            RequireSignedTokens = false,
            SignatureValidator = (token, _) =>
                new Microsoft.IdentityModel.JsonWebTokens.JsonWebToken(token),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = false,
        };
    });
builder.Services.AddAuthorization();

builder.Services.ConfigureHttpJsonOptions(o =>
    o.SerializerOptions.DefaultIgnoreCondition =
        System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull);

builder.Services.AddHttpClient();

builder.Services.AddOpenApi();

var app = builder.Build();

// Semilla de datos al arrancar.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CardamomoDbContext>();
    Semilla.Inicializar(db);
}

app.UseDeveloperExceptionPage();

// Log JSON por petición (antes de auth para capturar también los 401).
app.UseMiddleware<RegistroPeticiones>();

app.UseAuthentication();
app.UseAuthorization();

// OpenAPI + Scalar como UI navegable.
app.MapOpenApi();
app.MapScalarApiReference(o => o.WithTitle("CardamomoPay API"));

app.MapGet("/", () => Results.Redirect("/scalar/v1"));
app.MapGet("/salud", () => Results.Ok(new { estado = "ok", servicio = "CardamomoPay" })).WithTags("Infra");

app.MapAuth();
app.MapCuentas();
app.MapPerfil();
app.MapAdmin();
app.MapReportes();
app.MapBoveda();
app.MapTransferencias();
app.MapFoto();

app.Run();

// Expuesto para las pruebas de integración (WebApplicationFactory).
public partial class Program;
