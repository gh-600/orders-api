# Lab 1 · Gobernanza vía PR

Kit para el Lab 1 del plan GH-600: plantilla de PR, CI, Plan Gate, CODEOWNERS y ruleset sobre `main`.

## Contenido

| Fichero | Para qué |
|---|---|
| `bootstrap.sh` | Crea la solución .NET (`Orders.sln`, 4 proyectos + tests) |
| `.github/pull_request_template.md` | Da forma al plan y la evidencia |
| `.github/workflows/ci.yml` | Checks `build` y `test` |
| `.github/workflows/plan-gate.yml` | Check `require-plan`: valida la descripción del PR contra la plantilla de la rama base |
| `.github/CODEOWNERS` | Enruta revisiones por ruta (`*` primero) |
| `ruleset-main.json` | Ruleset de `main` para crear por API |
| `exercises/` | Ficheros de las trampas (no se ejecutan desde ahí) |

## Antes de empezar: dos cuentas

GitHub no te deja aprobar tus propios PR. Con «Require review from Code Owners» y una sola cuenta, los PR que tocan rutas de otro owner se quedan bloqueados. Crea una **segunda cuenta** gratuita, dale acceso de escritura al repo y úsala como `@TU-REVISOR` en `CODEOWNERS`.

## Pasos

1. Crea el repo `orders-api` en tu organización de pruebas y clónalo.
2. Copia el contenido de este kit en la raíz y ejecuta:
   ```bash
   chmod +x bootstrap.sh && ./bootstrap.sh
   ```
3. Sustituye `@TU-USUARIO` y `@TU-REVISOR` en `.github/CODEOWNERS`.
4. Commit y push a `main` (todavía sin ruleset).
5. Crea el ruleset, por la interfaz o por API:
   - **Interfaz:** Settings → Rules → Rulesets → *New branch ruleset* → rama por defecto → *Require a pull request* (0 aprobaciones + *Require review from Code Owners*) → *Require status checks to pass* con `build`, `test` y `require-plan` (fuente: GitHub Actions) → *Block force pushes* y *Restrict deletions* → *Active*.
   - **API:**
     ```bash
     gh api repos/OWNER/orders-api/rulesets --method POST --input ruleset-main.json
     ```
     Después, en la interfaz, fija **GitHub Actions** como fuente de cada check (el JSON no lo incluye porque no hemos verificado el `integration_id`).
6. Comprueba que la lista de *bypass* está vacía.

> Los checks aparecen en el selector del ruleset después de su primera ejecución. Si no salen, abre un PR de prueba primero.

## Comprobaciones

- [ ] **PR sin plan:** abre un PR cuya descripción no tenga `## Plan` → `require-plan` en rojo, merge bloqueado. Edita la descripción → el check se reevalúa y pasa.
- [ ] **CODEOWNERS:** un PR que toca `infra/main.bicep` exige la aprobación de `@TU-REVISOR`.
- [ ] **Orden de CODEOWNERS:** mueve `*` al final del fichero en un PR y observa que todas las rutas pasan a ese owner.
- [ ] **Trampa del nombre:** en una rama, renombra el job `test` a `unit-tests` → el PR queda en *Expected — Waiting for status*. Corrígelo actualizando el ruleset.
- [ ] **Trampa del skipped:** copia `exercises/trampa-skipped.yml` sobre `plan-gate.yml` en una rama `feature/x` → `require-plan` sale *skipped* y **no** bloquea.
- [ ] **Corrección:** usa `exercises/gate-agregador.yml`, exige solo `required-gate` en el ruleset y repite → ahora sí bloquea.

## Qué refuerza para el examen

- La plantilla **da forma**; el check obligatorio **hace cumplir**; CODEOWNERS **enruta**; el ruleset **bloquea**.
- Un check `skipped` cuenta como aprobado; un check que no se publica deja el PR pendiente.
- El nombre del check es el del job (o su `name:`): renombrar exige actualizar el ruleset.
