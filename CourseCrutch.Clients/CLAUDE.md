# CourseCrutch.Clients

.NET MAUI app with a Blazor Hybrid UI. Targets Android, iOS, Mac Catalyst and Windows.

## Rules

- UI lives in Razor components under `Components/`. Layouts go in `Components/Layout/`, pages in `Components/Pages/`.
- Do not reference Domain, Application or Infrastructure directly. Get data from the API over HTTP.
- Register services in `MauiProgram.cs`. Do not construct HTTP clients inside components.
- Platform-specific code goes under `Platforms/<platform>/`. Do not put `#if` platform checks in shared code unless there is no other option.
- Keep CSS in `wwwroot/app.css` or a component's `.razor.css` file. MAUI's default CSS handling is disabled (`EnableDefaultCssItems` is false), so do not rely on it.
- Do not change `ApplicationId`, `ApplicationTitle` or version numbers without asking.

## Build targets

Each target requires its platform SDK. On Windows, `net10.0-windows10.0.19041.0` builds. Other targets need their platform workloads installed. Build a single target when verifying changes, for example `dotnet build -f net10.0-windows10.0.19041.0`.

## Current state

The app still uses the MAUI template files (`MainPage.xaml`, `Home.razor`). Replace them as real screens are built.
