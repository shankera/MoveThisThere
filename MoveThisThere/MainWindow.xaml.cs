using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace MoveThisThere
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private StackPanel panel;
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

        private List<Grid> _grids; 
        public MainWindow()
        {
            InitializeUserInterface();
            InitializeComponent();
        }

        private void InitializeUserInterface()
        {
            panel = new StackPanel();
            _grids = new List<Grid>();
            var z = AddFields();
            _grids.Add(z);
            panel.Children.Add(z);
            var y = AddFields();
            _grids.Add(y);
            panel.Children.Add(y);
            var x = AddFields();
            _grids.Add(x);
            panel.Children.Add(x);
            Content = panel;
        }

        private Grid AddFields()
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

            var sourceBox = new TextBox();
            Grid.SetColumn(sourceBox, 2);
            Grid.SetRow(sourceBox, 1);

            var destBox = new TextBox();
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

        private void AddFields(object sender, RoutedEventArgs args)
        {
            var x = AddFields();
            _grids.Add(x);
            panel.Children.Add(x);
        }

        private void RemoveFields(object sender, RoutedEventArgs args)
        {
            if (_grids.Count == 1) return;
            var x = (Button) sender;
            var y = (Grid) VisualTreeHelper.GetParent(x);
            _grids.Remove(y);
            panel.Children.Remove(y);
        }
    }
}
