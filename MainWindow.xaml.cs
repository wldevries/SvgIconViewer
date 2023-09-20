using IconCatalog.Properties;
using Ookii.Dialogs.Wpf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace IconCatalog
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly DispatcherTimer UpdateFindTimer;

        public MainWindow()
        {
            InitializeComponent();

            this.ViewSource = new();
            this.ViewSource.GroupDescriptions.Add(new PropertyGroupDescription("Collection"));
            this.ViewSource.Filter += ViewSource_Filter;

            this.Width = Settings.Default.WindowWidth;
            this.Height = Settings.Default.WindowHeight;

            this.UpdateFindTimer = new DispatcherTimer(
                TimeSpan.FromMilliseconds(200),
                DispatcherPriority.Normal,
                UpdateView,
                this.Dispatcher);

            this.Loaded += MainWindow_Loaded;
            this.SizeChanged += MainWindow_SizeChanged;
            this.FindBox.PreviewKeyDown += FindBox_PreviewKeyDown;
        }

        public CollectionViewSource ViewSource { get; }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.UpdateDarkMade();
            this.OutlineToggle.IsChecked = Settings.Default.ShowOutline;
            this.IconSizeSlider.Value = Settings.Default.IconSize;
            this.LoadIcons(Settings.Default.CatalogPath);
        }

        private void MainWindow_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            Settings.Default.WindowWidth = this.Width;
            Settings.Default.WindowHeight = this.Height;
            Settings.Default.Save();
        }

        private bool LoadIcons(string text)
        {
            try
            {
                DirectoryInfo dir = new(text);
                if (!dir.Exists)
                {
                    return false;
                }

                var icons = dir.GetFiles("*.svg", SearchOption.AllDirectories)
                    .Concat(dir.GetFiles("*.png", SearchOption.AllDirectories))
                    .OrderBy(f => f.FullName);

                List<IIconViewModel> vms = new();
                foreach (var icon in icons)
                {
                    if (icon.Extension.ToLower() is ".svg")
                    {
                        vms.Add(new SvgIconViewModel(icon.FullName));
                    }
                    else
                        vms.Add(new RasterIconViewModel(icon.FullName));
                }

                this.ViewSource.Source = vms;

                this.iconList.ItemsSource = ViewSource.View;

                this.Title = $"Icon Catalog - {dir.FullName}";
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show("Failed to load icons." + Environment.NewLine + e.ToString());
                return false;
            }
        }

        private void UpdateDarkMade()
        {
            if (this.DarkToggle.IsChecked)
            {
                this.Background = new SolidColorBrush(Color.FromRgb(0x33, 0x33, 0x33));
                this.Foreground = Brushes.White;
            }
            else
            {
                this.Background = Brushes.White;
                this.Foreground = Brushes.Black;
            }
        }

        private void BrowseSource(object sender, RoutedEventArgs e)
        {
            VistaFolderBrowserDialog dialog = new();
            if (dialog.ShowDialog(this).GetValueOrDefault())
            {
                if (this.LoadIcons(dialog.SelectedPath))
                {
                    this.FindBox.Text = string.Empty;
                    Settings.Default.CatalogPath = dialog.SelectedPath;
                    Settings.Default.Save();
                }
            }
        }

        private void ToggleDarkMode(object sender, ExecutedRoutedEventArgs e)
        {
            this.DarkToggle.IsChecked = !this.DarkToggle.IsChecked;
            this.ToggleDarkModeClicked(sender, e);
        }

        private void ToggleDarkModeClicked(object sender, RoutedEventArgs e)
        {
            Settings.Default.DarkMode = this.DarkToggle.IsChecked;
            Settings.Default.Save();
            UpdateDarkMade();
        }

        private void ToggleOutline(object sender, ExecutedRoutedEventArgs e)
        {
            this.OutlineToggle.IsChecked = !this.OutlineToggle.IsChecked;
            this.ToggleOutlineClicked(sender, e);
        }

        private void ToggleOutlineClicked(object sender, RoutedEventArgs e)
        {
            Settings.Default.ShowOutline = this.OutlineToggle.IsChecked;
            Settings.Default.Save();
        }

        private void SaveIconSize(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Settings.Default.IconSize = (int)this.IconSizeSlider.Value;
            Settings.Default.Save();
        }

        private void UpdateFind(object sender, TextChangedEventArgs e)
        {
            this.UpdateFindTimer.Stop();
            this.UpdateFindTimer.Start();
        }

        private void UpdateView(object? sender, EventArgs e)
        {
            this.UpdateFindTimer.Stop();
            this.ViewSource.View.Refresh();
        }

        private void ViewSource_Filter(object sender, FilterEventArgs e)
        {
            var icon = e.Item as IIconViewModel;
            if (icon is null)
            {
                e.Accepted = false;
                return;
            }

            var terms = this.FindBox.Text.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            e.Accepted = terms.All(term => icon.Path.Contains(term, StringComparison.OrdinalIgnoreCase));
        }

        private void HandleExit(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void FocusFind(object sender, ExecutedRoutedEventArgs e)
        {
            this.FindLabel.Visibility = Visibility.Visible;
            this.FindBox.Visibility = Visibility.Visible;
            this.FindBox.MoveFocus(new TraversalRequest(FocusNavigationDirection.First));
        }

        private void FindBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                this.FindBox.Text = string.Empty;
                this.FindLabel.Visibility = Visibility.Collapsed;
                this.FindBox.Visibility = Visibility.Collapsed;
                e.Handled = true;
            }
        }

        private void ToggleColors(object sender, ExecutedRoutedEventArgs e)
        {
            this.OriginalColorsToggle.IsChecked = !this.OriginalColorsToggle.IsChecked;
        }

        private void FindLostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(this.FindBox.Text))
            {
                this.FindBox.Text = string.Empty;
                this.FindLabel.Visibility = Visibility.Collapsed;
                this.FindBox.Visibility = Visibility.Collapsed;
            }
        }

        private void ScrollPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftCtrl) || Keyboard.IsKeyDown(Key.RightCtrl))
            {
                double change = this.IconSizeSlider.SmallChange;

                if (Keyboard.IsKeyDown(Key.LeftAlt) || Keyboard.IsKeyDown(Key.RightAlt))
                {
                    change = this.IconSizeSlider.LargeChange;
                }

                // Mouse wheel delta is always 120
                this.IconSizeSlider.Value += change * e.Delta / 120;
                e.Handled = true;
            }
        }
    }
}
