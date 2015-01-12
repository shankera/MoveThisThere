using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;
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


        private static void UpdateLabels(Grid grid)
        {
            _labels[grid].Content = _grids[grid].SourcePath + LabelSeperator + _grids[grid].DestinationPath;
            if (_labels[grid].Content.Equals(LabelSeperator))
                _labels[grid].Content = "Unspecified";
        }

    }
}
