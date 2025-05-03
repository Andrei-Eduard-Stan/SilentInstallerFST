# 🧰 Silent Installer – Software Deployment Tool (Internal Use)

**Author:** Andrei Stan  
**Status:** Internal Prototype | Proof-of-Concept  
**Tech Stack:** C# (.NET WPF) | XAML | Batch Scripts | MahApps.Metro

---
![image](https://github.com/user-attachments/assets/4e5b0e5c-f173-4701-ae4a-970126e8e54b)

## 🔍 Overview

Silent Installer is a Windows desktop application designed to streamline software installation across company devices with zero user input. It offers category-based automation (e.g., _HO Laptop_, _MH Laptop_) and performs silent, batch execution of preconfigured application installers.

It was built to solve one recurring pain point in IT:  
**Manual installs are slow, error-prone, and inconsistent.**  
This tool fixes that with a GUI, a script backend, and logs that actually tell you what happened.


https://github.com/user-attachments/assets/61170aa3-df0c-4c94-aa87-9a02b68e7dca

---

## 🎯 Key Features

|Feature|Description|
|---|---|
|✅ Category Selection|MH / HO device profiles with role-specific apps|
|✅ Silent Mode|Predefined command-line installs (no user prompts)|
|✅ Abort Mechanism|Cancel mid-process safely|
|✅ Real-Time Logging|Timestamped log output per run|
|✅ GUI (WPF)|Clean layout with MahApps.Metro UI|
|✅ Restart Option|Prompted reboot post-install|

---

## 💻 Sample App Matrix

|   |   |   |
|---|---|---|
|App|MH Laptop|HO Laptop|
|Acrobat Reader|✅|✅|
|Google Chrome|✅|✅|
|GlobalProtect VPN|✅|✅|
|LogMeIn|✅|✅|
|Office + Teams|❌|✅|
|Mimecast|❌|✅|
|Dell Command Update|✅|✅|

---

## 🛠 Tech Breakdown

- **Frontend:** C# WPF (.NET 8), XAML, MahApps.Metro
    
- **Execution:** Windows Batch Scripts via `System.Diagnostics.Process`
    
- **Installer Path:** Configurable (currently internal file share or USB)
    
- **Pre-reqs:** Admin rights, Windows 7+, .NET 8.0
    

---

## 🧪 Testing Summary

|   |   |   |
|---|---|---|
|Area|Test Set|Result|
|General Installs|TC01–TC05|✅ Pass|
|UI Functionality|TC06–TC10|✅ Pass|
|Edge Handling|TC11–TC16|✅ Pass|

> ✅ 16/16 Test Cases Passed  
> Handles network disconnects, missing files, bad arguments, and abort logic gracefully.

---

## 🚧 Known Limitations

- Not yet portable; requires manual path setup or USB stick with correct structure.
    
- Only supports Windows; Linux/macOS not planned.
    
- Some apps (e.g., SupportAssist) have quirks in silent install detection.
    
- Admin execution required.
    

---

## 🗺️ Roadmap Ideas (Future Iteration)

- 📦 Checkbox-based custom install profiles
    
- 🌐 Remote deployment across network
    
- 📊 Centralized log collection & dashboard
    
- 🔁 Config-driven categories
    
- 💬 CLI mode for unattended scheduled installs
    

---

## 🔒 Disclaimer

This is an internal-use tool, redacted for public visibility.  
Paths, installers, and exact logic have been adjusted to protect proprietary configs.  
Think of this repo as a blueprint—not a plug-and-play product.

---

## 💬 Contact

Want to talk internal tooling? Automation? Infra from the inside out?  
[github.com/andreistan](https://github.com/andreistan) • Ready to scale what others overlook.
