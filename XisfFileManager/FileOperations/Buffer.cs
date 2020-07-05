namespace XisfFileManager.XisfFile
{
    public class Buffer
    {
        public enum TypeEnum { ASCII, BINARY, ZEROS, POSITION }
        public TypeEnum Type;
        public string AsciiData { get; set; }
        public int BinaryDataStart;
        public int BinaryByteLength;
        public long ToPosition;
        public byte[] BinaryData { get; set; }
    }
}
