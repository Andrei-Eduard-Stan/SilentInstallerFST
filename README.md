# SilentInstallerFST

Silent Installer for Essential Software [REWORK]
Project Overview
15/02/2025 – 23/02/2025
1. Overview
The Silent Installer is a Windows-based application designed to streamline the installation of multiple software applications in an automated, efficient, and user-friendly manner. It is particularly useful for the IT Department, enabling quick deployment of predefined software configurations based on different device types and user needs.
This tool ensures that essential applications are installed without requiring user interaction, making it ideal for new device setup, system reinstalls, and corporate standardization without the need for additional licensing or costs. 
________________________________________
2. How It Works
The Silent Installer follows a step-by-step process to install applications:
1.	User selects a category (e.g., "MH Laptop" or "HO Laptop").
2.	The application retrieves predefined installation steps based on the selected category.
3.	Each application is installed silently using its corresponding command-line arguments.
4.	Progress is logged in real-time, showing installation status.
5.	User can abort the process at any time, stopping all remaining installations.
6.	Once installation is complete, an option to restart the system is available.
________________________________________
3. Features & Capabilities
✔️ Automated Batch Installation – Reduces manual effort for IT teams.
✔️ Silent Mode Execution – Applications install in the background without user prompts.
✔️ Predefined Setup Categories – Different configurations for different user roles.
✔️ Abort and Logging Functionality – Users can stop installations anytime, with logs for debugging.
✔️ GUI-Based Selection – User-friendly interface for category selection.
✔️ Post-Installation Actions – Includes system restart options.
✔️ Multi-App Installation – Installs multiple applications sequentially.
________________________________________


4. Applications Installed
Applications Installed	MH Laptop	HO Laptop
Acrobat Reader	Yes	Yes
Google Chrome 	Yes	Yes
GlobalProtect 	Yes	Yes
SupportAssist	Yes	Yes
LogMeIn (MH)	Yes	Yes
LogMeIn (HO)	Yes	Yes
Office Suite	No	Yes
Mimecast for Outlook	No	Yes

________________________________________
5. Technologies & Resources Used
Programming Languages & Frameworks
•	C# (.NET Framework - WPF) – Used for GUI development.
•	XAML – Defines the UI layout.
•	Windows Batch Scripting – Used to execute silent installation commands.
Resources Used
•	MahApps.Metro Framework – For modern UI styling.
•	Windows Presentation Foundation (WPF) – UI framework.
•	System.Diagnostics.Process – Handles process execution for silent installations.
•	Windows Shell Commands – Used to launch installers and execute commands.
File Paths & Dependencies
•	Application Installer Path: C:\\Users\\asta\\OneDrive\\SilentInstaller\\Installers
o	Note: This can be modified anytime depending on the needs of the department, it represents the current installer path at the date of writing this documentation
•	Required Installation Files:
o	Acrobat\\acrsetup.exe
o	Chrome\\GoogleStandaloneSetup64.msi
o	PaloAlto\\GlobalProtect64-6.2.2.msi
o	SupportAssist\\SupportAssistx64.msi
o	LMI\\logmein.msi and LMI\\LMI Head Office.msi
o	Office\\setup.exe and Office\\configuration.xml
o	Mimecast\\Mimecast.msi
________________________________________




6. Future Enhancements
If possible and upon requests, I am considering further developing the application in the following areas:
🔹 Expanded Setup Categories
•	Additional device-specific configurations (e.g., "Finance Team Laptop," "Avondata/ Hotel PC", etc.).
o	Note: The only potential issue I can imagine regarding achieving this is if any of the required software do not have an .msi or standalone offline installer .exe version of the software, which will render silent installation unachievable.
•	Dynamic loading of categories via a config file or database instead of hardcoding them.
🔹 Custom Installation Profiles
•	Users can select/deselect individual applications before starting installation.
•	Option to install additional software beyond predefined categories.
🔹 Remote Deployment Features
•	Ability to deploy software to multiple machines on a network.
•	Integration with other components of our IT ecosystem to further facilitate processes.
🔹 Logging & Reporting Enhancements
•	More detailed installation logs (timestamps, errors, success rates).
🔹 Unattended Deployment
•	Scheduled silent installations at specific times or via a startup script.
•	Option to run silently without GUI (e.g., executed via a command line with parameters).
🔹 GUI Enhancements & User Experience
•	Progress Bar for Each Installation – Better visualization of installation progress.
•	Better GUI – Improved UI customization.
o	Custom Themes, for accessibility reasons.
o	Animations and Interactions for improved user experience.
•	Application Search & Filtering – Easier selection of apps, if the application will be significantly expanded.
________________________________________









7. Requirements & Constraints
System Requirements
🖥️ OS: Windows 7+ (64-bit)
🔧 Admin Rights: Required for system-wide installation
📡 Network: Required for accessing the installation folders if on the file server
Constraints
❌ Requires Administrator Permissions – Some installations may fail without admin rights.
❌ No Mac/Linux Support – Designed specifically for Windows environments.
❌ Dependent on Installer Files – Missing files will cause installations to fail.
❌ SupportAssist no GUI – This version of SupportAssist is not a standalone app but allows automatic detection of the device on the Dell webpage.
•	The MSI version provided by Dell does not install the standalone version in silent mode.
•	Note: I noticed however, that after a short time the SupportAssist desktop app becomes available.

________________________________________
8. Testing
Test Case Set 1: General Installation Scenarios
Test Case #	Test Description	Steps	Expected Result	Status
TC01	Verify successful installation of all applications for HO Laptop category	1. Launch the app 2. Select "HO Laptop" 3. Click "Start Installation" 4. Wait for completion	All listed apps install successfully without errors	✅ Pass  
TC02	Verify successful installation of all applications for MH Laptop category	1. Launch the app 2. Select "MH Laptop" 3. Click "Start Installation" 4. Wait for completion	All listed apps install successfully without errors	✅ Pass  
TC03	Ensure installation continues even if one application fails	1. Corrupt one installer file 2. Start installation 3. Observe behavior	The failed app is logged, but other apps continue installing	✅ Pass  
TC04	Check what happens when installation is aborted midway	1. Select category 2. Start installation 3. Click "Abort"	The installation should stop, and logs should reflect aborted status	✅ Pass  
TC05	Ensure installer skips already installed software	1. Pre-install some apps 2. Run Silent Installer	Already installed apps should be skipped	✅ Pass  
________________________________________





Test Case Set 2: UI & User Experience
Test Case #	Test Description	Steps	Expected Result	Status
TC06	Ensure GUI displays categories correctly	1. Launch the app 2. Observe the category dropdown	All expected categories appear	✅ Pass 
TC07	Verify that the installation progress updates correctly	1. Start installation 2. Observe progress messages/logs	Real-time updates should show in UI/logs	✅ Pass 
TC08	Ensure the "Abort" button works immediately	1. Start installation 2. Click "Abort" during an active install	Installation stops immediately without hanging	✅ Pass 
TC09	Verify that logs are generated correctly	1. Start installation 2. Check log file after completion	Logs should list installed apps, skipped apps, and errors	✅ Pass 
TC10	Ensure that the installer UI remains responsive during installs	1. Start installation 2. Interact with the UI	UI should not freeze during the process	✅ Pass 
________________________________________
Test Case Set 3: Error Handling & Edge Cases
Test Case #	Test Description	Steps	Expected Result	Status
TC11	Test with missing installer files	1. Delete some installer files 2. Run Silent Installer	Errors should be logged, but other installations continue	✅ Pass  
TC12	Verify behavior when internet-dependent apps install without a connection	1. Disable internet 2. Start installation	Cloud-based installs (e.g., Office) should fail gracefully	✅ Pass  
TC13	Check if the installer handles invalid command-line arguments correctly	1. Modify an installer argument 2. Run Silent Installer	Invalid arguments should be logged as errors	✅ Pass  
TC14	Test how the installer behaves if the process is killed mid-way	1. Start installation 2. Kill the process in Task Manager	Installation should stop without leaving broken installs	✅ Pass  
TC15	Check compatibility with Windows Defender / Antivirus settings	1. Enable strict Windows Defender settings 2. Start installation	Defender should not block safe installations	✅ Pass  
TC16	Test system behavior if multiple instances of the installer are run	1. Start installation 2. Open another instance of the app	Should prevent multiple instances from running simultaneously	✅ Pass  
________________________________________
Test Execution Summary
Status	Count
✅ Pass	16 / 16
❌ Fail	0 / 16
________________________________________

9. Conclusion
The Silent Installer is an efficient, reliable and user-friendly tool for the IT department to facilitate and standardize software installations across multiple machines. With future enhancements, it can evolve into a tool used on a wider scale, integrating remote deployment, custom software selection, and security features.
🚀 Next Steps:
•	Gather user feedback to identify improvements.
•	Implement custom profiles & remote deployment features.
•	Enhance logging, reporting, and security measures.

