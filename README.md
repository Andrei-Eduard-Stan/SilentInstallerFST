
# 🧰 Silent Installer – Essential Software Deployment (Revamped)

> Project Timeline: **15/02/2025 – 03/03/2025**  
> Platform: Windows (WPF Desktop App)  
> Author: Andrei

---

## 📄 1. Overview

The **Silent Installer** is a Windows-based application for streamlined software deployment. It automates multi-app installations based on device categories, ideal for IT-managed onboarding, reinstalls, and corporate standards—without licensing costs.

### Key Use Cases:
- New machine provisioning
- Reinstallation workflows
- Standardized corporate setups

---

## 🔧 2. How It Works

1. User runs the application **as administrator**.
2. Chooses a **category** (e.g., *MH Laptop*, *HO Laptop*) or opts for driver updates.
3. The app loads a **predefined install list** based on selection.
4. Each installer runs **silently**, using command-line arguments.
5. Progress is logged in real time.
6. User may **abort** at any point.
7. Optional system **restart** prompt upon completion.

> 💡 Dell Command Update (DCU) is bundled in both categories for driver handling.

---
![[fstinstaller_drivers.mp4]]
![[fstinstaller_installapps.mp4]]
## ⚙️ 3. Features

| Feature                        | Description                                                 |
|-------------------------------|-------------------------------------------------------------|
| ✔️ Batch Install              | Automates multi-app setup                                  |
| ✔️ Silent Execution           | No user input required during installs                     |
| ✔️ Category Selection         | MH Laptop / HO Laptop with different setups                |
| ✔️ Abort & Log Functionality  | Real-time logs + abort mechanism                          |
| ✔️ GUI-Based                  | Built with WPF + MahApps.Metro                            |
| ✔️ Post-Install Options       | Restart prompt after installs                              |

---

## 💻 4. Applications by Category

| Application               | MH Laptop | HO Laptop |
|---------------------------|-----------|-----------|
| Acrobat Reader            | ✅        | ✅        |
| Google Chrome             | ✅        | ✅        |
| GlobalProtect VPN         | ✅        | ✅        |
| SupportAssist             | ✅        | ✅        |
| LogMeIn                   | ✅ (MH)   | ✅ (HO)   |
| Office Suite              | ❌        | ✅        |
| Mimecast for Outlook      | ❌        | ✅        |
| Microsoft Teams           | ❌        | ✅        |
| Dell Command Update       | ✅        | ✅        |

---

## 🧪 5. Tech Stack

### Languages & Frameworks:
- `C# (.NET Framework - WPF)`
- `XAML` for UI layout
- `Windows Batch Scripting`

### Libraries:
- `MahApps.Metro` – UI styling
- `System.Diagnostics.Process` – Installer execution

### Installer Path:
```
C:\Users\asta\OneDrive\SilentInstaller\Installers
```

> ⚠️ This path is configurable depending on deployment location.

### Dependencies:
- `Acrobat\acrsetup.exe`
- `Chrome\GoogleStandaloneSetup64.msi`
- `PaloAlto\GlobalProtect64-6.2.2.msi`
- `SupportAssist\SupportAssistx64.msi`
- `LMI\logmein.msi` + `LMI\LMI Head Office.msi`
- `Office\setup.exe`, `Office\configuration.xml`
- `Mimecast\Mimecast.msi`
- `Teams\teamsbootstrapper.exe`, `Teams\MSTeams-x64.msix`
- `Dell Command Update installer`

---

## 🚀 6. Future Enhancements

| Area                 | Planned Improvement                                            |
|----------------------|----------------------------------------------------------------|
| 🔹 Categories         | More roles/devices (e.g. Finance, Hotel PCs)                 |
| 🔹 Profiles           | Custom checkboxes, optional app install                      |
| 🔹 Remote Deployment  | Push installs over LAN or domain                              |
| 🔹 Logs & Reports     | Error tracking, timestamps, install status                    |
| 🔹 Unattended Mode    | Command-line silent install, scheduled or login-time trigger |
| 🔹 GUI Enhancements   | Progress bar per app, UI themes, animations                   |

---

## 🧷 7. Requirements & Constraints

### System:
- OS: Windows 7+
- 64-bit only
- Admin rights required
- .NET 8.0 Framework installed

### Constraints:
- ❌ No Mac/Linux support
- ❌ Admin-only execution
- ❌ Installer file dependency (must exist)
- ❌ SupportAssist's silent install lacks GUI until post-setup

---

## ✅ 8. Testing – Execution Summary

### Test Case Set 1: General Installation

| Test Case | Description                                           | Status |
|-----------|-------------------------------------------------------|--------|
| TC01      | Full install (HO Laptop)                              | ✅ Pass |
| TC02      | Full install (MH Laptop)                              | ✅ Pass |
| TC03      | Continue if one installer fails                       | ✅ Pass |
| TC04      | Abort functionality during install                    | ✅ Pass |
| TC05      | Skips already installed apps                          | ✅ Pass |

---

### Test Case Set 2: UI Experience

| Test Case | Description                                           | Status |
|-----------|-------------------------------------------------------|--------|
| TC06      | Category dropdown displays correctly                  | ✅ Pass |
| TC07      | Progress updates live                                 | ✅ Pass |
| TC08      | Abort button responsive                               | ✅ Pass |
| TC09      | Log file generates expected content                   | ✅ Pass |
| TC10      | Installer UI stays responsive                         | ✅ Pass |

---

### Test Case Set 3: Edge Cases

| Test Case | Description                                           | Status |
|-----------|-------------------------------------------------------|--------|
| TC11      | Missing installer file                               | ✅ Pass |
| TC12      | No internet during install (cloud apps)              | ✅ Pass |
| TC13      | Invalid arguments logged                             | ✅ Pass |
| TC14      | Installer process killed midway                      | ✅ Pass |
| TC15      | Windows Defender / AV interference                   | ✅ Pass |
| TC16      | Prevent multiple instances                           | ✅ Pass |

> ✅ **Test Summary:** 16/16 Passed

---

## 🔚 9. Conclusion

The **Silent Installer** offers a scalable, modular, and user-friendly tool for IT to standardize and expedite software setup across systems. Future iterations aim to introduce remote deployment, deeper customization, and richer diagnostics.

> 🚀 Next Steps:
- Gather feedback
- Add custom profiles
- Implement remote deployment + improved logs

---

*This document was archived and formatted for vault integration by your dusk-coded rabbit archivist.*
