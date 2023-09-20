using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Data;

namespace SvgIconViewer
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoadIcons(string text)
        {
            DirectoryInfo dir = new(text);
            var svgs = dir.GetFiles("*.svg", SearchOption.AllDirectories);
            List<IconViewModel> vms = new();
            foreach (var svg in svgs)
            {
                vms.Add(new IconViewModel(svg.FullName));
            }

            CollectionViewSource viewSource = new()
            {
                Source = vms
            };
            viewSource.GroupDescriptions.Add(new PropertyGroupDescription("Collection"));

            this.iconList.ItemsSource = viewSource.View;
        }

        private void BrowseSource(object sender, RoutedEventArgs e)
        {
            Ookii.Dialogs.Wpf.VistaFolderBrowserDialog dialog = new();
            if (dialog.ShowDialog(this).GetValueOrDefault())
            {
                this.LoadIcons(dialog.SelectedPath);
            }
        }
    }
}
