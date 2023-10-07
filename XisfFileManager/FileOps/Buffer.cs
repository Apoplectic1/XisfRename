using XisfFileManager.Enums;

namespace XisfFileManager.FileOperations
{
    public class Buffer
    {
        public eBufferData Type;
        public string AsciiData { get; set; }
        public int BinaryDataStart;
        public int BinaryByteLength;
        public long ToPosition;
        public byte[] BinaryData { get; set; }
    }
}
