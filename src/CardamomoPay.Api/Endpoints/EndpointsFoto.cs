using CardamomoPay.Api.Contratos;

namespace CardamomoPay.Api.Endpoints;

public static class EndpointsFoto
{
    public static void MapFoto(this IEndpointRouteBuilder app)
    {
        var grupo = app.MapGroup("/api/v2/perfil").WithTags("Perfil").RequireAuthorization();

        // Importa la foto de perfil descargándola desde la URL indicada.
        grupo.MapPost("/foto", async (ImportarFotoRequest req, IHttpClientFactory factory) =>
        {
            if (string.IsNullOrWhiteSpace(req.Url))
                return Results.BadRequest(new { error = "Falta la URL." });

            var cliente = factory.CreateClient();
            cliente.Timeout = TimeSpan.FromSeconds(5);

            try
            {
                var respuesta = await cliente.GetAsync(req.Url);
                var cuerpo = await respuesta.Content.ReadAsStringAsync();
                return Results.Ok(new
                {
                    estado = (int)respuesta.StatusCode,
                    tipo = respuesta.Content.Headers.ContentType?.ToString(),
                    contenido = cuerpo.Length > 2000 ? cuerpo[..2000] : cuerpo,
                });
            }
            catch (Exception ex)
            {
                return Results.BadRequest(new { error = "No se pudo importar la foto.", detalle = ex.Message });
            }
        })
        .WithSummary("Importar la foto de perfil desde una URL");
    }
}
