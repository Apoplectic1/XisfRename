using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XisfRename.Parse
{
    public class Buffer
    {
        public enum TypeEnum { ASCII, BINARY, ZEROS }
        public TypeEnum Type;
        public string ASCII { get; set; }
        public int BinaryStart;
        public int BinaryLength;
        public byte[] Binary { get; set; }
    }
}
