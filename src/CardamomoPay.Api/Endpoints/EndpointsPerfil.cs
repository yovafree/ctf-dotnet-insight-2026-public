using System.Security.Claims;
using CardamomoPay.Api.Contratos;
using CardamomoPay.Api.Domain;
using CardamomoPay.Api.Seguridad;
using Microsoft.EntityFrameworkCore;

namespace CardamomoPay.Api.Endpoints;

public static class EndpointsPerfil
{
    public static void MapPerfil(this IEndpointRouteBuilder app)
    {
        var grupo = app.MapGroup("/api/v2/perfil").WithTags("Perfil").RequireAuthorization();

        grupo.MapGet("/", async (ClaimsPrincipal usuario, CardamomoDbContext db) =>
        {
            var socioId = int.Parse(usuario.FindFirstValue("sub")!);
            var socio = await db.Socios.FindAsync(socioId);
            if (socio is null) return Results.NotFound();
            return Results.Ok(new SocioDto(socio.Id, socio.Nombre, socio.Correo, socio.Municipio, socio.Rol));
        })
        .WithSummary("Ver mi perfil");

        // Actualiza los datos del perfil del socio autenticado.
        grupo.MapPut("/", async (ActualizarPerfilRequest req, ClaimsPrincipal usuario,
                                 CardamomoDbContext db, Banderas banderas) =>
        {
            var socioId = int.Parse(usuario.FindFirstValue("sub")!);
            var socio = await db.Socios.FindAsync(socioId);
            if (socio is null) return Results.NotFound();

            socio.Nombre = req.Nombre;
            socio.Municipio = req.Municipio;
            if (req.Rol is not null) socio.Rol = req.Rol;
            await db.SaveChangesAsync();

            var escalo = socio.Rol == "admin";
            return Results.Ok(new
            {
                socio = new SocioDto(socio.Id, socio.Nombre, socio.Correo, socio.Municipio, socio.Rol),
                bandera = escalo ? banderas.Perfil : null,
            });
        })
        .WithSummary("Actualizar mi perfil");
    }
}
