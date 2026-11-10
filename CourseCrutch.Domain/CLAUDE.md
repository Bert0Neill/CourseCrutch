# CourseCrutch.Domain

Core business entities, value objects and domain rules. No dependencies on other solution projects or on infrastructure packages.

## Rules

- No references to other CourseCrutch projects or to EF Core, ASP.NET Core or MAUI.
- Model invariants in the types themselves. Prefer constructors and factory methods that refuse invalid state over setters with validation.
- Prefer `record` or `readonly record struct` for value objects.
- Keep entities free of persistence concerns: no attributes, no navigation-property plumbing for an ORM.
- No I/O, no `DateTime.Now` or `Random`. Pass time and randomness in, so tests stay deterministic.

## Current state

Only a placeholder `Class1.cs` exists. Remove it when the first entity is added.
