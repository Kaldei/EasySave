using System;
using System.IO;
using System.Text.Json;
using System.Collections.Generic;
using System.Text;

namespace EasySave.NS_ViewModel
{
    class State
    {
        public string timestamp { get; set; }
        public string name { get; set; }
        public CurrentWork currentWork { get; set; }

        public void UpdateState(State _state, int _progress, int _nbFileLeft, long _leftSize, string _currSrcPath, string _currDestPath)
        {
            if (_progress != 100)
            {
                _state.currentWork.progress = _progress;
                _state.currentWork.nbFileLeft = _nbFileLeft;
                _state.currentWork.leftSize = _leftSize;
                _state.currentWork.currentPathSrc = _currSrcPath;
                _state.currentWork.currentPathDest = _currDestPath;
            }
            else
            {
                _state.currentWork = null;
            }

            try
            {
                var options = new JsonSerializerOptions()
                {
                    WriteIndented = true
                };
                File.WriteAllText("./State.json", JsonSerializer.Serialize(_state, options));
            }
            catch (System.Exception)
            {
                Console.WriteLine("Error");
                throw;
            }
        }
    }
}
