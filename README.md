<!-- markdownlint-disable MD033 MD041 -->

# GameActivityNG

[![GitHub release (latest by date)](https://img.shields.io/github/v/release/gOOvER/playnite-gameactivity-ng?style=flat-square)](https://github.com/gOOvER/playnite-gameactivity-ng/releases)
[![Website](https://img.shields.io/badge/playnite.goover.dev-Showcase%20%26%20Downloads-ea8024?style=flat-square&logo=googlechrome&logoColor=white)](https://playnite.goover.dev)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg?style=flat-square)](LICENSE)
[![Ko-fi](https://img.shields.io/badge/Ko--fi-Support-F16061?style=flat-square&logo=ko-fi&logoColor=white)](https://ko-fi.com/goover)

> **GameActivityNG** is an advanced extension for the [Playnite](https://playnite.link/ "Playnite - video game library manager") video game library manager that tracks gameplay sessions, visualizes playtime trends, and monitors hardware performance directly inside Playnite.
>
> Originally created as [GameActivity](https://github.com/Lacro59/playnite-gameactivity-plugin) by Lacro59, now modernized, secured, and actively maintained as **GameActivityNG** by [gOOvER](https://github.com/gOOvER).

🌐 **Official Showcase & Direct Downloads**: [https://playnite.goover.dev/](https://playnite.goover.dev/)  
[GitHub Repository](https://github.com/gOOvER/playnite-gameactivity-ng) | [Issue Tracker](https://github.com/gOOvER/playnite-gameactivity-ng/issues)

---

## ✨ Features

- **Advanced activity tracking**: records session date and elapsed time per game, then aggregates trends by day, week, month, source, and genre.
- **Per-game performance insights**: displays average metrics and session-level details such as FPS, CPU/GPU usage, RAM usage, temperatures, and power.
- **Hardware monitoring providers**: supports Windows Performance Counters, WMI, LibreHardware, HWiNFO, MSI Afterburner, and RivaTuner depending on your setup.
- **QuickSearch integration**: lets you search games by activity data using FPS, session duration, or date queries.
- **Built-in data tools**: includes CSV export, data mismatch checks, isolated-data detection, transfer tools, and database maintenance actions.
- **Theme integration points**: exposes controls for custom themes in game details, list views, and dedicated activity views.
- **Seamless Legacy Migration**: automatically detects previous installations of `playnite-gameactivity-plugin` (Lacro59), preserves all existing user session data and databases, and safely disables conflicting legacy extensions.

---

## 📸 Screenshots

### Main dashboard
<picture>
  <img alt="Game activity main dashboard with playtime charts and statistics" src="https://raw.githubusercontent.com/gOOvER/playnite-gameactivity-ng/master/forum/main_01.jpg" height="250px">
</picture>

### In-view controls
<picture>
  <img alt="Chart controls for filtering and navigating session metrics" src="https://raw.githubusercontent.com/gOOvER/playnite-gameactivity-ng/master/forum/control_01.jpg" height="250px">
</picture>

### Settings panel
<picture>
  <img alt="Plugin settings including monitoring and integration options" src="https://raw.githubusercontent.com/gOOvER/playnite-gameactivity-ng/master/forum/settings_01.jpg" height="250px">
</picture>

---

## 🔍 QuickSearch

GameActivityNG integrates with Playnite QuickSearch (command key: `ga`) and adds sub-commands to filter games by recorded activity data.

Example queries:

- `ga fps > 60`
- `ga fps 30 <> 60`
- `ga time > 2 h`
- `ga date 2025-01-01 <> 2025-01-31`

| Parameter | Purpose | Syntax | Example |
| --- | --- | --- | --- |
| `fps` | Filter by average FPS | `fps <value`, `fps >value`, `fps <min> <> <max>` | `fps > 75` |
| `time` | Filter by session duration | `time <value> <unit>`, `time >value <unit>`, `time <min> <unit> <> <max> <unit>` | `time 30 min <> 2 h` |
| `date` | Filter by session date | `date <YYYY-MM-DD`, `date >YYYY-MM-DD`, `date <start> <> <end>` | `date > 2026-01-01` |

---

## ⚙️ Configuration

### General behavior

- Enable or disable integration buttons in header, sidebar, and game details.
- Configure chart visibility, axis display, data density, and displayed metric series.
- Adjust session handling rules (ignore short sessions, cumulative behavior, paused-time subtraction).

### Hardware monitoring

- Enable logging and select automatic or manual provider mode.
- Configure provider-specific options (for example HWiNFO sensor IDs / indexes, LibreHardware remote endpoint, RivaTuner usage).
- Set fallback and cache behavior for stability when a provider fails.

### Warnings and analysis

- Configure in-game warning thresholds for FPS, CPU/GPU temperature, CPU/GPU usage, and RAM usage.
- Tune analysis windows used for recent activity and chart grouping.

---

## 🚀 Installation & Migration

### Automatic Migration from Legacy GameActivity
If you already had `playnite-gameactivity-plugin` (by Lacro59) installed:
- Installing **GameActivityNG** will automatically recognize and preserve all your historical playtime logs, hardware sessions, and configuration.
- The legacy plugin will be automatically disabled to prevent conflicts.

### Manual Installation (`.pext`)
1. Download the latest `goover_GameActivityNG_Plugin_*.pext` package from [Releases](https://github.com/gOOvER/playnite-gameactivity-ng/releases).
2. In Playnite, navigate to **Main Menu (top left) > Add-ons > Install from file...**
3. Select the downloaded `.pext` file.
4. Restart Playnite when prompted.

---

## 🤝 Contributing & Feedback

- **Bug reports**: [Open an issue](https://github.com/gOOvER/playnite-gameactivity-ng/issues/new)
- **Feature requests**: [Submit a suggestion](https://github.com/gOOvER/playnite-gameactivity-ng/issues/new)
- **Pull requests**: [Submit a PR](https://github.com/gOOvER/playnite-gameactivity-ng/pulls)

---

## 💝 Support

[![Ko-fi](https://img.shields.io/badge/Ko--fi-Support-F16061?style=flat-square&logo=ko-fi&logoColor=white)](https://ko-fi.com/goover)

If you find this plugin helpful, consider supporting the development on [Ko-fi](https://ko-fi.com/goover).

---

## 📄 License

This project is licensed under the [MIT License](LICENSE).
Original work Copyright (c) Lacro59. Modifications and NG release Copyright (c) gOOvER.
