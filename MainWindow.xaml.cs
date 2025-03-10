using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using MahApps.Metro.Controls;


namespace SilentInstaller
{
    public partial class MainWindow
    {
        private CancellationTokenSource cancellationTokenSource;
        private List<Category> Categories;
        private List<InstallationStep> installationSteps;
        private int currentStepIndex = 0;
        private int currentCategoryIndex = 0;
        private string installerPath = @"\\ph-svr-file\Software\Andrei";




        private void DefineInstallationSteps(Category category)
        {
            installationSteps = new List<InstallationStep>();
            installationSteps.Add(new InstallationStep("Acrobat Reader", $"{installerPath}\\Acrobat\\acrsetup.exe","/sAll /rs /msi EULA_ACCEPT=YES", "data/acrobatreader.png"));
            installationSteps.Add(new InstallationStep("Google Chrome","msiexec", $"/i \"{installerPath}\\Chrome\\Installers\\GoogleChromeStandaloneEnterprise64.msi\" /quiet /norestart", "data/chrome_logo.png"));
            installationSteps.Add(new InstallationStep("GlobalProtect","msiexec", $"/i \"{installerPath}\\PaloAlto\\GlobalProtect64-6.0.1.msi\" /quiet /norestart","data/globalprotect.png"));
            installationSteps.Add(new InstallationStep("SupportAssist","msiexec", $"/i \"{installerPath}\\SupportAssist\\SupportAssistx64-4.6.3.23467.msi\" /quiet /norestart", "data/dell.png"));

            if (category.Name == "MH Laptop") {
                installationSteps.Add(new InstallationStep("Logmein MH","msiexec", $"/i \"{installerPath}\\LMI\\logmein.msi\" /quiet DEPLOYID=01_p7xqfoq7wc6kh6vcw4d007hp3hb1mgk5bm79z INSTALLMETHOD=5 FQDNDESC=1", "data/logmein.png"));
            }

            if (category.Name == "HO Laptop")
            {
                installationSteps.Add(new InstallationStep("Logmein HO","msiexec", $"/i \"{installerPath}\\LMI\\LMI Head Office.msi\" /quiet DEPLOYID=01_p7xqfoq7wc6kh6vcw4d007hp3hb1mgk5bm79z INSTALLMETHOD=5 FQDNDESC=1", "data/logmein.png"));
                installationSteps.Add(new InstallationStep("Office Suite", $"{installerPath}\\Office\\setup.exe", $"/configure \"{installerPath}\\Office\\Latest.xml\"", "data/officesetup.png" ));
                installationSteps.Add(new InstallationStep("Mimecast","msiexec", $"/i \"{installerPath}\\Mimecast\\Mimecast.msi\" /quiet /norestart", "data/Mimecast_Logo.png"));
                installationSteps.Add(new InstallationStep("Teams", $"{installerPath}\\Teams\\teamsbootstrapper.exe", $"-p -o \"{installerPath}\\Teams\\MSTeams-x64.msix\"", "data/Microsoft_Office_Teams_Logo_512px.png"));
            }

            installationSteps.Add(new InstallationStep("Dell DCU", $"{installerPath}\\SupportAssist\\Dell-Command-Update-Windows-Universal-Application_9M35M_WIN_5.4.0_A00.EXE", "/s", "data/dell.png"));

        }

        private async void StartInstallation_Click(object sender, RoutedEventArgs e)
        {
            cancellationTokenSource = new CancellationTokenSource();

            SelectionPage.Visibility = Visibility.Collapsed;
            InstallationPage.Visibility = Visibility.Visible;

            Category selectedCategory = Categories[currentCategoryIndex];
            DefineInstallationSteps(selectedCategory);

            await InstallApplicationsAsync(cancellationTokenSource.Token);

        }

        private async Task InstallApplicationsAsync(CancellationToken token)
        {
            List<InstallationStep> installedApps = new List<InstallationStep>();

            foreach (var step in installationSteps)
            {

                if(cancellationTokenSource.Token.IsCancellationRequested)
                {
                    AppendLog("[INFO] Installation Aborted.");
                    break;
                }
                UpdateUI(step.Name);
                await RunInstallationProcessAsync(step,token);
                installedApps.Add(step);
            }
            // Move to completion page only if not cancelled
            if (!token.IsCancellationRequested)
            {
                InstallationPage.Visibility = Visibility.Collapsed;
                CompletionPage.Visibility = Visibility.Visible;
            }

            UpdateInstalledApss(installedApps);
        }

        private void AbortInstallation_Click(object sender, RoutedEventArgs e)
        {
            if (cancellationTokenSource != null)
            {
                cancellationTokenSource.Cancel(); // Signal cancellation
            }

            AppendLog("[INFO] Installation Aborted by user.");

            // Return to the first panel
            InstallationPage.Visibility = Visibility.Collapsed;
            CompletionPage.Visibility = Visibility.Collapsed;
            AbortPage.Visibility = Visibility.Visible;
        }

        private void GoBack_Click(object sender, RoutedEventArgs e)
        {
            AbortPage.Visibility=Visibility.Collapsed;
            CompletionPage.Visibility = Visibility.Collapsed;
            InstallationPage.Visibility = Visibility.Collapsed;
            SelectionPage.Visibility = Visibility.Visible;
        }
        private void RestartButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("shutdown.exe", "-r -t 0");
                AppendLog("[INFO] Restarting PC in 5 seconds...");
            }
            catch (Exception ex)
            {
                AppendLog($"[ERROR] Failed to restart: {ex.Message}");
            }
        }



        private async void UpdateDrivers_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                AppendLog("[INFO] Checking for Dell driver updates...");

                ProcessStartInfo psi = new ProcessStartInfo
                {
                    FileName = "cmd.exe",
                    Arguments = "/c cd /d \"C:\\Program Files\\Dell\\CommandUpdate\" && dcu-cli.exe /scan && dcu-cli.exe /applyUpdates -silent",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                using (Process process = new Process { StartInfo = psi })
                {
                    process.Start();

                    // Switch UI to installation page while update is running
                    SelectionPage.Visibility = Visibility.Collapsed;
                    InstallationPage.Visibility = Visibility.Visible;

                    // Read output asynchronously (REAL-TIME)
                    Task outputTask = Task.Run(async () =>
                    {
                        while (!process.StandardOutput.EndOfStream)
                        {
                            string line = await process.StandardOutput.ReadLineAsync();
                            if (!string.IsNullOrWhiteSpace(line))
                            {
                                AppendLog($"[INFO] {line}"); // Show real-time progress
                            }

                            await Task.Delay(3000); // Wait 3 seconds before reading next line
                        }
                    });

                    await Task.WhenAll(outputTask, process.WaitForExitAsync()); // Wait for process to complete

                    // Once the process is done, switch to completion page
                    InstallationPage.Visibility = Visibility.Collapsed;
                    AbortPage.Visibility = Visibility.Collapsed;
                    CompletionPage.Visibility = Visibility.Visible;

                    string error = await process.StandardError.ReadToEndAsync();
                    if (!string.IsNullOrWhiteSpace(error))
                    {
                        AppendLog($"[ERROR] {error}");
                    }
                    else
                    {
                        AppendLog("[INFO] Driver Updates Completed.");
                    }
                }
            }
            catch (Exception ex)
            {
                AppendLog($"[ERROR] Exception: {ex.Message}");
            }
        }




        private void UpdateUI(string appName)
        {
            Dispatcher.Invoke(() =>
            {
                CurrentAppText.Text = $"Installing: {appName}...";
                OutputLogBox.AppendText($"[INFO] Starting installation: {appName}\n");
                OutputLogBox.ScrollToEnd();
            });
        }
        private void AppendLog(string message)
        {
            if (!string.IsNullOrEmpty(message))
            {
                Dispatcher.Invoke(() =>
                {
                    OutputLogBox.AppendText(message + "\n");
                    OutputLogBox.ScrollToEnd();
                });
            }
        }
        private async Task RunInstallationProcessAsync(InstallationStep step, CancellationToken token)
        {
            await Task.Run(() =>
            {
                try
                {

                    if (token.IsCancellationRequested) return; // Stop if cancelled

                    ProcessStartInfo psi = new ProcessStartInfo
                    {
                        FileName = step.Command,
                        Arguments = step.Arguments,
                        RedirectStandardOutput = true,
                        RedirectStandardError = true,
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };

                    Process process = new Process { StartInfo = psi };
                    process.OutputDataReceived += (sender, e) => AppendLog(e.Data);
                    process.ErrorDataReceived += (sender, e) => AppendLog(e.Data);

                    process.Start();
                    process.BeginOutputReadLine();
                    process.BeginErrorReadLine();
                    process.WaitForExit();
                    // Wait for process to exit or cancel
                    while (!process.HasExited)
                    {
                        if (token.IsCancellationRequested)
                        {
                            AppendLog($"[INFO] Aborting: {step.Name}");
                            process.Kill(); // Force stop installation
                            return;
                        }
                        Thread.Sleep(500); // Check every 500ms
                    }
                }
                catch (Exception ex)
                {
                    AppendLog($"[ERROR] {step.Name} failed: {ex.Message}");
                }
            });
        }

        public MainWindow()
        {
            InitializeComponent();
            InitializeCategories();
            UpdateCategoryDisplay();
        }


        // Smooth Fade-In on Start
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            var fadeIn = (Storyboard)FindResource("FadeIn");
            this.BeginStoryboard(fadeIn);
        }

        // Move Window (Title Bar Dragging)
        public void MoveWindow(object sender, MouseButtonEventArgs e)
        {
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                DragMove();
            }
        }

        // Close Application
        private void CloseApp(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void InitializeCategories()
        {
            Categories = new List<Category>
            {
                new Category("MH Laptop", "data/mhlogotrsp.png", "Default software for the laptops used by managed houses.", new List<App>
                {
                    new App("Acrobat Reader", "data/acrobatreader.png"),
                    new App("Google Chrome", "data/chrome_logo.png"),
                    new App("GlobalProtect", "data/globalprotect.png"),
                    new App("Logmein", "data/logmein.png"),
                    new App("SupportAssist", "data/dell.png"),
                    new App("Dell DCU", "data/dell.png"),
                }),
                new Category("HO Laptop", "data/hologo.png", "Default software for the laptops used by Head Office.", new List<App>
                {
                    new App("Acrobat Reader", "data/acrobatreader.png"),
                    new App("Google Chrome", "data/chrome_logo.png"),
                    new App("GlobalProtect", "data/globalprotect.png"),
                    new App("Logmein", "data/logmein.png"),
                    new App("SupportAssist", "data/dell.png"),
                    new App("Office Suite", "data/officesetup.png"),
                    new App("Mimecast", "data/Mimecast_Logo.png"),
                    new App("Teams", "data/Microsoft_Office_Teams_Logo_512px.png"),
                    new App("Dell DCU", "data/dell.png"),
                })
                // Add more categories as needed
            };
        }



        private void UpdateCategoryDisplay()
        {
            if (Categories.Count == 0) return;

            Category currentCategory = Categories[currentCategoryIndex];
            CategoryImage.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(currentCategory.ImagePath, UriKind.RelativeOrAbsolute));
            CategoryTitle.Text = currentCategory.Name;
            CategoryDescription.Text= currentCategory.Description;
            UpdateIncludedApps(currentCategory.Apps);
        }

        private void UpdateIncludedApps(List<App> apps)
        {
            IncludedAppsGrid.Children.Clear();
            IncludedAppsGrid.RowDefinitions.Clear();
            IncludedAppsGrid.ColumnDefinitions.Clear();

            for (int i = 0; i < 2; i++)
                IncludedAppsGrid.RowDefinitions.Add(new RowDefinition());

            for (int i = 0; i < 5; i++)
                IncludedAppsGrid.ColumnDefinitions.Add(new ColumnDefinition());

            for (int i = 0; i < apps.Count && i < 10; i++)
            {
                int row = i / 5;
                int col = i % 5;

                StackPanel appPanel = new StackPanel { Orientation = Orientation.Vertical, HorizontalAlignment = HorizontalAlignment.Center, Margin = new Thickness() };
                Image appImage = new Image
                {
                    Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(apps[i].LogoPath, UriKind.Relative)),
                    Width = 30,
                    Height = 30
                };
                TextBlock appName = new TextBlock
                {
                    Text = apps[i].Name,
                    Foreground = System.Windows.Media.Brushes.White,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    FontSize=12,
                    TextWrapping = TextWrapping.Wrap,
                    TextAlignment = TextAlignment.Center
                };

                appPanel.Children.Add(appImage);
                appPanel.Children.Add(appName);

                Grid.SetRow(appPanel, row);
                Grid.SetColumn(appPanel, col);
                IncludedAppsGrid.Children.Add(appPanel);
            }
        }

        private void UpdateInstalledApss(List<InstallationStep> installedApps) {
            InstalledAppsGrid.Children.Clear();
            InstalledAppsGrid.RowDefinitions.Clear();
            InstalledAppsGrid.ColumnDefinitions.Clear();

            for (int i = 0; i < 2; i++)
                InstalledAppsGrid.RowDefinitions.Add(new RowDefinition());
            for (int i = 0; i < 5; i++)
                InstalledAppsGrid.ColumnDefinitions.Add(new ColumnDefinition());

            for (int i = 0; i < installedApps.Count && i < 10; i++) {
                int row = i / 5;
                int col = i % 5;

                StackPanel appPanel = new StackPanel { Orientation = Orientation.Vertical, HorizontalAlignment = HorizontalAlignment.Center, Margin = new Thickness() };
                Image appImage = new Image
                {
                    Source = new BitmapImage(new Uri(installedApps[i].LogoPath, UriKind.RelativeOrAbsolute)),
                    Width = 30,
                    Height = 30
                };
                TextBlock appName = new TextBlock
                {
                    Text = installedApps[i].Name,
                    Foreground = System.Windows.Media.Brushes.White,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    FontSize = 12,
                    TextWrapping = TextWrapping.Wrap,
                    TextAlignment = TextAlignment.Center
                };
                appPanel.Children.Add(appImage);
                appPanel.Children.Add(appName);

                Grid.SetRow(appPanel, row);
                Grid.SetColumn(appPanel, col);
                InstalledAppsGrid.Children.Add(appPanel);
            }
        }

        private void NextCategory_Click(object sender, RoutedEventArgs e)
        {
            currentCategoryIndex = (currentCategoryIndex + 1) % Categories.Count;
            ApplyFadeAnimation();
        }

        private void PreviousCategory_Click(object sender, RoutedEventArgs e)
        {
            currentCategoryIndex = (currentCategoryIndex - 1 + Categories.Count) % Categories.Count;
            ApplyFadeAnimation();
        }

        private void ApplyFadeAnimation()
        {
            DoubleAnimation fadeOut = new DoubleAnimation(1, 0, TimeSpan.FromSeconds(0.3));
            DoubleAnimation fadeIn = new DoubleAnimation(0, 1, TimeSpan.FromSeconds(0.3));

            fadeOut.Completed += (s, e) =>
            {
                UpdateCategoryDisplay();
                CategoryImage.BeginAnimation(OpacityProperty, fadeIn);
                CategoryTitle.BeginAnimation(OpacityProperty, fadeIn);
                CategoryDescription.BeginAnimation(OpacityProperty, fadeIn);
                IncludedAppsGrid.BeginAnimation(OpacityProperty, fadeIn);
            };

            CategoryImage.BeginAnimation(OpacityProperty, fadeOut);
            CategoryTitle.BeginAnimation(OpacityProperty, fadeOut);
            CategoryDescription.BeginAnimation(OpacityProperty, fadeOut);
            IncludedAppsGrid.BeginAnimation(OpacityProperty, fadeOut);
        }
    }

    public class Category
    {
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public string Description { get; set; }
        public List<App> Apps { get; set; }

        public Category(string name, string imagePath, string description, List<App> apps)
        {
            Name = name;
            ImagePath = imagePath;
            Apps = apps;
            Description = description;
        }
    }

    public class InstallationStep
    {
        public string Name { get; set; }
        public string Command { get; set; }
        public string Arguments { get; set; }
        public string LogoPath { get; set; } // ✅ Add this

        public InstallationStep(string name, string command, string arguments, string logoPath)
        {
            Name = name;
            Command = command;
            Arguments = arguments;
            LogoPath = logoPath;
        }
    }

    public class App
    {
        public string Name { get; set; }
        public string LogoPath { get; set; }

        public string installPath { get; set; } 

        public string installCommands { get; set; } 

        public App(string name, string logoPath)
        {
            Name = name;
            LogoPath = logoPath;
        }
    }
}
