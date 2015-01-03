using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MoveThisThere
{
    class PathStrings
    {
        public PathStrings(string source, string destination)
        {
            SourcePath = source;
            DestinationPath = destination;
        }
        public string SourcePath { get; set; }
        public string DestinationPath { get; set; }
    }
}
