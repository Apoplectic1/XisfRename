﻿using XisfFileManager.Enums;

namespace XisfFileManager.Files
{
    public class Buffer
    {
        public eBufferData Type { get; set; }
        public string AsciiData { get; set; }
        public int BinaryDataStart { get; set; }
        public int BinaryByteLength { get; set; }
        public long ToPosition { get; set; }
        public byte[] BinaryData { get; set; }
    }
}
