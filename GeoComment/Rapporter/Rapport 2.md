# Rapport 2

## Versionering - Lösning för databasen

Jag gjorde så att modellen för kommentarer kan ha en Author, men den har också en AuthorName property. Så om kommentaren kommer från en registrerad användare (via v0.2) så kopplas kommentaren med den användaren och AuthorName får användarens UserName. Om kommentaren inte är kopplad till en användare (v0.1) så är Author null men AuthorName är namnet från den som postade via 0.1.

Ett annat sätt att lösa det på hade kunnat vara att lägga till en property som innehåller version och sen koppla på fler modeller till en kommentar beroende på vilken version. Ett problem som kan uppstå med detta sättet är att det skulle kunna bli väldigt många modeller, det kan också blir väldigt krångligt att skriva kodan som ska hämta ut informationen från databasen.

## Versionering - parameter

Ett annat sätt att versionera istället för med en query parameter är att hade det som en URL parameter. (ex. /api/{version}/comment). För det här projektet fungerar queryparameter, men jag skulle nog föredra att ha det i URLen då det:

1. Blir tydligt när man ser URLen.
2. Är lätt att arbeta med både för utvecklare och användare.
3. Det är, från vad jag sett, ett etablerat sätt som används mycket av.
4. Jag själv vill anse att query parametrar bör användas för filtrering av data. Och versionering borde inte passa in där i de flesta fall.

Vad som kan vara en nackdel jämfört med query parameter är att det kan vara krångligare att koda endpoints som inte bryr sig om versionen. Har dock inte testat tillräckligt för att säga om det är sant eller inte.

En annan metod man kunnat använda sig av är att ha sin versionering i headern på requesten.

## Berhörighetskontroll

När man ska hantera information specifik för en användare så är det bra med behörighetskontroll. Ex. vid uppdatering av personlig info, eller när vi tar bort eller lägger till en post av något slag (i vår API en kommentar till exempel)

När man inte vill ha behörighetskontroll - Vid inloggning och registrering av en ny användare. För att se "gratis" information som vi valt att man ska kunna se utan att vara användare.

> Kim Björnsen Åklint, Maj 2022
