Tytuł: TheFlights API

Opis: TheFlights to działające API do zarządzania lotami, wzbogacone o system rejestracji i logowania. Po zalogowaniu, w odpowiedzi otrzymasz token JWT, który jest wymagany do zarządzania lotami.

Instrukcje instalacji:

1. Sklonuj repozytorium.
2. Uruchom migracje, aby zainicjować bazę danych. Projekt wymaga SQL Server i SQL Server Management Studio.
3. Otwórz projekt w środowisku Visual Studio i uruchom aplikację.

Instrukcje użytkowania:
Aby skorzystać z API, utwórz konto za pomocą żądania POST na endpoint /register.
Zaloguj się za pomocą żądania POST na endpoint /login. Po zalogowaniu zostanie wygenerowany token JWT.
Używaj tokena JWT w nagłówku Authorization (Bearer token) do autoryzacji żądań API.
Endpointy API
GET /flights: Wyświetla wszystkie loty.
GET /flights/{id}: Wyświetla lot o określonym ID.
POST /flights: Dodaje nowy lot. W ciele żądania należy podać dane w formacie JSON.
PUT /flights/{id}: Modyfikuje istniejący lot o określonym ID.
DELETE /flights/{id}: Usuwa lot o określonym ID.

Przykładowe formaty danych:

json
// POST /flights
{
"flightNumber": "string",
"departureDate": "2024-04-22T19:36:32.125Z",
"departurePoint": "string",
"arrivalPoint": "string",
"aircraftType": "string"
}
// PUT /flights/{id}
{
"flightNumber": "string",
"departureDate": "2024-04-22T19:38:14.326Z",
"departurePoint": "string",
"arrivalPoint": "string",
"aircraftType": "string"
}
Wymagania:
1.SQL Server
2.SQL Server Management Studio
3.Visual Studio

Michał Rychert - DjDecay21
