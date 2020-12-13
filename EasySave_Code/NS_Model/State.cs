using EasySave.Observable;

namespace EasySave.NS_Model
{
    public class State : ObservableObject
    {
        // --- Attributes ---
        public int totalFile { get; set; }
        public int totalPrioFile { get; set; }
        public int leftPrioFile { get; set; }
        public long totalSize { get; set; }
        public int Progress { get; set; }
        public int nbFileLeft { get; set; }
        public long leftSize { get; set; }
        public string currentPathSrc { get; set; }
        public string currentPathDest { get; set; }

        public int progress
        {
            get
            {
                return Progress;
            }
            set
            {
                 if(Progress != value)
                 {
                    Progress = value;
                    OnPropertyChanged("progress");
                 }
            }
           
        }

        // --- Contructors ---
        // Constructor used by LoadWorks()
        public State() { }

        // Constructor used by DoBackup()
        public State(int _totalFile, int _totalPrioFile, long _totalSize, string _currentPathSrc, string _currentPathDest)
        {
            this.progress = 0;
            this.totalFile = _totalFile;
            this.nbFileLeft = _totalFile;
            this.totalPrioFile = _totalPrioFile;
            this.leftPrioFile = _totalPrioFile;
            this.totalSize = _totalSize;
            this.leftSize = _totalSize;
            this.currentPathSrc = _currentPathSrc;
            this.currentPathDest = _currentPathDest;
        }


        // --- Methods ---
        // Update State during DoBacup()
        public void UpdateState(int _progress, int _leftPrioFile, int _nbFileLeft, long _leftSize, string _currSrcPath, string _currDestPath)
        {
            this.progress = _progress;
            this.leftPrioFile = _leftPrioFile;
            this.nbFileLeft = _nbFileLeft;
            this.leftSize = _leftSize;
            this.currentPathSrc = _currSrcPath;
            this.currentPathDest = _currDestPath;
        }
    }
}