namespace CardamomoPay.Api.Domain;

/// <summary>
/// Socio de la cooperativa CardamomoPay. Es a la vez el usuario que inicia sesión
/// y el titular de una o varias cuentas.
/// </summary>
public class Socio
{
    public int Id { get; set; }
    public string Nombre { get; set; } = "";
    public string Correo { get; set; } = "";

    /// <summary>Hash de la contraseña. Nunca se expone en respuestas.</summary>
    public string HashContrasena { get; set; } = "";

    /// <summary>Rol del socio: "socio" o "admin".</summary>
    public string Rol { get; set; } = "socio";

    /// <summary>Municipio de Alta Verapaz del socio (dato de perfil, no sensible).</summary>
    public string Municipio { get; set; } = "";

    /// <summary>PIN de 3 dígitos que autoriza las transferencias.</summary>
    public string Pin { get; set; } = "000";

    public List<Cuenta> Cuentas { get; set; } = new();
}

/// <summary>Cuenta de billetera de un socio, con su saldo en quetzales.</summary>
public class Cuenta
{
    public int Id { get; set; }
    public int SocioId { get; set; }
    public Socio? Socio { get; set; }

    public string Numero { get; set; } = "";
    public decimal Saldo { get; set; }

    public List<Movimiento> Movimientos { get; set; } = new();
}

/// <summary>Movimiento (transferencia o bono) asociado a una cuenta.</summary>
public class Movimiento
{
    public int Id { get; set; }
    public int CuentaId { get; set; }
    public Cuenta? Cuenta { get; set; }

    public DateTime Fecha { get; set; }
    public decimal Monto { get; set; }
    public string Concepto { get; set; } = "";
}
