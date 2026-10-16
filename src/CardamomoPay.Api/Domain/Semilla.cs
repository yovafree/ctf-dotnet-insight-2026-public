using System.Security.Cryptography;

namespace CardamomoPay.Api.Domain;

/// <summary>
/// Carga los datos iniciales. Cada contenedor arranca con esta misma semilla,
/// de modo que todos los equipos ven exactamente el mismo escenario.
/// </summary>
public static class Semilla
{
    public static void Inicializar(CardamomoDbContext db)
    {
        db.Database.EnsureCreated();
        if (db.Socios.Any()) return;

        // Socios de la cooperativa.
        var yolanda = NuevoSocio("Yolanda Caal", "yolanda@cardamomopay.gt", "Cobán", "742", 48250.75m);
        var mynor = NuevoSocio("Mynor Chub", "mynor@cardamomopay.gt", "Carchá", "118", 1320.00m);
        var ligia = NuevoSocio("Ligia Pop", "ligia@cardamomopay.gt", "Chamelco", "305", 875.50m);

        // Cuenta con la que inicias sesión en el taller.
        var jugador = NuevoSocio("Equipo Invitado", "equipo@cardamomopay.gt", "Cobán", "486", 100.00m);

        // Administrador de la cooperativa.
        var admin = NuevoSocio("Admin Cooperativa", "admin@cardamomopay.gt", "Cobán", "246", 5000.00m);
        admin.Rol = "admin";

        db.Socios.AddRange(yolanda, mynor, ligia, jugador, admin);
        db.SaveChanges();
    }

    private static Socio NuevoSocio(string nombre, string correo, string municipio, string pin, decimal saldo)
    {
        var socio = new Socio
        {
            Nombre = nombre,
            Correo = correo,
            Municipio = municipio,
            Pin = pin,
            Rol = "socio",
            HashContrasena = Hash("cardamomo"),
        };
        socio.Cuentas.Add(new Cuenta
        {
            Numero = NumeroCuenta(correo),
            Saldo = saldo,
            Movimientos =
            {
                new Movimiento { Fecha = DateTime.UtcNow.AddDays(-3), Monto = saldo, Concepto = "Saldo inicial" },
            },
        });
        return socio;
    }

    // Número de cuenta estable y legible, derivado del correo.
    private static string NumeroCuenta(string correo)
        => "AV-" + Math.Abs(correo.GetHashCode() % 900000 + 100000);

    /// <summary>Hash de contraseña con PBKDF2. La contraseña demo de todos es "cardamomo".</summary>
    public static string Hash(string contrasena)
    {
        byte[] sal = RandomNumberGenerator.GetBytes(16);
        byte[] clave = Rfc2898DeriveBytes.Pbkdf2(contrasena, sal, 100_000, HashAlgorithmName.SHA256, 32);
        return $"{Convert.ToBase64String(sal)}.{Convert.ToBase64String(clave)}";
    }

    public static bool Verificar(string contrasena, string almacenado)
    {
        var partes = almacenado.Split('.');
        if (partes.Length != 2) return false;
        byte[] sal = Convert.FromBase64String(partes[0]);
        byte[] esperado = Convert.FromBase64String(partes[1]);
        byte[] actual = Rfc2898DeriveBytes.Pbkdf2(contrasena, sal, 100_000, HashAlgorithmName.SHA256, 32);
        return CryptographicOperations.FixedTimeEquals(actual, esperado);
    }
}
