# Hackea esta API — CTF de seguridad (OWASP API Top 10)

Bienvenido/a al taller **"Hackea esta API"**. Durante la sesión, tu equipo atacará una
API intencionalmente vulnerable — **CardamomoPay**, la billetera digital de una
cooperativa ficticia de Alta Verapaz — y capturará banderas explotando fallas reales
del [OWASP API Security Top 10 (2023)](https://owasp.org/API-Security/editions/2023/en/0x00-header/).

Este repositorio contiene **las instrucciones, las pistas y el código fuente** de la API
(en [src/](src/)). Puedes atacarla como caja negra contra la instancia de tu equipo, o
leer el código para entender qué ocurre por dentro.

> ⚠️ Todo ocurre en un entorno controlado creado solo para el taller. Ataca
> **únicamente la instancia de tu equipo**.

## Cómo se juega

1. Tu equipo recibe una **hoja de acceso** con:
   - La **URL de la API de tu equipo** (p. ej. `http://<IP>:8001`).
   - Tu **usuario y contraseña** del tablero CTFd.
2. Entra al tablero **CTFd** y abre los retos.
3. Resuelve cada reto atacando la API y obtén una bandera con formato `FLAG{...}`.
4. Envía la bandera en CTFd para sumar puntos.

## Tu acceso a la API

Inicias sesión en la API como un socio de la cooperativa:

- **Correo:** `equipo@cardamomopay.gt`
- **Contraseña:** `cardamomo`

```bash
# Obtén tu token JWT
curl -s -X POST http://<IP>:<PUERTO>/api/v2/auth/login \
  -H "Content-Type: application/json" \
  -d '{"correo":"equipo@cardamomopay.gt","contrasena":"cardamomo"}'
```

Usa ese token en el resto de peticiones: `Authorization: Bearer <token>`.

La **documentación interactiva** de la API (todos los endpoints, para probarlos) está en
la raíz de tu instancia: `http://<IP>:<PUERTO>/scalar/v1`.

## ¿Primera vez? Empieza por el ejemplo resuelto

Resolvemos el **reto 1 paso a paso** para que aprendas la mecánica (login, token,
explorar endpoints): **[EJEMPLO-RESUELTO-RETO-1.md](EJEMPLO-RESUELTO-RETO-1.md)**.
Los demás retos son tuyos.

## Herramientas sugeridas

- `curl`, Postman o Insomnia.
- La documentación interactiva (Scalar) de tu instancia.
- Para el reto de fuerza bruta: un script propio o Burp Suite.

## Los retos y sus pistas

👉 Las pistas generales de cada reto están en **[GUIA-RETOS.md](GUIA-RETOS.md)**.

| Reto | Puntos |
|---|---|
| El saldo del vecino | 100 |
| Puerta trasera de administración | 100 |
| El error que habla de más | 100 |
| Token de confianza | 200 |
| Ascenso no autorizado | 200 |
| PIN sin límites | 200 |
| Mirando hacia adentro *(extra)* | 300 |

## Correr la API en tu máquina (opcional)

Si quieres levantar tu propia copia para experimentar:

```bash
cd src/CardamomoPay.Api
dotnet run
```

Necesitas el SDK de .NET 10. La API queda en `http://localhost:5041` y su documentación
en `http://localhost:5041/scalar/v1`. Las banderas de tu copia local son de ejemplo
(`FLAG{demo}`); las reales viven solo en las instancias del taller.

## Reglas

- Ataca **solo la instancia de tu equipo**. No ataques el tablero CTFd ni a otros equipos.
- No necesitas tumbar el servicio: todas las banderas salen de la lógica de la API.
- Pregunta al facilitador cuando te atores. El objetivo es aprender.
