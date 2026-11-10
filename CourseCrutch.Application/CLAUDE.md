# CourseCrutch.Application

Use cases and application services. Orchestrates Domain objects. Defines the interfaces that Infrastructure implements.

## Rules

- Depends only on `CourseCrutch.Domain`. Do not reference Infrastructure, API or EF Core types here.
- Define repository and external-service interfaces here. Implement them in Infrastructure.
- One use case per class or method. Name them as verbs: `EnrolStudent`, `GetCourseCatalogue`.
- Accept `CancellationToken` on all async methods.
- Return a result type for expected failures. Reserve exceptions for unexpected faults.
- Use `Microsoft.Extensions.*` abstractions (for example `ILogger<T>`), not concrete implementations.

## Current state

Only a placeholder `Class1.cs` exists. Remove it when the first real type is added.
