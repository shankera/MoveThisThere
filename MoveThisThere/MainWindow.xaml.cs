using System;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using MoveThisThere.Lib;
using MoveThisThere.Properties;

namespace MoveThisThere
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        internal static StackPanel _gridPanel;

        public MainWindow()
        {
            InitializeComponent();
            _gridPanel = new StackPanel();
            var sourcePaths = Settings.Default.SourcePaths.Split(',');
            var destinationPaths = Settings.Default.DestinationPaths.Split(',');
            var total = String.IsNullOrWhiteSpace(Settings.Default.Fields) ? 1 : Int32.Parse(Settings.Default.Fields);
            for (var x = 0; x < total; x++)
            {
                var pathStrings = new PathStrings(sourcePaths[x], destinationPaths[x]);
                var field = new Field(pathStrings);
                _gridPanel.Children.Add(field);
            }
            FieldScroller.Content = _gridPanel;

            Content = BigGrid;
            Closed += OnWindowClosing;
        }

        public void OnWindowClosing(object sender, EventArgs eventArgs)
        {
            var count = _gridPanel.Children.Count;
            Settings.Default.Fields = count.ToString(CultureInfo.InvariantCulture);
            var sources = new string[count];
            var destinations = new string[count];
            var increment = 0;
            foreach (Field grid in _gridPanel.Children)
            {
                sources[increment] = grid.SourceBox.Text;
                destinations[increment] = grid.DestinationBox.Text;
                increment++;
            }
            Settings.Default.SourcePaths = string.Join(",", sources);
            Settings.Default.DestinationPaths = string.Join(",", destinations);
            Settings.Default.Save();
        }

        private void DoOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            var x = new[] { 0, 0, 0 };
            foreach (Field field in _gridPanel.Children)
            {
                x = FileMover.Move(field.SourceBox.Text, field.DestinationBox.Text);
            }

            var messageBoxResult = System.Windows.MessageBox.Show(x[0] + " copied, " + x[1] + " overwritten, " + x[2] + " total.",
                "Delete Confirmation", MessageBoxButton.OK);
        }


    }
}
