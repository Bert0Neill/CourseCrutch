# CourseCrutch.Tests

xUnit test project for all layers. Test against public behaviour, not private implementation.

## Rules

- Name test classes `<ClassUnderTest>Tests`. Name methods `Method_Scenario_ExpectedResult`.
- One behaviour per test. Use `[Theory]` with `[InlineData]` for input variations.
- Use `Assert.*` from xUnit. Do not add FluentAssertions or other assertion libraries unless asked.
- Keep tests deterministic. Do not depend on wall-clock time, random values or test order.
- Mock dependencies with Moq. Put shared test doubles and helpers in `TestDoubles/`.
- Mock `HttpClient` calls with `TestDoubles/MockHttpHandler`. Verify log calls with `LoggerMockExtensions.VerifyLogged`.
- Test API endpoints through `WebApplicationFactory<Program>`. Replace services with `ConfigureTestServices` and set `FileLogging:Enabled` to `false` so tests do not write log files.
- Coverage: run with `dotnet test --collect:"XPlat Code Coverage"`.

## Current state

- References Logging, Application, Infrastructure and API.
- Clients targets MAUI platforms only, so the csproj compiles `OcrContracts.cs`, `IOcrApiClient.cs` and `OcrApiClient.cs` in as linked files. `OcrService` (TesseractOcrMaui) and `PhotoCaptureService` (`MediaPicker`) need MAUI and have no tests.
- `Logging/` covers log file naming, same-day suffixes and level filtering. Tests write to a unique temp folder and delete it afterwards. `CourseCrutch.Logging` exposes internals here through `InternalsVisibleTo`.
- `Infrastructure/` covers `GroqOcrAnalysisService`, `MockOcrAnalysisService` and `AddInfrastructure` service selection.
- `Api/` covers `POST /api/ocr/analyse`. `Clients/` covers `OcrApiClient`.
