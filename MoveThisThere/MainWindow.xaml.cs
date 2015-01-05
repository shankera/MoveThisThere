using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Shapes;
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
            var sv = new ScrollViewer { Content = _gridPanel, VerticalScrollBarVisibility = ScrollBarVisibility.Auto };
            bigGrid.Children.Add(sv);
            Grid.SetRow(sv, 0);
            var butt = new Button {Content = "Do"};
            bigGrid.Children.Add(butt);
            Grid.SetRow(butt, 1);
            Content = bigGrid;
            InitializeComponent();
            Closed += OnWindowClosing;
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

        private Grid AddFields(PathStrings pathStrings)
        {
            var myGridLengthConverter = new GridLengthConverter();
            var grid = new Grid();

            foreach (var col in from width in ColWidths
                                select myGridLengthConverter.ConvertFromString(width) into x
                                where x != null
                                select new ColumnDefinition { Width = (GridLength)x })
            {
                grid.ColumnDefinitions.Add(col);
            }

            foreach (var row in from height in RowHeights
                                select myGridLengthConverter.ConvertFromString(height) into x
                                where x != null
                                select new RowDefinition { Height = (GridLength)x })
            {
                grid.RowDefinitions.Add(row);
            }

            var headerLabel = new Label{FontWeight = FontWeights.Bold};
            _labels.Add(grid, headerLabel);
            Grid.SetColumn(headerLabel, 1);
            Grid.SetRow(headerLabel, 0);
            Grid.SetColumnSpan(headerLabel, 3);

            var plusBtn = new Button();
            plusBtn.Click += AddFields;
            Grid.SetColumn(plusBtn, 0);
            Grid.SetRow(plusBtn, 1);

            var plus = new Polygon
            {
                Fill = Brushes.White,
                Stroke = Brushes.Black,
                StrokeThickness = 0.5,
                IsHitTestVisible = false,
                Points = new PointCollection
                {
                    new Point(3, 10),
                    new Point(10, 10),
                    new Point(10, 3),
                    new Point(16, 3),
                    new Point(16, 10),
                    new Point(23, 10),
                    new Point(23, 16),
                    new Point(16, 16),
                    new Point(16, 23),
                    new Point(10, 23),
                    new Point(10, 16),
                    new Point(3, 16)
                }
            };
            Grid.SetColumn(plus, 0);
            Grid.SetRow(plus, 1);

            var minusBtn = new Button();
            minusBtn.Click += RemoveFields;
            Grid.SetColumn(minusBtn, 0);
            Grid.SetRow(minusBtn, 2);

            var minus = new Polygon
            {
                Fill = Brushes.White,
                Stroke = Brushes.Black,
                StrokeThickness = 0.5,
                IsHitTestVisible = false,
                Points = new PointCollection
                {
                    new Point(3, 10),
                    new Point(23, 10),
                    new Point(23, 16),
                    new Point(3, 16)
                }
            };
            Grid.SetColumn(minus, 0);
            Grid.SetRow(minus, 2);

            var sourceLabel = new Label {Content = "Source Directory:"};
            Grid.SetColumn(sourceLabel, 1);
            Grid.SetRow(sourceLabel, 1);

            var destLabel = new Label {Content = "Destination Directory:"};
            Grid.SetColumn(destLabel, 1);
            Grid.SetRow(destLabel, 2);

            Grid.SetColumn(pathStrings.SourceBox, 2);
            Grid.SetRow(pathStrings.SourceBox, 1);

            Grid.SetColumn(pathStrings.DestinationBox, 2);
            Grid.SetRow(pathStrings.DestinationBox, 2);

            var sourceBtn = new Button {Content = "Select", Name = "sourceButton"};
            sourceBtn.Click += OnClick;
            Grid.SetColumn(sourceBtn, 3);
            Grid.SetRow(sourceBtn, 1);

            var destBtn = new Button { Content = "Select", Name = "destinationButton"};
            destBtn.Click += OnClick;
            Grid.SetColumn(destBtn, 3);
            Grid.SetRow(destBtn, 2);

            grid.Children.Add(headerLabel);
            grid.Children.Add(plusBtn);
            grid.Children.Add(plus);
            grid.Children.Add(minusBtn);
            grid.Children.Add(minus);
            grid.Children.Add(sourceLabel);
            grid.Children.Add(destLabel);
            grid.Children.Add(pathStrings.SourceBox);
            grid.Children.Add(pathStrings.DestinationBox);
            grid.Children.Add(sourceBtn);
            grid.Children.Add(destBtn);

            return grid;
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
            _gridPanel.Children.Remove(y);
        }
    }
}
