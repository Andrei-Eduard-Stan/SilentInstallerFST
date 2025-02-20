using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using MahApps.Metro.Controls;

namespace SilentInstaller
{
    public partial class MainWindow
    {
        private List<Category> Categories;
        private List<InstallationStep> installationSteps;
        private int currentStepIndex = 0;
        private int currentCategoryIndex = 0;
        private string installerPath = @"C:\\Users\\asta\\OneDrive - Fuller Smith and Turner\\Documents\\projects\\silentinstaller";
        
        private void DefineInstallationSteps(Category category)
        {
            installationSteps = new List<InstallationStep>();
            installationSteps.Add(new InstallationStep("Acrobat Reader", $"{installerPath}\\Acrobat\\acrsetup.exe","/sAll /rs /msi EULA_ACCEPT=YES"));
            installationSteps.Add(new InstallationStep("Google Chrome","msiexec", $"/i \"{installerPath}\\Chrome\\Installers\\GoogleChromeStandaloneEnterprise64.msi\" /quiet /norestart"));
            installationSteps.Add(new InstallationStep("GlobalProtect","msiexec", $"/i \"{installerPath}\\PaloAlto\\GlobalProtect64-6.0.1.msi\" /quiet /norestart"));
            installationSteps.Add(new InstallationStep("SupportAssist","msiexec", $"/i {installerPath}\\SupportAssist\\SupportAssistx64-4.6.3.23467.msi\" /quiet /norestart"));

            if (category.Name == "MH Laptop") {
                installationSteps.Add(new InstallationStep("Logmein MH","msiexec", $"/i \"{installerPath}\\LMI\\logmein.msi\", /quiet DEPLOYID=01_p7xqfoq7wc6kh6vcw4d007hp3hb1mgk5bm79z INSTALLMETHOD=5 FQDNDESC=1"));
            }

            if (category.Name == "HO Laptop")
            {
                installationSteps.Add(new InstallationStep("Logmein HO", "msiexec", $"/i \"{installerPath}\\LMI\\LMI Head Office.msi\" /quiet DEPLOYID=01_p7xqfoq7wc6kh6vcw4d007hp3hb1mgk5bm79z INSTALLMETHOD=5 FQDNDESC=1"));
                installationSteps.Add(new InstallationStep("Office Suite", $"{installerPath}\\Office\\setup.exe", $"/configure \"{installerPath}\\Office\\configuration-Office365-x64.xml\""));
                installationSteps.Add(new InstallationStep("Mimecast","msiexec", $"/i \"{installerPath}\\Mimecast\\Mimecast.msi\" /quiet /norestart"));
            }

        }

        private async void StartInstallation_Click(object sender, RoutedEventArgs e)
        {
            SelectionPage.Visibility = Visibility.Collapsed;
            InstallationPage.Visibility = Visibility.Visible;

            Category selectedCategory = Categories[currentCategoryIndex];
            DefineInstallationSteps(selectedCategory);

            await InstallApplicationsAsync();
        }

        private async Task InstallApplicationsAsync()
        {
            foreach (var step in installationSteps)
            {
                UpdateUI(step.Name);
                await RunInstallationProcessAsync(step);
            }

            // Move to completion page
            InstallationPage.Visibility = Visibility.Collapsed;
            CompletionPage.Visibility = Visibility.Visible;
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
        private async Task RunInstallationProcessAsync(InstallationStep step)
        {
            await Task.Run(() =>
            {
                try
                {
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
                })
                // Add more categories as needed
            };
        }



        private void UpdateCategoryDisplay()
        {
            if (Categories.Count == 0) return;

            Category currentCategory = Categories[currentCategoryIndex];
            CategoryImage.Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(currentCategory.ImagePath, UriKind.Relative));
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

        public InstallationStep(string name, string command, string arguments)
        {
            Name = name;
            Command = command;
            Arguments = arguments;
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
