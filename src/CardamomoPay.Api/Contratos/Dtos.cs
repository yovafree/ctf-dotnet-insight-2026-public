namespace CardamomoPay.Api.Contratos;

// Peticiones
public record LoginRequest(string Correo, string Contrasena);
public record ActualizarPerfilRequest(string Nombre, string Municipio, string? Rol = null);
public record TransferenciaRequest(string CuentaDestino, decimal Monto, string Pin, string Concepto);
public record ImportarFotoRequest(string Url);

// Respuestas (DTOs explícitos: nunca se devuelve la entidad del dominio directamente)
public record LoginResponse(string Token, string Nombre, string Rol);
public record SocioDto(int Id, string Nombre, string Correo, string Municipio, string Rol);
public record CuentaDto(int Id, string Numero, decimal Saldo, string? Bandera = null);
public record MovimientoDto(DateTime Fecha, decimal Monto, string Concepto);
