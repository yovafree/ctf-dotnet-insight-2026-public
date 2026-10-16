using CardamomoPay.Api.Contratos;
using CardamomoPay.Api.Domain;
using CardamomoPay.Api.Seguridad;
using Microsoft.EntityFrameworkCore;

namespace CardamomoPay.Api.Endpoints;

public static class EndpointsAuth
{
    public static void MapAuth(this IEndpointRouteBuilder app)
    {
        var grupo = app.MapGroup("/api/v2/auth").WithTags("Autenticación");

        grupo.MapPost("/login", async (LoginRequest req, CardamomoDbContext db, ServicioToken tokens) =>
        {
            var socio = await db.Socios.SingleOrDefaultAsync(s => s.Correo == req.Correo);
            if (socio is null || !Semilla.Verificar(req.Contrasena, socio.HashContrasena))
                return Results.Unauthorized();

            var token = tokens.Emitir(socio);
            return Results.Ok(new LoginResponse(token, socio.Nombre, socio.Rol));
        })
        .WithSummary("Iniciar sesión y obtener un token JWT");
    }
}
