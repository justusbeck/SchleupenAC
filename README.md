# Personenverwaltung

Anwendung zur Verwaltung von Personendaten mit Anschriften und Telefonnummern.
WPF-Oberfläche, REST/JSON-Webservice und SQL-Server-Datenbank in einer 3-Schichten-Architektur auf Basis von .NET Framework 4.8.

## Architektur

| Projekt | Schicht | Aufgabe |
|---|---|---|
| `Personenverwaltung.Client` | Präsentation | WPF-Oberfläche |
| `Personenverwaltung.Api` | Anwendungslogik | REST/JSON-Webservice (ASP.NET Web API 2, selbst gehostet per OWIN) |
| `Personenverwaltung.Logic` | Anwendungslogik | Suchen und Speichern von Personen |
| `Personenverwaltung.Data` | Datenhaltung | Zugriff auf SQL Server mit Entity Framework 6 |

## Voraussetzungen

- .NET Framework 4.8
- [.NET SDK](https://dotnet.microsoft.com/download) (aktuelle Version, zum Bauen)
- Microsoft SQL Server und SQL Server Management Studio
- optional: JetBrains Rider oder Visual Studio

## Einrichtung

### 1. Datenbank anlegen

Das SQL-Skript im Ordner `Database` in SQL Server Management Studio öffnen und ausführen.
Es legt die Datenbank mit den Tabellen `Person`, `Anschrift` und `Telefonverbindung` an und fügt Testdaten ein.

### 2. Verbindung zur Datenbank eintragen

Die Datei `Personenverwaltung.Api/App.config` enthält bereits einen Connection String. Hier ist der Servername und der Datenbankname anzugeben:

`connectionString="Data Source=[Server-Name];Initial Catalog=[Datenbank-Name];Integrated Security=True;TrustServerCertificate=True"`

### 3. Bauen

```bash
dotnet build
```

## Starten

Die API und den Client in der Reihenfolge mit den folgenden Befehlen starten.

```bash
dotnet run --project Personenverwaltung.Api
```

Die Konsole zeigt `API läuft auf http://localhost:5050/`.

```bash
dotnet run --project Personenverwaltung.Client
```

Das Hauptfenster der Anwendung öffnet sich.

**Kurztest der API:** Im Browser `http://localhost:5050/api/personen` öffnen. Es erscheinen die Personen als JSON.

## Bedienung

| Aktion | Bedienung |
|---|---|
| Personen laden | Schaltfläche **Personen laden** |
| Nach Namen suchen | Namen oder Namensteil ins Suchfeld eingeben, dann **Personen laden** |
| Details anzeigen | Person in der Tabelle anklicken, rechts öffnet sich das Detailfenster mit Anschriften und Telefonnummern |
| Namen ändern | Doppelklick auf Name oder Vorname --> ändern |
| Änderungen speichern | Schaltfläche **Änderungen speichern** zum Speichern der Änderungen in der Datenbank |
# SchleupenAC