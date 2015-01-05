using System.Dynamic;
using System.Windows.Controls;

namespace MoveThisThere
{
    class PathStrings
    {
        public PathStrings()
        {
            SetParams("","");
        }

        public PathStrings(string source, string destination)
        {
            SetParams(source, destination);
        }

        private void SetParams(string source, string destination)
        {
            SourceBox = new TextBox {Text = source, IsReadOnly = true};
            DestinationBox = new TextBox {Text = destination, IsReadOnly = true};
        }

        public TextBox SourceBox { get; set; }

        public TextBox DestinationBox { get; set; }

        public string SourcePath 
        {
            get { return SourceBox.Text; }
            set { SourceBox.Text = value; }
        }

        public string DestinationPath
        {
            get { return DestinationBox.Text; }
            set { DestinationBox.Text = value; }
        }
    }
}
