using System.Security.Claims;
using CardamomoPay.Api.Seguridad;

namespace CardamomoPay.Api.Endpoints;

public static class EndpointsBoveda
{
    public static void MapBoveda(this IEndpointRouteBuilder app)
    {
        var grupo = app.MapGroup("/api/v2/boveda").WithTags("Bóveda").RequireAuthorization();

        // Bóveda interna: solo para procesos internos (claim modo=interno).
        grupo.MapGet("/", (ClaimsPrincipal usuario, Banderas banderas) =>
        {
            var modo = usuario.FindFirstValue("modo");
            if (modo != "interno")
            {
                return Results.Json(new
                {
                    error = "Acceso denegado a la bóveda interna.",
                    requisito = "Se requiere un token con el claim \"modo\":\"interno\".",
                }, statusCode: StatusCodes.Status403Forbidden);
            }

            return Results.Ok(new
            {
                mensaje = "Bóveda interna de CardamomoPay",
                bandera = banderas.Boveda,
            });
        })
        .WithSummary("Bóveda interna (requiere un token de modo interno)");
    }
}
