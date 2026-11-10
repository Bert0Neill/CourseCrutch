# CourseCrutch.Infrastructure

Implementations of interfaces defined in Application: persistence, external services, and other I/O.

## Rules

- Implement interfaces from `CourseCrutch.Application`. Do not expose infrastructure types to Application or Domain.
- Register implementations through an extension method such as `AddInfrastructure(this IServiceCollection, IConfiguration)`. API calls it from `Program.cs`.
- Read connection strings and secrets from configuration. Never hard-code them.
- Keep database migrations and data-access code in this project. Do not put them in API.
- Dispose and cancel properly: pass `CancellationToken` through every async call.

## Current state

Only a placeholder `Class1.cs` exists. Application and Domain are not yet referenced. Add those references when the first implementation needs them.
