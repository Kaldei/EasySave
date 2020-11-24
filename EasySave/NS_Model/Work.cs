namespace EasySave.NS_Model
{
    class Work
    {
        // --- Attributes ---
        public int id { get; set; }
        public string name { get; set; }
        public string src { get; set; }
        public string dst { get; set; }
        public BackupType backupType { get; set; }
        public State state { get; set; }
        public string lastBackupDate { get; set; }


        // --- Constructors ---
        // Constructor used by LoadWorks()
        public Work() {}

        // Constructor used by AddWork()
        public Work (int _id, string _name, string _src, string _dst, BackupType _backupType)
        {
            this.id = _id;
            this.name = _name;
            this.src = _src;
            this.dst = _dst;
            this.backupType = _backupType;
            this.state = null;
        }
    }
}
