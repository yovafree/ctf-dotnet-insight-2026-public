using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CardamomoPay.Api.Domain;
using Microsoft.IdentityModel.Tokens;

namespace CardamomoPay.Api.Seguridad;

/// <summary>Opciones del JWT, enlazadas desde la sección "Jwt" de la configuración.</summary>
public class OpcionesJwt
{
    public string Secreto { get; set; } = "";
    public string Issuer { get; set; } = "CardamomoPay";
    public string Audience { get; set; } = "CardamomoPay";
    public int MinutosVigencia { get; set; } = 60;
}

/// <summary>Emite los JWT de inicio de sesión (HMAC-SHA256, con expiración).</summary>
public class ServicioToken(OpcionesJwt opciones)
{
    public string Emitir(Socio socio)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, socio.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.Email, socio.Correo),
            new Claim("nombre", socio.Nombre),
            new Claim(ClaimTypes.Role, socio.Rol),
        };

        var clave = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(opciones.Secreto));
        var credenciales = new SigningCredentials(clave, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: opciones.Issuer,
            audience: opciones.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(opciones.MinutosVigencia),
            signingCredentials: credenciales);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}
