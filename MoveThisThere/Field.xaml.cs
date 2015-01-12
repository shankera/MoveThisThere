using System.Windows;
using System.Windows.Forms;
using Button = System.Windows.Controls.Button;
using UserControl = System.Windows.Controls.UserControl;

namespace MoveThisThere
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class Field : UserControl
    {
        private const string LabelSeperator = " --> ";
        public Field(PathStrings pathStrings)
        {
            InitializeComponent();
            SourceBox.Text = pathStrings.SourcePath;
            DestinationBox.Text = pathStrings.DestinationPath;
            UpdateLabels();
        }
        

        private void AddFields(object sender, RoutedEventArgs args)
        {
            var ps = new PathStrings();
            var field = new Field(ps);
            MainWindow._gridPanel.Children.Add(field);
            UpdateLabels();
        }
        
        private void RemoveFields(object sender, RoutedEventArgs args)
        {
            if (MainWindow._gridPanel.Children.Count == 1) return;
            
            if (!HeaderLabel.Content.Equals("Unspecified"))
            {
                var messageBoxResult = System.Windows.MessageBox.Show("Are you sure you would like to delete this?",
                    "Delete Confirmation", MessageBoxButton.YesNo);
                if (messageBoxResult != MessageBoxResult.Yes) return;
            }

            MainWindow._gridPanel.Children.Remove(this);
            
        }

        private void OnClick(object sender, RoutedEventArgs routedEventArgs)
        {
            var folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() != DialogResult.OK) return;
            var button = sender as Button;
            if (button != null && button.Equals(SourceButton))
            {
                SourceBox.Text = folderBrowserDialog.SelectedPath;
                UpdateLabels();
            }
            else if (button != null && button.Equals(DestinationButton))
            {
                DestinationBox.Text = folderBrowserDialog.SelectedPath;
                UpdateLabels();
            }
        }

        private void UpdateLabels()
        {
            HeaderLabel.Content = SourceBox.Text + LabelSeperator + DestinationBox.Text;
            if (HeaderLabel.Content.Equals(LabelSeperator))
                HeaderLabel.Content = "Unspecified";
        }
    }
}
