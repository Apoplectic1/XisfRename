using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XisfFileManager
{
    public class CalibrationTabPageValues
    {
        public string FileName { get; set; }
        public int Progress { get; set; }
        public int TotalFiles { get; set; }
    }

    public class CalibrationTabPageEvent
    {
        public static event DataReceivedEventHandler CalibrationTabPage_InvokeEvent;

        public static void TransmitData(CalibrationTabPageValues data)
        {
            CalibrationTabPage_InvokeEvent?.Invoke(data); // Raise the event.
        }
    }
}
