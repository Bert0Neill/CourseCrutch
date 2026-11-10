# CourseCrutch: Solution Standards

Solution-wide rules for Claude. Project-specific rules live in each project's `CLAUDE.md`. Where they conflict, the project file wins.

## Stack

- .NET 10 (`net10.0`) across all projects. Do not change the target framework without asking.
- `Nullable` and `ImplicitUsings` enabled in every project.
- Web API: ASP.NET Core minimal APIs with OpenAPI (`Microsoft.AspNetCore.OpenApi`).
- Client: .NET MAUI with Blazor Hybrid (`Microsoft.AspNetCore.Components.WebView.Maui`).
- Tests: xUnit with coverlet.

## Layers and dependencies

Dependencies point inward. Do not add a reference that breaks this order.

| Project | May reference |
|---|---|
| `CourseCrutch.Domain` | nothing |
| `CourseCrutch.Application` | Domain |
| `CourseCrutch.Infrastructure` | Application, Domain |
| `CourseCrutch.API` | Application, Infrastructure |
| `CourseCrutch.Clients` | nothing in the solution; calls the API over HTTP |
| `CourseCrutch.Tests` | any project, for testing |

Current state: the Domain, Application and Infrastructure projects contain only placeholder `Class1.cs` files, and the Application and Infrastructure projects have no project references yet.

## Coding conventions

- Follow the existing file's style, naming and comment density. Do not add comments that restate the code.
- PascalCase for types, methods and properties. camelCase for locals and parameters. `_camelCase` for private fields.
- Prefer `async`/`await` with `CancellationToken` on any I/O-bound method.
- Use `file`-scoped namespaces in new files, as the template does.
- Nullable reference types are on. Do not suppress warnings with `!` or `#pragma` without a comment explaining why.
- No `Console.WriteLine` in library code. Use `ILogger<T>`.

## Testing

- Every new Application or Domain behaviour gets an xUnit test in `CourseCrutch.Tests`.
- Run `dotnet build` and `dotnet test` before reporting work as done. Report failures with the output.

## Git

- Branch from `master`. Do not commit directly to `master` for feature work.
- Commit messages: imperative mood, subject under 72 characters.
- Do not commit `bin/`, `obj/`, `.vs/` or `*.csproj.user`. The `.gitignore` covers these.

## Claude configuration

- The root `.claude/` folder holds skills, hooks and commands shared across the solution.
- Each project's `.claude/` folder holds skills, hooks and commands specific to that project.
- Hooks are not active yet. Do not add a `settings.json` with hooks without confirming first.
