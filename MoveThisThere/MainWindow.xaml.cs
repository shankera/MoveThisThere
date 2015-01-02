using System.Linq;
using System.Windows;
using System.Windows.Controls;

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
                "24"
            };

        public MainWindow()
        {
            InitializeUserInterface();
            InitializeComponent();
        }

        private void InitializeUserInterface()
        {
            panel = new StackPanel();

            AddFields(null, null);
            AddFields(null, null);
            AddFields(null, null);
            Content = panel;
        }

        private void AddFields(object sender, RoutedEventArgs args)
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
            Grid.SetRow(plusBtn, 0);
            var minusBtn = new Button();
            Grid.SetColumn(minusBtn, 0);
            Grid.SetRow(minusBtn, 1);
            var sourceLabel = new Label();
            Grid.SetColumn(sourceLabel, 1);
            Grid.SetRow(sourceLabel, 0);
            var destLabel = new Label();
            Grid.SetColumn(destLabel, 1);
            Grid.SetRow(destLabel, 1);
            var sourceBox = new TextBox();
            Grid.SetColumn(sourceBox, 2);
            Grid.SetRow(sourceBox, 0);
            var destBox = new TextBox();
            Grid.SetColumn(destBox, 2);
            Grid.SetRow(destBox, 1);
            var sourceBtn = new Button();
            Grid.SetColumn(sourceBtn, 3);
            Grid.SetRow(sourceBtn, 0);
            var destBtn = new Button();
            Grid.SetColumn(destBtn, 3);
            Grid.SetRow(destBtn, 1);
            grid.Children.Add(plusBtn);
            grid.Children.Add(minusBtn);
            grid.Children.Add(sourceLabel);
            grid.Children.Add(destLabel);
            grid.Children.Add(sourceBox);
            grid.Children.Add(destBox);
            grid.Children.Add(sourceBtn);
            grid.Children.Add(destBtn);
            panel.Children.Add(grid);
        }
    }
}
