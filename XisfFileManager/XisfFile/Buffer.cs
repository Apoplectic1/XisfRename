using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XisfFileManager.XisfFile
{
    public class Buffer
    {
        public enum TypeEnum { ASCII, BINARY, ZEROS }
        public TypeEnum Type;
        public string AsciiData { get; set; }
        public int BinaryDataStart;
        public int BinaryByteLength;
        public byte[] BinaryData { get; set; }
    }
}
