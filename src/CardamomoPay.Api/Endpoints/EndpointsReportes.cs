using CardamomoPay.Api.Seguridad;

namespace CardamomoPay.Api.Endpoints;

public static class EndpointsReportes
{
    public static void MapReportes(this IEndpointRouteBuilder app)
    {
        var grupo = app.MapGroup("/api/v2/reportes").WithTags("Reportes").RequireAuthorization();

        // Genera el reporte mensual conectándose a la base de reportes heredada.
        grupo.MapGet("/mensual", (Banderas banderas) =>
        {
            var cadena =
                $"Server=reportes-legacy.cardamomopay.local;Database=Reportes;" +
                $"User Id=sa;Password={banderas.Reporte};TrustServerCertificate=true";

            throw new InvalidOperationException(
                $"No se pudo abrir la conexión con la base de reportes. Cadena usada: {cadena}");
        })
        .WithSummary("Generar el reporte mensual (heredado)");
    }
}
