# Changelog

All notable changes to the GameActivity plugin for Playnite will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.1.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

---

## [1.0.0] - 2026-10-04 - GameActivityNG Initial Release

### Added
- **Rebranding to GameActivityNG**:
  - Rebranded plugin to **GameActivityNG** (`Id: goover_GameActivityNG_Plugin`, `Author: gOOvER`) with updated manifests, metadata, and repository links.
  - Seamless automatic migration on startup from legacy `playnite-gameactivity-plugin` (Lacro59): detects legacy directories, migrates configuration and user data, disables conflicting legacy extension, and displays an informative notification.
  - Added localized migration notice strings (`LOC_GA_LegacyMigrationNotice`) for German and English.
- **Modern SDK-Style Project Conversion**:
  - Converted `GameActivity.csproj` to modern SDK-style format (`Microsoft.NET.Sdk` with `<UseWpf>true</UseWpf>`) targeting .NET Framework 4.6.2.
  - Native CLI build support using `dotnet build` without requiring legacy Visual Studio MSBuild installation.
  - Modernized `PackageReference` structure replacing legacy `packages.config`.
- **CI/CD Automation**:
  - GitHub Actions workflows for continuous build testing (`build.yml`) and automated release packaging with SHA256 checksums (`release.yml`).
### Fixed
- **View Crash on Open (`NullReferenceException` in `GameActivityView`)**:
  - Fixed a crash when opening the GameActivity main view or sidebar with `CumulPlaytimeStore` enabled: added missing null guards on lazily initialized `PART_AggregateSourcesCharts` and ensured `DayGrid` column span is configured on creation.
- **Thread Concurrency & Race Conditions in `GameActivityMonitoring`**:
  - Replaced unsynchronized access to `_runningActivities` with thread-safe synchronization to prevent `InvalidOperationException` and activity state corruption during concurrent session starts/stops.
- **WMI Resource & Handle Leaks in `WMIProvider`**:
  - Added proper disposal of `ManagementObject` instances inside `foreach` loops to prevent COM pointer leakage and WMI quota exhaustion during long gaming sessions.
- **Handle & Memory Leaks in `HWiNFODumper`**:
  - Added proper disposal for `MemoryMappedFile` and `MemoryMappedViewAccessor` on every read cycle.
  - Added boundary index validation on `dwSensorIndex` to prevent `ArgumentOutOfRangeException`.
- **Hardware Monitoring Startup Race Condition**:
  - Resolved `ERROR: Hardware monitor not initialized` on Playnite launch by removing the premature synchronous `CheckMonitoringReadiness()` call before the background initialization finished.
- **Hardware Monitoring Polling Redundancy in `HardwareDataAggregator`**:
  - Implemented a single-round provider cache (`roundCache`) inside `GetMetrics()` so each provider is polled at most once per tick instead of once per metric (eliminating up to 10x redundant polling).
- **MsiAfterburner Sensor Resolution & Localization**:
  - Fixed degree sign `°` rendering as replacement character `` by switching MAHM shared memory string decoding to Windows-1252 ANSI with `°C` normalization.
  - Fixed CPU sensors (e.g. `CPU1 power`, `CPU1 temperature`) being incorrectly labeled with GPU adapter context (`GPU 0: ...`) by restricting GPU table lookup strictly to GPU sensors.
  - Added reading and matching against localized sensor names (`szLocSrcName`, e.g. German `GPU-Auslastung`, `GPU-Temperatur`, `RAM-Nutzung`, `GPU-Leistungsaufnahme`).
  - Added support for sensor aliases (e.g. `Power` <-> `GPU power`, `Framerate` <-> `FPS`).
  - Added automatic fallback from process RAM to system RAM when `RAM usage \ process` is unavailable (e.g. outside 3D games).
- **HWiNFO Polling Performance & Memory Overhead**:
  - Eliminated nested dynamic JSON serialization/deserialization on every sensor and reading item inside `HWiNFOProvider.GetMetrics()`, switching to direct strongly-typed access on `HWiNFODumper.JsonObj`.
- **PerformanceCounterProvider Non-Blocking Sampling**:
  - Eliminated blocking `Thread.Sleep(100)` loops during CPU and RAM sampling.
  - Switched RAM measurement to direct Win32 `GlobalMemoryStatusEx` kernel memory retrieval.
- **LibreHardwareProvider Initialization & Exception Safety**:
  - Fixed initialization returning `true` when no remote server endpoint was configured.
  - Added exception safety and validation to `GetRemoteData()`.
- **WMI Connection Test Optimization**:
  - Changed `SELECT * FROM Win32_Processor` to lightweight `SELECT DeviceID FROM Win32_Processor`.
- **MetricsValidator Modern Thresholds**:
  - Increased upper FPS threshold from 1000 to 2000 and CPU power threshold from 500W to 1000W to accommodate modern high refresh displays and extreme hardware.
- **Integer Overflow in Session Aggregates**:
  - Fixed `(int)x.ElapsedSeconds` cast overflowing to negative numbers on sessions longer than 24.8 days in `FilterItems` and `AvgPlayTime`.
- **Detached Activity Reference Bug**:
  - Fixed `GetLastSessionActivity(false)` returning an unattached instance when session list was empty, which prevented elapsed time updates from being persisted on game stop.
- **Division by Zero Guards**:
  - Added zero-dimension guards in `ImageServices.Resize` when image width or height is 0.
  - Added division-by-zero check in `PerformanceCounterProvider.SampleRam` when `totalRam <= 0`.
- **Culture Invariant Parsing in QuickSearch**:
  - Replaced culture-sensitive `double.Parse` with `CultureInfo.InvariantCulture` in `QuickSearchItemSource` to prevent crashes on non-English locales (e.g. German decimal comma).
- **Thread-Safety in `BrushCache`**:
  - Replaced plain `Dictionary` with thread-safe cache to prevent concurrent read/write crashes.
- **Global Click Handler Overhead**:
  - Isolated custom theme button routing instead of intercepting every button click application-wide via `EventManager.RegisterClassHandler(typeof(Button))`.

### Security
- **Command Injection Guard in `ProcessStarter.StartUrl`**:
  - Replaced unsafe shell invocation (`cmd.exe /C start {url}`) with URI scheme validation (`http`/`https`) and direct `ProcessStartInfo` execution with `UseShellExecute = true`.
- **Hardened External Process Launches**:
  - Added input validation on external process arguments to prevent command execution vectors.

### Changed
- **Embedded & Cleaned Up `playnite-plugincommon`**:
  - Removed git submodule dependency on external `Lacro59/playnite-plugincommon` and deleted `.gitmodules`.
  - Thoroughly cleaned up `playnite-plugincommon`: removed hundreds of unused legacy files and directories including `CommonPluginsStores` (all store integrations: Steam, Epic, GOG, PSN, Xbox, etc.), `CommonPlayniteShared/PluginLibrary`, `CommonPluginsControls/Stores`, unused standalone solution files (`PluginCommon.sln`, `PluginCommon.csproj`, `.coderabbit.yaml`, `.github`), unused controls (`MediaElementExtend`, `ProgressBarExtend`, `RangeSlider`, `SliderWithPointer`, `SourceLinkControl`), and unused views (`ListDataUpdated`, `SelectVariable`).
  - Modernized `CommonPluginsResources.csproj` from a 569-line legacy MSBuild project to a clean, lightweight SDK-style project (`Microsoft.NET.Sdk`).
- **Dependency Modernization**:
  - Updated `PlayniteSDK` to `6.18.0`.
  - Upgraded obsolete packages (`AngleSharp`, `System.IO.Abstractions`, `YamlDotNet`).
- Replaced blocking `Thread.Sleep` calls in `OnGameStopped` with asynchronous delay handling.
- **Web & Network Hardening in `playnite-plugincommon`**:
  - Added TLS 1.2 auto-configuration in `Downloader` to prevent handshake failures on modern secure endpoints under .NET Framework 4.6.2.
  - Fixed socket exhaustion vulnerability in `Web.cs` by ensuring `SharedClient` properly triggers lazy initialization instead of returning null and spawning ephemeral throwaway `HttpClient` instances.
  - Removed redundant, unused ephemeral `HttpClient` allocation in `Web.DownloadFileImage`.
- **GDI+ Resource Leak Prevention**:
  - Added safe disposal for `Bitmap` in catch block in `ImageServices.Resize`.
  - Added proper `using` disposal for `EncoderParameters` and `EncoderParameter` in `ImageServices.ConvertToJpg`.

### Removed
- Removed 200+ dead files and entire unused library subsystems (`CommonPluginsStores`, `PluginLibrary`, and standalone solution scaffolding) from `playnite-plugincommon`.

---

## [Legacy Versions] (Lacro59/playnite-gameactivity-plugin)

> All releases below are legacy versions originally published by Lacro59.

### [3.6] - 2026-10-02
- **Added**: Period view and time filter for per-game activity charts.
- **Added**: Complementary pie charts on the aggregate home views.
- **Added**: Dual playtime totals with mismatch warnings.
- **Added**: Option to ignore games on the data mismatch screen.
- **Added**: Option to hide Playnite hidden games from stats.
- **Added**: Session duration field and synced end date when editing sessions.
- **Added**: Per-game exclusion list for activity tracking.
- **Added**: Remember Game Activity window size and position.
- **Fixed**: Game list sort and column layout now persist correctly.
- **Fixed**: Chart log series visibility remembered across restarts.
- **Fixed**: More reliable session backup timer.
- **Fixed**: Settings crash when store colors were missing.
- **Fixed**: Steam Family Sharing source icon.
- **Fixed**: Unknown sources use the Playnite glyph instead of a broken icon.
- **Fixed**: Aggregate chart tooltips match the product display rules.
- **Fixed**: Game last activity binding in the main activity view.
- **Optimized**: Faster open of the single-game activity view.
- **Improved**: Aggregate charts isolated per display mode.
- **Updated**: Translations.

### [3.5] - 2026-05-15
- **Added**: HWiNFO Gadget mode and more hardware stats in charts.
- **Added**: Choose which HWiNFO sensor to use when several share the same name.
- **Added**: Option to shorten performance logs for short play sessions.
- **Fixed**: More reliable tracking when a game stops.
- **Fixed**: Crash or bad data when stopping a game in some cases.
- **Fixed**: Play time no longer over-counted after the PC was in sleep or standby.
- **Fixed**: Last played date in Playnite now matches your local time.
- **Fixed**: Game Activity panel refreshes when you open it again from the sidebar.
- **Fixed**: Session start and end times can be set to the second and match what you type.
- **Fixed**: Session settings kept when you edit an existing session.
- **Fixed**: HWiNFO readings refresh correctly after a sensor list change.
- **Improved**: Game list selection and column layout in the main view.
- **Updated**: Translations.

### [3.4] - 2026-04-19
- **Added**: Hardware monitoring screen with performance charts and a session timeline (Gantt) view.
- **Added**: FPS charts now include 1% and 0.1% low values.
- **Added**: Export session details and more options to export your activity data.
- **Added**: Hardware stats from MSI Afterburner in charts.
- **Updated**: Translations.
- **Optimized**: Main activity view and chart readability.
- **Fixed**: Time warnings now use your local time.
- **Improved**: General stability and interface polish.

### [3.3.2] - 2025-02-13
- **Fixed**: Crash on settings view.
- **Fixed**: No more GPU Usage tracking.

### [3.3.1] - 2025-02-12
- **Updated**: Localizations.
- **Updated**: Removed LibreHardwareMonitor & OpenHardwareMonitor.

### [3.3] - 2025-01-27
- **Updated**: Localizations.
- **Fixed**: Issue with filtered/unfiltered data (thanks to da3ch1r).
- **Optimized**: UI.
- **Fixed**: No data for CPU & GPU power.
- **Updated**: Export data functions.
- **Fixed**: Many crashes.
- **Fixed**: Last played not updated when adding manual session.
- **Fixed**: Cannot remove play session from game or game from recently played.
- **Fixed**: Can't hide the chart from details.
- **Added**: New data in plugin view.
- **Added**: New view to show data inconsistencies.

### [3.2] - 2024-02-08
- **Updated**: Localizations.
- **Fixed**: Bug with merge function.
- **Added**: New graphic in general view.
- **Fixed**: Bugs with HWiNFO.
- **Fixed**: Bugs with game list order.
- **UI tweaks**.
- **Added**: Log with Open Hardware Monitor.
- **Fixed**: Crashes.

### [3.1.0] - 2023-04-15
- **Updated**: Localizations.
- **Updated**: Icons.
- **Updated**: HWiNFO to use gadget reports or memory sharing.
- **Added**: New elements for HWiNFO.
- **Added**: New theme element to display playtime over weeks.
- **UI tweaks**.
- **Fixed**: Bugs (thanks to CanRanBan).
- **Fixed**: Crashes.

### [3.0.1] - 2022-10-05
- **Updated**: Localizations.
- **Fixed**: Crashes.
- **Fixed**: Issue with chart colors.

### [3.0] - 2022-09-23
- Playnite 10 only.
- **Updated**: Localizations.
- **Fixed**: Crashes.
- **Fixed**: Issue with chart colors.
- **Fixed**: Issue with play count and last played anomalies.
- **Added**: Option to set custom game action name.

### [2.8.1] - 2022-05-20
- **Updated**: Localizations.
- **Fixed**: Issue with column visibility.
- **Updated**: Last game activity when a game session is deleted.
- **Fixed**: Issue with chart colors configuration.

### [2.8] - 2022-05-18
- **Updated**: Localizations.
- **Fixed**: Minor bugs.
- **Fixed**: Unsaved columns order.
- **Fixed**: Issue on log data.
- **Added**: Button to display game data in general plugin view.
- **UI tweaks**.
- **Fixed**: Crashes.

### [2.7.1] - 2022-03-16
- **Updated**: Localizations.
- **Fixed**: Minor bugs.
- **Fixed**: Crash (thanks to BanCrash).

### [2.7] - 2022-03-11
- **Improved**: Performance.
- **Added**: Support for multiple running games.
- **Added**: PSU data for CPU & GPU from MSI Afterburner.
- **Added**: New options for chart in game data view.
- **Added**: Temporary workaround for PlayState time (thanks to BanCrash).
- **UI tweaks**.
- **Updated**: Localizations.
- **Fixed**: Minor bugs.
- **Fixed**: Crashes.

### [2.6] - 2022-02-04
- **UI tweaks**.
- **Updated**: Localizations.
- **Added**: Option to truncate empty date session.
- **Added**: Gantt view style.
- **Fixed**: Minor bugs.
- **Fixed**: Crashes.

### [2.5.1] - 2022-01-24
- **UI tweaks**.
- **Fixed**: Random issue on game stopped event.
- **Fixed**: Issue with dates not in local time.
- **Updated**: Localizations.

### [2.5] - 2022-01-14
- **UI tweaks**.
- **Added**: Option to transfer data to another game.
- **Added**: Option to delete/transfer data without Playnite game.
- **Added**: Backup system when Playnite crashes.
- **Added**: Option to save column order.
- **Updated**: Localizations.
- **Fixed**: Minor bugs.
- **Fixed**: Crashes.

### [2.4.3] - 2021-12-03
- **Fixed**: Crashes.

### [2.4.2] - 2021-12-03
- **Updated**: Localizations.
- **Added**: Missing edit value for game sessions.
- **Fixed**: Minor bugs.
- **Fixed**: Crashes.

### [2.4.1] - 2021-11-29
- **Fixed**: Issue with play action integration.

### [2.4] - 2021-11-28
- **Updated**: Localizations.
- **Fixed**: Issue with QuickSearch.
- **Fixed**: Issue with time zone.
- **Added**: Play action used for the game session.
- **Fixed**: Crashes.

### [2.3.1] - 2021-11-20
- **Fixed**: Loading issue.

### [2.3] - 2021-11-20
- **Added**: Option to add game session manually.
- **Supported**: QuickSearch plugin.
- **Fixed**: Minor bugs.
- **Fixed**: Crash with StartPage plugin.

### [2.2] - 2021-11-11
- **UI tweaks**.
- **Added**: Option to select the increment for chart navigation.
- **Added**: Option to hide short sections of games.
- **Added**: Ability to delete a game session.
- **Updated**: Localizations.
- **Fixed**: Minor bugs.
- **Fixed**: Crashes.

### [2.1.2] - 2021-11-05
- **Improved**: Performance.
- **Fixed**: Minor bugs.
- **Fixed**: Crashes.

### [2.1.1] - 2021-10-12
- Playnite 9 version.
- **Fixed**: Crashes.

### [2.1] - 2021-10-08
- Playnite 9 version.
- **Added**: Export data function.
- **Added**: Store selection color.

### [2.0.1] - 2021-09-16
- Playnite 9 beta version.
- **UI tweaks**.
- **Fixed**: Minor bugs.

### [2.0] - 2021-09-05
- Playnite 9 beta version.
- **Added**: Custom UI Elements for custom theme integration.
- **Fixed**: Minor bugs.
- **Added**: New chart options.
- **Added**: New options in plugin main view.
