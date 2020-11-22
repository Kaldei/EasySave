using System;
using System.Collections.Generic;
using System.Text;

namespace EasySave.NS_ViewModel
{
    class CurrentWork
    {
        public int totalFile { get; set; }
        public long totalSize { get; set; }
        public int progress { get; set; }
        public int nbFileLeft { get; set; }
        public long leftSize { get; set; }
        public string currentPathSrc { get; set; }
        public string currentPathDest { get; set; }
    }
}
