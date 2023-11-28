using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XisfFileManager.Files;

namespace XisfFileManager
{
    public partial class MainForm
    {
        private GPU mXisfToGpu;

        private void XisfToGpuTab_Button_ConvertToGpu_Click(object sender, EventArgs e)
        {
            mXisfToGpu = new GPU();

            mXisfToGpu.XisfToGpuData(mFileList.First(), @"E:\Temp\input_data.dat");
        }

        private void XisfToGpuTab_Button_ConvertToXisf_Click(object sender, EventArgs e)
        {
            mXisfToGpu = new GPU();
            mXisfToGpu.GpuDataToXisf(@"E:\Temp\output_data.dat", @"E:\Temp\output_data.xisf");
        }
    }
}
