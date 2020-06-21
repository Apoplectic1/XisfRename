using System;

namespace XisfFileManager.Keywords
{
    public class Keyword
    {
        public enum EType  {NULL, COPY, STRING, INTEGER, FLOAT, BOOL }

        public EType Type { get; set; } = EType.NULL;
        public string Name { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;

        public object GetValue()
        {
            switch (Type)
            {
                case EType.FLOAT:
                    return Convert.ToDouble(Value);
                case EType.INTEGER:
                    return Convert.ToInt32(Value);
                case EType.BOOL:
                    return Convert.ToBoolean(Value);
                default:
                    return Value.Replace("\"","").Replace("'","");
            }
        }
    }
}
