using System.Security.Claims;
using CardamomoPay.Api.Domain;
using CardamomoPay.Api.Seguridad;

namespace CardamomoPay.Api.Endpoints;

public record ConfirmarPinRequest(string Pin);

public static class EndpointsTransferencias
{
    public static void MapTransferencias(this IEndpointRouteBuilder app)
    {
        var grupo = app.MapGroup("/api/v2/transferencias").WithTags("Transferencias").RequireAuthorization();

        // Confirma una transferencia validando el PIN del socio.
        grupo.MapPost("/confirmar", async (ConfirmarPinRequest req, ClaimsPrincipal usuario,
                                           CardamomoDbContext db, Banderas banderas) =>
        {
            var socioId = int.Parse(usuario.FindFirstValue("sub")!);
            var socio = await db.Socios.FindAsync(socioId);
            if (socio is null) return Results.NotFound();

            if (req.Pin != socio.Pin)
                return Results.Json(new { error = "PIN incorrecto." },
                    statusCode: StatusCodes.Status401Unauthorized);

            return Results.Ok(new
            {
                mensaje = "Transferencia autorizada.",
                bandera = banderas.Transferencia,
            });
        })
        .WithSummary("Confirmar una transferencia con el PIN");
    }
}
