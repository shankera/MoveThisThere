using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using MoveThisThere.Lib;
using Button = System.Windows.Controls.Button;
namespace MoveThisThere
{
    partial class MainWindow
    {

        private static void DoOnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            var x = new[]{0,0,0};
            foreach (var pathStrings in _grids)
            {
                x = FileMover.Move(pathStrings.Value.SourcePath, pathStrings.Value.DestinationPath);
            }

            var messageBoxResult = System.Windows.MessageBox.Show(x[0] + " copied, " + x[1] + " overwritten, " + x[2] + " total.",
                "Delete Confirmation", MessageBoxButton.OK);
        }

        private static void OnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            var folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() != System.Windows.Forms.DialogResult.OK) return;
            var button = sender as Button;
            if (button != null && button.Name.Equals("sourceButton"))
            {
                _grids[(Grid)button.Parent].SourcePath = folderBrowserDialog.SelectedPath;
                UpdateLabels((Grid)button.Parent);

            }
            else if (button != null && button.Name.Equals("destinationButton"))
            {
                _grids[(Grid)button.Parent].DestinationPath = folderBrowserDialog.SelectedPath;
                UpdateLabels((Grid)button.Parent);
            }
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
            var x = (Button)sender;

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
