using System;
using System.Windows;

namespace SilentInstaller
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void PreviousInstaller_Click(object sender, RoutedEventArgs e)
        {
            // Logic to switch to previous installer type
            //InstallerType.Text = "Avon Data";
        }

        private void NextInstaller_Click(object sender, RoutedEventArgs e)
        {
            // Logic to switch to next installer type
            //InstallerType.Text = "Head Office";
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            // Switch to the installation page
            SelectionPage.Visibility = Visibility.Collapsed;
            InstallationPage.Visibility = Visibility.Visible;

            // Start installation process (you'll need to add the actual logic)
            //StatusMessage.Text = "Installation in progress...";
        }

        private void AbortButton_Click(object sender, RoutedEventArgs e)
        {
            // Logic to abort installation
            MessageBox.Show("Installation Aborted!", "Silent Installer", MessageBoxButton.OK, MessageBoxImage.Warning);
        }
    }
}
