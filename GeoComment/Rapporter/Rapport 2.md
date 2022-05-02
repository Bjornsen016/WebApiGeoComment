# Rapport 2

## Versionering - Lösning för databasen

Jag gjorde så att modellen för kommentarer kan ha en Author, men den har också en AuthorName property. Så om kommentaren kommer från en registrerad användare (via v0.2) så kopplas kommentaren med den användaren och AuthorName får användarens UserName. Om kommentaren inte är kopplad till en användare (v0.1) så är Author null men AuthorName är namnet från den som postade via 0.1.

Ett annat sätt att lösa det på hade kunnat vara: ...


## Versionering - parameter

URL parameter
Queryparameter

## Berhörighetskontroll

När man ska hantera information specifik för en användare så är det bra med behörighetskontroll. Ex. vid uppdatering av personlig info, eller när vi tar bort eller lägger till en post av något slag (i vår API en kommentar till exempel)

När man inte vill ha behörighetskontroll - Vid inloggning och registrering av en ny användare. För att se "gratis" information som vi valt att man ska kunna se utan att vara användare.

Kim Björnsen Åklint