# Guía de retos — pistas y recomendaciones

Cada reto cuenta una historia. Aquí tienes el **escenario** y varias **pistas
progresivas**: empieza por la primera y solo baja a la siguiente si te atoras. Descubrir
el "cómo" es parte del juego.

---

## Antes de empezar: recomendaciones generales

Una metodología que sirve para **todos** los retos de APIs:

1. **Inicia sesión y guarda tu token.** Casi todo requiere el encabezado
   `Authorization: Bearer <token>` (ver [README.md](README.md)).
2. **Explora primero.** Abre la documentación interactiva en `/scalar/v1` y haz un mapa
   mental de los endpoints: cuáles hay, qué datos piden y qué devuelven.
3. **Observa cada petición y cada respuesta completa.** Fíjate en:
   - Los **datos que viajan** en la URL, en el cuerpo (JSON) y en el token.
   - El **código de estado** (200, 401, 403, 500…) y los **mensajes de error**.
   - Campos de la respuesta que no esperabas.
4. **Cambia una cosa a la vez.** Modifica un identificador, agrega un campo, repite con
   otro valor. Compara qué cambia en la respuesta.
5. **Piensa como la aplicación.** Pregúntate: *¿qué está confiando el servidor que
   debería estar verificando?* Ahí suelen estar las fallas de las APIs.

**Herramientas útiles:** `curl`, Postman o Insomnia; la pestaña **Red (Network)** de las
herramientas de desarrollador del navegador (F12); [jwt.io](https://jwt.io) para leer el
contenido de un token; y un editor para automatizar peticiones repetidas.

> La bandera siempre tiene el formato `FLAG{...}`. Cuando la obtengas, envíala en el
> reto correspondiente en CTFd.

---

## 1. El saldo del vecino · 100 pts

**Escenario.** Cada socio puede consultar su propia cuenta en la app. Doña Yolanda, de
Cobán, presume ser la socia con más ahorros. ¿Podrás confirmar su saldo sin ser ella?

**Primeros pasos.** Consulta tu propia cuenta y observa bien la URL de la petición.

**Si te atoras.** Hay un número que identifica *qué* cuenta pides. ¿Qué pasa si lo
cambias por otro? ¿El servidor comprueba que esa cuenta es tuya?

**Pista fuerte.** Prueba a pedir cuentas con identificadores distintos al tuyo. La de
mayor saldo esconde algo.

**En el código.** Mira el manejador `GET /{id}` en
[src/CardamomoPay.Api/Endpoints/EndpointsCuentas.cs](src/CardamomoPay.Api/Endpoints/EndpointsCuentas.cs#L16):
busca la cuenta por el id de la URL, pero fíjate en qué **no** hace con `cuenta.SocioId`
([línea 22](src/CardamomoPay.Api/Endpoints/EndpointsCuentas.cs#L22)).

> 📘 Este reto está **resuelto paso a paso** como ejemplo en
> [EJEMPLO-RESUELTO-RETO-1.md](EJEMPLO-RESUELTO-RETO-1.md). Úsalo para aprender la
> mecánica que necesitarás en los demás.

---

## 2. Puerta trasera de administración · 100 pts

**Escenario.** La cooperativa tiene un panel que solo deberían ver los administradores.
Tú eres un socio común... ¿o el panel no distingue bien quién entra?

**Primeros pasos.** Busca en la documentación (`/scalar/v1`) qué rutas de administración
existen. Normalmente viven bajo una ruta que lo deja claro.

**Si te atoras.** Intenta llamar esas rutas de administración **con tu token de socio
normal**, tal cual. No asumas que te las van a bloquear: compruébalo.

**Pista fuerte.** El panel de administración no verifica tu **rol**, solo que estés
autenticado. ¿Qué devuelve si entras igual?

**En el código.** Compara cómo se protege este grupo de rutas en
[src/CardamomoPay.Api/Endpoints/EndpointsAdmin.cs](src/CardamomoPay.Api/Endpoints/EndpointsAdmin.cs#L13):
observa qué le pasa (o no le pasa) a `.RequireAuthorization(...)`
([línea 15](src/CardamomoPay.Api/Endpoints/EndpointsAdmin.cs#L15)) comparado con lo que
necesitaría para exigir un rol.

**Qué estás aprendiendo.** Que autenticarse ("quién eres") no es lo mismo que autorizar
("qué puedes hacer"). Muchas APIs fallan justo ahí.

---

## 3. El error que habla de más · 100 pts

**Escenario.** El módulo de reportes es viejo y a veces falla. Cuando algo sale mal, el
sistema cuenta más de lo que debería.

**Primeros pasos.** Encuentra el endpoint de reportes y llámalo. Puede que no funcione
como esperas: eso es justo lo que buscas.

**Si te atoras.** No te quedes con el "algo salió mal": **lee la respuesta completa**,
de arriba a abajo. Un error mal gestionado puede incluir detalles técnicos internos.

**Pista fuerte.** La respuesta de error incluye información de conexión que no debería
ser pública. La bandera está escondida ahí dentro.

**En el código.** El manejador que falla está en
[src/CardamomoPay.Api/Endpoints/EndpointsReportes.cs](src/CardamomoPay.Api/Endpoints/EndpointsReportes.cs#L18);
y por qué el error se muestra con todo detalle, en
[Program.cs](src/CardamomoPay.Api/Program.cs#L66) (`UseDeveloperExceptionPage`).

**Qué estás aprendiendo.** Que exponer errores detallados en producción (stack traces,
cadenas de conexión, secretos) es una mala configuración muy común.

---

## 4. Token de confianza · 200 pts

**Escenario.** La bóveda interna de CardamomoPay solo se abre para procesos "internos".
El guardia revisa tu gafete (tu token), pero ¿de verdad lo revisa bien?

**Primeros pasos.** Llama al endpoint de la bóveda con tu token normal. Lee bien el
mensaje de rechazo: te dice **qué le falta** a tu token.

**Si te atoras.** Un token JWT tiene tres partes separadas por puntos
(`cabecera.datos.firma`); las dos primeras son solo texto codificado en Base64 que
puedes leer en [jwt.io](https://jwt.io). Pregúntate: ¿el servidor **verifica la firma**
del token, o se cree lo que diga el contenido?

**Pista fuerte.** La bóveda pide un campo (*claim*) que tu token no trae. Si el servidor
no valida la firma, puedes **construir tu propio token** con ese campo (incluso con el
algoritmo `none`, que significa "sin firma").

**En el código.** Revisa cómo se valida el token en
[Program.cs](src/CardamomoPay.Api/Program.cs#L38) (fíjate en los `Validate...` y en el
`SignatureValidator`), y qué exige la bóveda en
[src/CardamomoPay.Api/Endpoints/EndpointsBoveda.cs](src/CardamomoPay.Api/Endpoints/EndpointsBoveda.cs#L15).

**Qué estás aprendiendo.** Que un JWT solo es de fiar si el servidor valida su firma,
su expiración y su origen. Si no, cualquiera lo falsifica.

---

## 5. Ascenso no autorizado · 200 pts

**Escenario.** Actualizar tu perfil parece inofensivo: nombre, municipio... Pero el
formulario quizás acepte más campos de los que muestra.

**Primeros pasos.** Actualiza tu perfil de forma normal y observa qué campos devuelve la
respuesta sobre ti (incluido tu "rol" o tipo de usuario).

**Si te atoras.** La pantalla te deja cambiar nombre y municipio, pero ¿y si en el JSON
de la petición **añades a mano** uno de esos otros campos que la respuesta sí muestra?

**Pista fuerte.** Intenta enviar el campo que define tu rol/privilegios con un valor
elevado. Si el servidor lo acepta sin filtrar, habrás escalado.

**En el código.** Mira qué campos acepta el contrato en
[src/CardamomoPay.Api/Contratos/Dtos.cs](src/CardamomoPay.Api/Contratos/Dtos.cs#L5)
(`ActualizarPerfilRequest`) y cómo se aplican al guardar en
[EndpointsPerfil.cs](src/CardamomoPay.Api/Endpoints/EndpointsPerfil.cs#L34).

**Qué estás aprendiendo.** Que una API nunca debe confiar en todos los campos que manda
el cliente ("mass assignment"): debe aceptar solo los permitidos.

---

## 6. PIN sin límites · 200 pts

**Escenario.** Toda transferencia se confirma con un PIN corto. El sistema no parece
cansarse de que te equivoques.

**Primeros pasos.** Encuentra el endpoint que confirma una transferencia con un PIN.
Pruébalo con un PIN cualquiera y mira qué responde cuando es incorrecto.

**Si te atoras.** El PIN es muy corto (pocos dígitos) y el servidor **no limita los
intentos**. ¿Cuántas combinaciones posibles hay realmente?

**Pista fuerte.** Son solo unas cuantas centenas de combinaciones. Escribe un pequeño
bucle (o usa una herramienta como Burp Intruder) que las pruebe todas hasta que una
responda distinto. Eso se llama **fuerza bruta**.

**Recomendación.** Un bucle en la terminal con `curl` basta; prueba desde `000` hasta el
máximo y detente cuando el código de respuesta cambie a éxito.

**En el código.** El manejador que valida el PIN está en
[src/CardamomoPay.Api/Endpoints/EndpointsTransferencias.cs](src/CardamomoPay.Api/Endpoints/EndpointsTransferencias.cs#L16):
fíjate en que no hay nada que cuente ni limite los intentos fallidos.

**Qué estás aprendiendo.** Que los secretos cortos sin límite de intentos ni bloqueo se
rompen en segundos. La defensa es *rate limiting* y bloqueo tras varios fallos.

---

## 7. Mirando hacia adentro · 300 pts *(extra)*

**Escenario.** La app te deja importar tu foto de perfil desde una URL: le pasas un
enlace y el servidor lo descarga por ti. Pero el servidor vive en una red con vecinos
que tú no ves desde afuera.

**Primeros pasos.** Usa la función de importar foto con una URL pública cualquiera y
observa qué te devuelve el servidor (¿te muestra el contenido que descargó?).

**Si te atoras.** Si el servidor descarga **cualquier** URL que le des, no tiene por qué
limitarse a Internet. ¿Qué pasaría si le pides una dirección **interna** de su propia
red, a la que tú no llegas desde tu equipo?

**Pista fuerte.** Junto a la API vive otro servicio interno. Prueba a pedirle al servidor
que descargue una dirección interna (por nombre de servicio o `localhost`/`127.0.0.1`) en
distintos puertos y rutas comunes. El contenido que traiga puede ser tu bandera.

**En el código.** Mira el manejador de importar foto en
[src/CardamomoPay.Api/Endpoints/EndpointsFoto.cs](src/CardamomoPay.Api/Endpoints/EndpointsFoto.cs#L22):
toma tu URL y la descarga tal cual, sin comprobar a dónde apunta.

**Qué estás aprendiendo.** Esto se llama **SSRF**: obligar al servidor a hacer peticiones
en tu nombre hacia destinos internos. La defensa es validar y restringir a dónde puede
conectarse.
