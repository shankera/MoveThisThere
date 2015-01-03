using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MoveThisThere.Properties;

namespace MoveThisThere
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private static readonly string[] ColWidths =
            {
                "24",
                "125",
                "1*",
                "60"
            };
        private static readonly string[] RowHeights =
            {
                "24",
                "24",
                "24"
            };

        private static Dictionary<Grid, PathStrings> _grids;
        private StackPanel _panel;

        public MainWindow()
        {
            InitializeUserInterface();
            Content = _panel;
            InitializeComponent();
            Closed += OnWindowClosing;
        }

        private void InitializeUserInterface()
        {
            _panel = new StackPanel();
            _grids = new Dictionary<Grid, PathStrings>();
            var sourcePaths = Settings.Default.SourcePaths.Split(',');
            var destinationPaths = Settings.Default.DestinationPaths.Split(',');
            var total = Settings.Default.Fields.Equals("") ? 1 : Int32.Parse(Settings.Default.Fields);
            for (var x = 0; x < total; x++)
            {
                var grid = AddFields(sourcePaths[x], destinationPaths[x]);
                _grids.Add(grid, new PathStrings(sourcePaths[x], destinationPaths[x]));
                _panel.Children.Add(grid);
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

        private Grid AddFields(string sourcePath, string destPath)
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

            var plusBtn = new Button();
            plusBtn.Click += AddFields;
            Grid.SetColumn(plusBtn, 0);
            Grid.SetRow(plusBtn, 1);

            var minusBtn = new Button();
            minusBtn.Click += RemoveFields;
            Grid.SetColumn(minusBtn, 0);
            Grid.SetRow(minusBtn, 2);

            var sourceLabel = new Label {Content = "Source Directory:"};
            Grid.SetColumn(sourceLabel, 1);
            Grid.SetRow(sourceLabel, 1);

            var destLabel = new Label {Content = "Destination Directory:"};
            Grid.SetColumn(destLabel, 1);
            Grid.SetRow(destLabel, 2);

            var sourceBox = new TextBox {Text = sourcePath, Name = "sourceDir"};
            sourceBox.TextChanged += textChangedEventHandler;
            Grid.SetColumn(sourceBox, 2);
            Grid.SetRow(sourceBox, 1);

            var destBox = new TextBox { Text = destPath, Name = "destDir" };
            destBox.TextChanged += textChangedEventHandler;
            Grid.SetColumn(destBox, 2);
            Grid.SetRow(destBox, 2);

            var sourceBtn = new Button {Content = "Select"};
            Grid.SetColumn(sourceBtn, 3);
            Grid.SetRow(sourceBtn, 1);

            var destBtn = new Button {Content = "Select"};
            Grid.SetColumn(destBtn, 3);
            Grid.SetRow(destBtn, 2);

            grid.Children.Add(plusBtn);
            grid.Children.Add(minusBtn);
            grid.Children.Add(sourceLabel);
            grid.Children.Add(destLabel);
            grid.Children.Add(sourceBox);
            grid.Children.Add(destBox);
            grid.Children.Add(sourceBtn);
            grid.Children.Add(destBtn);

            return grid;
        }

        private static void textChangedEventHandler(object sender, TextChangedEventArgs args)
        {
            var textBox = sender as TextBox;
            if (textBox != null && textBox.Name.Equals("sourceDir"))
            {
                _grids[(Grid)textBox.Parent].SourcePath = textBox.Text;
            } 
            else if (textBox != null && textBox.Name.Equals("destDir"))
            {
                _grids[(Grid)textBox.Parent].DestinationPath = textBox.Text;
            }
        }

        private void AddFields(object sender, RoutedEventArgs args)
        {
            var x = AddFields("","");
            _grids.Add(x, new PathStrings("",""));
            _panel.Children.Add(x);
        }

        private void RemoveFields(object sender, RoutedEventArgs args)
        {
            if (_grids.Count == 1) return;
            var x = (Button) sender;
            var y = (Grid) VisualTreeHelper.GetParent(x);
            _grids.Remove(y);
            _panel.Children.Remove(y);
        }
    }
}
