namespace EasySave.NS_Model
{
    public class State
    {
        // --- Attributes ---
        public int totalFile { get; set; }
        public long totalSize { get; set; }
        public int progress { get; set; }
        public int nbFileLeft { get; set; }
        public long leftSize { get; set; }
        public string currentPathSrc { get; set; }
        public string currentPathDest { get; set; }


        // --- Contructors ---
        // Constructor used by LoadWorks()
        public State() { }

        // Constructor used by DoBackup()
        public State(int _totalFile, long _totalSize, string _currentPathSrc, string _currentPathDest)
        {
            this.progress = 0;
            this.totalFile = _totalFile;
            this.totalSize = _totalSize;
            this.currentPathSrc = _currentPathSrc;
            this.currentPathDest = _currentPathDest;
        }


        // --- Methods ---
        // Update State during DoBacup()
        public void UpdateState(int _progress, int _nbFileLeft, long _leftSize, string _currSrcPath, string _currDestPath)
        {
            this.progress = _progress;
            this.nbFileLeft = _nbFileLeft;
            this.leftSize = _leftSize;
            this.currentPathSrc = _currSrcPath;
            this.currentPathDest = _currDestPath;
        }
    }
}