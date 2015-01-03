using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace MoveThisThere
{
    class PathStrings
    {
        public PathStrings(string source, string destination)
        {
            SourceBox = new TextBox { Text = source , IsReadOnly = true };
            DestinationBox = new TextBox { Text = destination, IsReadOnly = true };
        }
        public TextBox SourceBox { get; set; }
        public string SourcePath {
            get { return SourceBox.Text; }
            set { SourceBox.Text = value; }
        }
        public TextBox DestinationBox { get; set; }
        public string DestinationPath
        {
            get { return DestinationBox.Text; }
            set { DestinationBox.Text = value; }
        }
    }
}
