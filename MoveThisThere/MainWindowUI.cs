using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MoveThisThere
{
    partial class MainWindow
    {
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

            var headerLabel = new Label { FontWeight = FontWeights.Bold };
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

            var sourceLabel = new Label { Content = "Source Directory:" };
            Grid.SetColumn(sourceLabel, 1);
            Grid.SetRow(sourceLabel, 1);

            var destLabel = new Label { Content = "Destination Directory:" };
            Grid.SetColumn(destLabel, 1);
            Grid.SetRow(destLabel, 2);

            Grid.SetColumn(pathStrings.SourceBox, 2);
            Grid.SetRow(pathStrings.SourceBox, 1);

            Grid.SetColumn(pathStrings.DestinationBox, 2);
            Grid.SetRow(pathStrings.DestinationBox, 2);

            var sourceBtn = new Button { Content = "Select", Name = "sourceButton" };
            sourceBtn.Click += OnClick;
            Grid.SetColumn(sourceBtn, 3);
            Grid.SetRow(sourceBtn, 1);

            var destBtn = new Button { Content = "Select", Name = "destinationButton" };
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
    }
}
