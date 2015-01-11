using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using MoveThisThere.Lib;
using MoveThisThere.Properties;
using Button = System.Windows.Controls.Button;
using Label = System.Windows.Controls.Label;

namespace MoveThisThere
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private new const string MinHeight = "26";
        private const string LabelSeperator = " --> ";
        private static readonly string[] ColWidths =
            {
                MinHeight,
                "125",
                "1*",
                "60"
            };
        private static readonly string[] RowHeights =
            {
                MinHeight,
                MinHeight,
                MinHeight
            };

        private static Dictionary<Grid, PathStrings> _grids;
        private static Dictionary<Grid, Label> _labels;
        private readonly StackPanel _gridPanel;

        public MainWindow()
        {
            _gridPanel = new StackPanel();
            _grids = new Dictionary<Grid, PathStrings>();
            _labels = new Dictionary<Grid, Label>();
            var sourcePaths = Settings.Default.SourcePaths.Split(',');
            var destinationPaths = Settings.Default.DestinationPaths.Split(',');
            var total = String.IsNullOrWhiteSpace(Settings.Default.Fields) ? 1 : Int32.Parse(Settings.Default.Fields);
            for (var x = 0; x < total; x++)
            {
                var pathStrings = new PathStrings(sourcePaths[x], destinationPaths[x]);
                var grid = AddFields(pathStrings);
                _grids.Add(grid, pathStrings);
                _gridPanel.Children.Add(grid);
                UpdateLabels(grid);
            }

            var bigGrid = new Grid();
            var glc = new GridLengthConverter();
            var convertFromString = glc.ConvertFromString("1*");
            if (convertFromString != null)
                bigGrid.RowDefinitions.Add(new RowDefinition { Height = (GridLength)convertFromString });
            var fromString = glc.ConvertFromString(MinHeight);
            if (fromString != null)
                bigGrid.RowDefinitions.Add(new RowDefinition { Height = (GridLength)fromString });
            if (convertFromString != null)
                bigGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = (GridLength)convertFromString });
            bigGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(75) });

            var sv = new ScrollViewer { Content = _gridPanel, VerticalScrollBarVisibility = ScrollBarVisibility.Auto };
            bigGrid.Children.Add(sv);
            Grid.SetRow(sv, 0);
            Grid.SetColumnSpan(sv, 2);

            var doButton = new Button {Content = "Do"};
            doButton.Click += DoOnClick;
            bigGrid.Children.Add(doButton);
            Grid.SetRow(doButton, 1);
            Grid.SetColumn(doButton, 1);

            Content = bigGrid;
            InitializeComponent();
            Closed += OnWindowClosing;
        }

        private static void DoOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            Console.Out.WriteLine("click");
            foreach (var pathStrings in _grids)
            {
                Console.Out.WriteLine("loop");
                FileMover.Move(pathStrings.Value.SourcePath, pathStrings.Value.DestinationPath);
            }
        }

        public void OnWindowClosing(object sender, EventArgs eventArgs)
        {
            Settings.Default.Fields = _grids.Count.ToString(CultureInfo.InvariantCulture);
            var sources = new string[_grids.Count];
            var destinations = new string[_grids.Count];
            var increment = 0;
            foreach (var grid in _grids)
            {
                sources[increment] = grid.Value.SourcePath;
                destinations[increment] = grid.Value.DestinationPath;
                increment++;
            }
            Settings.Default.SourcePaths = string.Join(",", sources);
            Settings.Default.DestinationPaths = string.Join(",", destinations);
            Settings.Default.Save();
        }


        private static void OnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            var folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;
            var button = sender as Button;
            if (button != null && button.Name.Equals("sourceButton"))
            {
                _grids[(Grid) button.Parent].SourcePath = folderBrowserDialog.SelectedPath;
                UpdateLabels((Grid)button.Parent);

            }
            else if (button != null && button.Name.Equals("destinationButton"))
            {
                _grids[(Grid)button.Parent].DestinationPath = folderBrowserDialog.SelectedPath;
                UpdateLabels((Grid)button.Parent);
            }
        }

        private static void UpdateLabels(Grid grid)
        {
            _labels[grid].Content = _grids[grid].SourcePath + LabelSeperator + _grids[grid].DestinationPath;
            if (_labels[grid].Content.Equals(LabelSeperator))
                _labels[grid].Content = "Unspecified";
        }

        private void AddFields(object sender, RoutedEventArgs args)
        {
            var ps = new PathStrings();
            var grid = AddFields(ps);
            _grids.Add(grid, ps);
            _gridPanel.Children.Add(grid);
            UpdateLabels(grid);
        }

        private void RemoveFields(object sender, RoutedEventArgs args)
        {
            if (_grids.Count == 1) return;
            var x = (Button) sender;

            if (!_labels[(Grid)x.Parent].Content.Equals("Unspecified"))
            {
                var messageBoxResult = System.Windows.MessageBox.Show("Are you sure you would like to delete this?",
                    "Delete Confirmation", MessageBoxButton.YesNo);
                if (messageBoxResult != MessageBoxResult.Yes) return;
            }

            var y = (Grid)x.Parent;
            _grids.Remove(y);
            foreach (var pathStringse in _grids)
            {
                Console.Out.WriteLine(pathStringse.ToString());
            }
            _gridPanel.Children.Remove(y);
        }
    }
}
