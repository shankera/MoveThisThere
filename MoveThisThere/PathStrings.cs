namespace MoveThisThere
{
    public class PathStrings
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
            SourcePath = source;
            DestinationPath = destination;
        }

        public string SourcePath { get; set; }

        public string DestinationPath { get; set; }
    }
}
