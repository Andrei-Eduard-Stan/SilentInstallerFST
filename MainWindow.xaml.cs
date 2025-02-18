using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace SilentInstaller
{
    public partial class MainWindow : Window
    {
        private List<Category> Categories;
        private int currentCategoryIndex = 0;

        public MainWindow()
        {
            InitializeComponent();
            InitializeCategories();
            UpdateCategoryDisplay();
        }

        private void InitializeCategories()
        {
            Categories = new List<Category>
            {
                new Category("MH Laptop", "data/mhlogo.png", "Default software for the laptops used by managed houses.", new List<App>
                {
                    new App("Acrobat Reader", "data/acrobatreader.png"),
                    new App("Google Chrome", "data/chrome_logo.png"),
                    new App("GlobalProtect", "data/globalprotect.png"),
                    new App("Logmein", "data/logmein.png"),
                    new App("SupportAssist", "data/dell.png"),
                }),
                new Category("HO Laptop", "data/mhlogo.png", "Default software for the laptops used by Head Office.", new List<App>
                {
                    new App("Acrobat Reader", "data/acrobatreader.png"),
                    new App("Google Chrome", "data/chrome_logo.png"),
                    new App("GlobalProtect", "data/globalprotect.png"),
                    new App("Logmein", "data/logmein.png"),
                    new App("SupportAssist", "data/dell.png"),
                    new App("VLC", "data/girl.png"),
                    new App("WinRAR", "data/girl.png"),
                    new App("Chrome1", "data/girl.png"),
                    new App("Notepad1++", "data/girl.png"),
                    new App("7-Zip1", "data/girl.png"),
                    new App("VLC1", "data/girl.png"),
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

                StackPanel appPanel = new StackPanel { Orientation = Orientation.Vertical, HorizontalAlignment = HorizontalAlignment.Center };
                Image appImage = new Image
                {
                    Source = new System.Windows.Media.Imaging.BitmapImage(new Uri(apps[i].LogoPath, UriKind.Relative)),
                    Width = 50,
                    Height = 50
                };
                TextBlock appName = new TextBlock
                {
                    Text = apps[i].Name,
                    Foreground = System.Windows.Media.Brushes.White,
                    HorizontalAlignment = HorizontalAlignment.Center
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

    public class App
    {
        public string Name { get; set; }
        public string LogoPath { get; set; }

        public App(string name, string logoPath)
        {
            Name = name;
            LogoPath = logoPath;
        }
    }
}
