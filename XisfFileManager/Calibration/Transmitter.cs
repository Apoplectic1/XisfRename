using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XisfFileManager
{
    public class CalibrationPageValues
    {
        public string FileName { get; set; }
        public int Progress { get; set; }
        public int TotalFiles { get; set; }
    }

    public class Transmitter
    {
        public static event DataReceivedEventHandler DataReceived;

        public static void TransmitData(CalibrationPageValues data)
        {
            DataReceived?.Invoke(data); // Raise the event.
        }
    }
}
