# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

StartLauncher is a lightweight Windows WPF desktop app that lets a user configure custom shell commands via a UI and execute them from the Windows Start Menu. For each configured command it generates a `.bat` file in a `bin`-relative `Commands` folder and a Windows shortcut (`.lnk`) in the current user's Start Menu folder (`%AppData%\Microsoft\Windows\Start Menu\Programs\Start Launcher\Commands`). Commands are persisted to a JSON file (path configurable via user settings, defaults to `../commands.json` relative to the app's working directory), which is watched for external changes while the app runs.

- Stack: .NET 8 (`net8.0-windows`), C#, WPF, MSTest v2/v3 unit tests, SDK-style projects with PackageReference.
- Solution: `StartLauncher.sln`
- App project: `StartLauncher.App/`
- Test project: `StartLauncher.Tests/`

## Build and Test

The App project has a `<COMReference>` (`IWshRuntimeLibrary`, used to create `.lnk` shortcuts), and `ResolveComReference` is **not supported by the .NET Core version of MSBuild** — always build with the Visual Studio (desktop) MSBuild, not `dotnet build`:

```
"<path-to-VS>\MSBuild\Current\Bin\MSBuild.exe" StartLauncher.sln /p:Configuration=Debug /p:Platform="Any CPU"
"<path-to-VS>\MSBuild\Current\Bin\MSBuild.exe" StartLauncher.sln /p:Configuration=Release /p:Platform="Any CPU"
```

`dotnet restore`/`dotnet test`/`dotnet build` work fine for the Tests project alone (it has no COM reference), and `dotnet test` is the normal way to run tests once the solution has been built:

```
dotnet test StartLauncher.Tests/StartLauncher.Tests.csproj -c Debug --no-build
```

CI (`.github/workflows/build.yml`, `windows-latest`) restores via `dotnet restore`, then builds/publishes via desktop MSBuild (added to `PATH` with `microsoft/setup-msbuild`) for the same COM-reference reason as above, and runs tests via `dotnet test --no-build`. On push to `master` it additionally publishes a self-contained `win-x64` build and packages/releases it with Velopack's `vpk` CLI (`vpk pack` + `vpk upload github`).

## Architecture

- **Composition root**: `StartLauncher.App/AppContainerBuilder.cs` — wraps an Autofac `ContainerBuilder` (composition, not inheritance — Autofac's `ContainerBuilder` is `sealed` as of Autofac 9.x) and registers `App`, `IIconResolver` → `SimpleIconResolver`, `IExecutablesAccessor` → `ExecutablesAccessor`, `ICommandsDataAccessor` → `CommandsDataAccessor` (all single-instance). Exposes a `Build()` method returning `IContainer`. `App.Main()` builds the container, resolves `App`, and runs it — dependencies flow in via constructor injection from there.
- **App lifecycle**: `StartLauncher.App/App.xaml.cs`. `Main()` first calls `VelopackApp.Build().Run()` (must be the very first line — Velopack's startup hook handling for install/update/uninstall events), then kicks off `EnsureAppUpToDate()` (fire-and-forget), which uses `Velopack.UpdateManager` + `Velopack.Sources.GithubSource` to check/download/apply updates from the GitHub releases of this repo; it no-ops via `mgr.IsInstalled == false` when running from the dev environment (unpackaged). `OnStartup` calls `_executablesAccessor.EnsureCommands()` to sync shortcuts/batch files with the current command list; `App_Startup` then shows `MainWindow` bound to a new `MainViewModel`. Note: `App.xaml` intentionally keeps its build action as `Page` (not the SDK's implicit `ApplicationDefinition`) — otherwise the WPF SDK auto-generates a conflicting `Main()` in `App.g.cs`.
- **UI layer**: `StartLauncher.App/Views/` (XAML windows: `MainWindow`, `EditWindow`, built on MahApps.Metro 2.x `MetroWindow`) — kept thin; avoid adding logic in code-behind beyond what's already there.
- **Presentation logic**: `StartLauncher.App/ViewModels/` — `MainViewModel`, `EditViewModel`, `CommandViewModel` (MVVM), plus `ObservableObject`/`DelegateCommand` base plumbing. Preserve the MVVM split.
- **Domain model**: `StartLauncher.App/Models/CommandDto.cs`.
- **Persistence and OS integration** (`StartLauncher.App/DataAccess/`):
  - `CommandsDataAccessor` — reads/writes the commands JSON file at the path from the `commandsJsonFilePath` user setting (a relative path resolved against the process's **current working directory**, not the exe's folder — matters when launching/testing the built exe manually), watches it for external changes, and tolerates missing files / transient read errors. Uses `Application.Current.Dispatcher` to marshal collection updates back to the UI thread — preserve UI-thread affinity when touching this.
  - `ExecutablesAccessor` — generates `.bat` files and Start Menu `.lnk` shortcuts from the command list, via the `IWshRuntimeLibrary` COM interop.
  - `SimpleIconResolver` — resolves icons for commands from PATH/registry, returning `FluentOptionals.Optional<string>`. FluentOptionals 2.x dropped `IsSome`/`IsNone` from the plain `Optional<T>` (only the multi-type `Composition.Optional<T1,T2,...>` still has them) — use `.Match(some, none)` / `.IfSome()` / `.IfNone()` instead.
- **Core**: `StartLauncher.App/Core/ObservableCollectionExtensions.cs`.

## Conventions and Pitfalls

- Use constructor injection; wire new dependencies through `AppContainerBuilder`, not `new`.
- C# brace/indentation style differs between files (older vs. newer) — match the local file's existing style rather than reformatting.
- The app is Windows-specific by design (Start Menu folders, `.lnk`/`.bat` generation, COM interop) — the assembly is marked `[assembly: SupportedOSPlatform("windows")]`; don't add cross-platform abstractions unless asked.
- **Never manually launch the built exe from an arbitrary working directory to "smoke test" it** — `EnsureCommands()` runs unconditionally on startup and will overwrite the real, shared Windows Start Menu `Start Launcher\Commands` folder based on whatever `commandsJsonFilePath` resolves to from the current working directory (which, for a real installed instance, is `%LocalAppData%\StartLauncher\app-<version>\..\commands.json`). Launching with the wrong working directory (e.g. the repo root) resolves to a nonexistent commands file, which clears the live shortcut folder. If you must run it, set `-WorkingDirectory` to the app's own output/install folder and know what's already in the target Start Menu folder first.
- `App.xaml.cs` contains the Velopack auto-update flow; be careful changing startup order — `VelopackApp.Build().Run()` must remain the first statement in `Main()`.
- Existing tests (`StartLauncher.Tests/AutofacTests.cs`, `SimpleIconResolverTests.cs`) cover DI registration and icon resolution edge cases — extend these when touching `DataAccess`/`ViewModels`. Uses FluentAssertions (pinned to the 7.x line, which is still Apache-2.0 — v8+ introduced a commercial license) and MSTest.

## Agent Working Rules

- Keep edits minimal and task-scoped; no broad refactors or package upgrades beyond what's requested.
- Validate changes with a targeted build/test of the impacted project(s) before finishing — remember the desktop-MSBuild requirement above for the App project's COM reference.
