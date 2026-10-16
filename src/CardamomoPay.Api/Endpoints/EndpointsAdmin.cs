using CardamomoPay.Api.Contratos;
using CardamomoPay.Api.Domain;
using CardamomoPay.Api.Seguridad;
using Microsoft.EntityFrameworkCore;

namespace CardamomoPay.Api.Endpoints;

public static class EndpointsAdmin
{
    public static void MapAdmin(this IEndpointRouteBuilder app)
    {
        // Endpoints de administración de la cooperativa.
        var grupo = app.MapGroup("/api/v2/admin")
            .WithTags("Administración")
            .RequireAuthorization();

        grupo.MapGet("/socios", async (CardamomoDbContext db) =>
        {
            var socios = await db.Socios
                .Select(s => new SocioDto(s.Id, s.Nombre, s.Correo, s.Municipio, s.Rol))
                .ToListAsync();
            return Results.Ok(socios);
        })
        .WithSummary("Listar todos los socios");

        grupo.MapGet("/panel", (Banderas banderas) =>
        {
            // Panel interno de la cooperativa.
            return Results.Ok(new
            {
                titulo = "Panel de administración · CardamomoPay",
                mensaje = "Zona restringida de la cooperativa.",
                bandera = banderas.Panel,
            });
        })
        .WithSummary("Panel de administración");
    }
}
