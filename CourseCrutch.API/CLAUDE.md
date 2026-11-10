# CourseCrutch.API

ASP.NET Core web API. Hosts HTTP endpoints for the MAUI client.

## Rules

- Use minimal APIs (`app.MapGet`, `app.MapPost`, and so on). Do not add MVC controllers unless asked.
- Name each endpoint with `.WithName(...)`, matching the `Get`/`Post` verb plus the resource.
- Endpoints call Application-layer services. Do not put business logic in `Program.cs`.
- Return typed results (`Results.Ok`, `TypedResults.*`) and document them for OpenAPI.
- OpenAPI is mapped in Development only (`app.MapOpenApi()`). Keep that unless asked.
- Configuration goes in `appsettings*.json`. Secrets do not.

## Current state

- `POST /api/ocr/analyse` (`AnalyseOcrText`) takes `{ text }` and calls `IOcrAnalysisService`. Empty text returns 400.
- `Contracts/OcrContracts.cs` holds request and response records. Keep them in step with `CourseCrutch.Clients/Models/OcrContracts.cs`.
- The template weather endpoint has been removed.
- HTTPS redirection runs outside Development only. Development serves `http://localhost:5059` so the Android emulator can call it.

## Files

- `CourseCrutch.API.http`: manual request samples. Update when endpoints change.
- `Properties/launchSettings.json`: local launch profiles. Do not change ports without asking.
