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

`Program.cs` still holds the template `weatherforecast` endpoint and `WeatherForecast` record. Replace it when real endpoints are added.

## Files

- `CourseCrutch.API.http`: manual request samples. Update when endpoints change.
- `Properties/launchSettings.json`: local launch profiles. Do not change ports without asking.
