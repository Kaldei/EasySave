namespace EasySave.NS_Model
{
    public class Log
    {
        // --- Attributes ---
        public string name { get; set; }
        public string source { get; set; }
        public string destination { get; set; }
        public string size { get; set; }
        public string startTime { get; set; }
        public string elapsedTime { get; set; }
        public string cryptedTime { get; set; }


        // --- Constructors ---
        // Constructor used to Read json File
        public Log() { }

        // Constructor used to Add a Log
        public Log(string _name, string _source, string _destination, string _size, string _startTime, string _elapsedTime, string _cryptedTime)
        {
            this.name = _name;
            this.source = _source;
            this.destination = _destination;
            this.size = _size;
            this.startTime = _startTime;
            this.elapsedTime = _elapsedTime;
            this.cryptedTime = _cryptedTime;
        }
    }
}
