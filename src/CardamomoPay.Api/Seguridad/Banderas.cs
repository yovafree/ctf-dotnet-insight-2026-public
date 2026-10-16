namespace CardamomoPay.Api.Seguridad;

/// <summary>
/// Valores que entrega la API en ciertos endpoints. Se inyectan por variable de
/// entorno en el despliegue (ej. Banderas__Cuenta=...) y no se escriben en el código.
/// </summary>
public class Banderas
{
    public string Cuenta { get; set; } = "FLAG{demo}";
    public string Panel { get; set; } = "FLAG{demo}";
    public string Reporte { get; set; } = "FLAG{demo}";
    public string Boveda { get; set; } = "FLAG{demo}";
    public string Perfil { get; set; } = "FLAG{demo}";
    public string Transferencia { get; set; } = "FLAG{demo}";
    public string Foto { get; set; } = "FLAG{demo}";
}
