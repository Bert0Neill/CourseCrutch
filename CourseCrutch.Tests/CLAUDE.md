# CourseCrutch.Tests

xUnit test project for all layers. Test against public behaviour, not private implementation.

## Rules

- Name test classes `<ClassUnderTest>Tests`. Name methods `Method_Scenario_ExpectedResult`.
- One behaviour per test. Use `[Theory]` with `[InlineData]` for input variations.
- Use `Assert.*` from xUnit. Do not add FluentAssertions or other assertion libraries unless asked.
- Keep tests deterministic. Do not depend on wall-clock time, random values or test order.
- Put test doubles in a `Fakes/` or `TestDoubles/` folder. Prefer hand-written fakes for simple interfaces. Use a mocking library only when a fake is impractical.
- Coverage: run with `dotnet test --collect:"XPlat Code Coverage"`.

## Current state

- References `CourseCrutch.Logging` only.
- `Logging/LogFileNamerTests.cs` and `Logging/FileLoggerProviderTests.cs` cover log file naming, same-day suffixes and level filtering. They write to a unique temp folder per test and delete it afterwards.
- `CourseCrutch.Logging` exposes internals to this project through `InternalsVisibleTo`.
