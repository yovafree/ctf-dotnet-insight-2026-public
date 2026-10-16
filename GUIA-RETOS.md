# Guía de retos — pistas generales

Cada reto cuenta una historia. Aquí tienes el **escenario** y una **pista general** para
orientarte. No te damos el paso a paso: descubrir el "cómo" es parte del juego.

Recuerda: primero inicia sesión en la API de tu equipo para obtener tu token
(ver [README.md](README.md)) y explora los endpoints en `/scalar/v1`.

---

## 1. El saldo del vecino · 100 pts

**Escenario.** Cada socio puede consultar su propia cuenta en la app. Doña Yolanda, de
Cobán, presume ser la socia con más ahorros. ¿Podrás confirmar su saldo sin ser ella?

**Pista.** Cuando consultas una cuenta, observa qué dato identifica a esa cuenta en la
petición. ¿El servidor comprueba que esa cuenta es tuya?

> 📘 Este reto está **resuelto paso a paso** como ejemplo en
> [EJEMPLO-RESUELTO-RETO-1.md](EJEMPLO-RESUELTO-RETO-1.md).

---

## 2. Puerta trasera de administración · 100 pts

**Escenario.** La cooperativa tiene un panel que solo deberían ver los administradores.
Tú eres un socio común... ¿o el panel no distingue bien quién entra?

**Pista.** Explora qué rutas de administración existen e intenta alcanzarlas con tu
token de socio normal. Fíjate si de verdad te exigen ser administrador.

---

## 3. El error que habla de más · 100 pts

**Escenario.** El módulo de reportes es viejo y a veces falla. Cuando algo sale mal, el
sistema cuenta más de lo que debería.

**Pista.** Provoca un error en el módulo de reportes y **lee con atención** el mensaje
completo que devuelve el servidor. A veces los errores revelan secretos.

---

## 4. Token de confianza · 200 pts

**Escenario.** La bóveda interna de CardamomoPay solo se abre para procesos "internos".
El guardia revisa tu gafete (tu token), pero ¿de verdad lo revisa bien?

**Pista.** Mira qué le falta a tu token para que la bóveda te deje entrar. Luego
pregúntate: ¿el servidor verifica realmente que el token sea auténtico, o confía en lo
que diga? Investiga cómo está compuesto un JWT.

---

## 5. Ascenso no autorizado · 200 pts

**Escenario.** Actualizar tu perfil parece inofensivo: nombre, municipio... Pero el
formulario quizás acepte más campos de los que muestra.

**Pista.** Cuando guardas tu perfil, prueba a enviar en la petición algún campo que la
pantalla no muestra. ¿Qué pasaría si pudieras cambiar algo que no deberías?

---

## 6. PIN sin límites · 200 pts

**Escenario.** Toda transferencia se confirma con un PIN corto. El sistema no parece
cansarse de que te equivoques.

**Pista.** El PIN es corto y no hay límite de intentos. Si no te sabes el PIN... pero
pudieras probarlos todos muy rápido, ¿cuánto tardarías? Automatiza.

---

## 7. Mirando hacia adentro · 300 pts *(extra)*

**Escenario.** La app te deja importar tu foto de perfil desde una URL: le pasas un
enlace y el servidor lo descarga por ti. Pero el servidor vive en una red con vecinos
que tú no ves desde afuera.

**Pista.** Si el servidor descarga cualquier URL que le des, ¿qué pasaría si le pides
una dirección **interna** en vez de una pública? Piensa qué servicios podrían vivir
junto a la API.
