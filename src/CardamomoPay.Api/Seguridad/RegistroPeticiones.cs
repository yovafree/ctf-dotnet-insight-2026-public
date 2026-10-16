using System.Text.Json;

namespace CardamomoPay.Api.Seguridad;

/// <summary>
/// Escribe una línea JSON por petición a la salida estándar (método, ruta, estado,
/// socio del token, etc.) para fines de auditoría.
/// </summary>
public class RegistroPeticiones(RequestDelegate siguiente, ILogger<RegistroPeticiones> log)
{
    public async Task Invoke(HttpContext ctx)
    {
        var inicio = DateTime.UtcNow;
        try
        {
            await siguiente(ctx);
        }
        finally
        {
            // Identidad del llamante tomada del token (si lo hay).
            var socioId = ctx.User?.FindFirst("sub")?.Value
                          ?? ctx.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var rol = ctx.User?.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value;

            var entrada = new
            {
                ts = inicio,
                metodo = ctx.Request.Method,
                ruta = ctx.Request.Path.Value,
                estado = ctx.Response.StatusCode,
                ms = (int)(DateTime.UtcNow - inicio).TotalMilliseconds,
                socioId,
                rol,
                recurso = ctx.Request.RouteValues.TryGetValue("id", out var id) ? id?.ToString() : null,
                ip = ctx.Connection.RemoteIpAddress?.ToString(),
            };

            log.LogInformation("ACCESO {Acceso}", JsonSerializer.Serialize(entrada));
        }
    }
}
