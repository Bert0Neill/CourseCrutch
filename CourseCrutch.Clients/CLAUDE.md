# CourseCrutch.Clients

.NET MAUI app with a Blazor Hybrid UI. Targets Android, iOS, Mac Catalyst and Windows.

## Rules

- UI lives in Razor components under `Components/`. Layouts go in `Components/Layout/`, pages in `Components/Pages/`.
- Do not reference Domain, Application or Infrastructure directly. Get data from the API over HTTP.
- Register services in `MauiProgram.cs`. Do not construct HTTP clients inside components.
- Platform-specific code goes under `Platforms/<platform>/`. Do not put `#if` platform checks in shared code unless there is no other option.
- Keep CSS in `wwwroot/app.css` or a component's `.razor.css` file. MAUI's default CSS handling is disabled (`EnableDefaultCssItems` is false), so do not rely on it.
- Do not change `ApplicationId`, `ApplicationTitle` or version numbers without asking.
- Photos stay in memory. `PhotoCaptureService` deletes the picker's temp file after reading it. Do not write images to storage.
- OCR runs on device through `IOcrService` (TesseractOcrMaui). Do not send images to the API. Only the extracted text goes to the API.
- Register services and the API base address in `MauiProgram.cs`. Android uses `10.0.2.2` to reach the host.

## Build targets

Each target requires its platform SDK. On Windows, `net10.0-windows10.0.19041.0` builds. Other targets need their platform workloads installed. Build a single target when verifying changes, for example `dotnet build -f net10.0-windows10.0.19041.0`.

## Current state

- `Components/Pages/Home.razor` is the Scan page with Image, Text and Results tabs.
- `MainPage.xaml` and the other MAUI template files are unused. Remove them when convenient.
- OCR uses `Resources/Raw/eng.traineddata` (tessdata_fast, English). Verified by building Windows and Android. iOS is not built on Windows.
- `Platforms/Android/AndroidManifest.xml` sets `usesCleartextTraffic` so the emulator can reach the local HTTP API. Remove it before a release build.
