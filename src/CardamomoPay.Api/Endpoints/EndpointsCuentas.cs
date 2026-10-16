using System.Security.Claims;
using CardamomoPay.Api.Contratos;
using CardamomoPay.Api.Domain;
using CardamomoPay.Api.Seguridad;
using Microsoft.EntityFrameworkCore;

namespace CardamomoPay.Api.Endpoints;

public static class EndpointsCuentas
{
    public static void MapCuentas(this IEndpointRouteBuilder app)
    {
        var grupo = app.MapGroup("/api/v2/cuentas").WithTags("Cuentas").RequireAuthorization();

        // Consulta de una cuenta de la billetera por su identificador.
        grupo.MapGet("/{id}", async (int id, ClaimsPrincipal usuario, CardamomoDbContext db, Banderas banderas) =>
        {
            var cuenta = await db.Cuentas.FindAsync(id);
            if (cuenta is null) return Results.NotFound();

            var socioId = int.Parse(usuario.FindFirstValue("sub")!);
            var esAjena = cuenta.SocioId != socioId;

            return Results.Ok(new CuentaDto(
                cuenta.Id,
                cuenta.Numero,
                cuenta.Saldo,
                esAjena ? banderas.Cuenta : null));
        })
        .WithSummary("Consultar una cuenta por su identificador");

        grupo.MapGet("/{id}/movimientos", async (int id, ClaimsPrincipal usuario, CardamomoDbContext db) =>
        {
            var cuenta = await db.Cuentas.Include(c => c.Movimientos).FirstOrDefaultAsync(c => c.Id == id);
            if (cuenta is null) return Results.NotFound();

            var movs = cuenta.Movimientos
                .OrderByDescending(m => m.Fecha)
                .Select(m => new MovimientoDto(m.Fecha, m.Monto, m.Concepto));
            return Results.Ok(movs);
        })
        .WithSummary("Listar los movimientos de una cuenta");
    }
}
