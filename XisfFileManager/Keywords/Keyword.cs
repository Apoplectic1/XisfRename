using System;

namespace XisfFileManager.Keywords
{
    public class Keyword
    {
        public enum EType  {NULL, COPY, STRING, INTEGER, DOUBLE, BOOL }

        public EType Type { get; set; } = EType.NULL;
        public string Name { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        private string sValue { get; set; } = string.Empty;
        private int iValue { get; set; } = -1;
        private double dValue { get; set; } = double.NaN;
        private bool bValue { get; set; } = false;
        public string Comment { get; set; } = string.Empty;

        public bool SetKeyword(string sName, string sValue, string sComment = "XisfFile Manager")
        {
            Name = sName;
            this.sValue = sValue;
            Comment = sComment;
            Type = EType.STRING;
            return true;
        }

        public bool SetKeyword(string sName, int iValue, string sComment = "XisfFile Manager")
        {
            Name = sName;
            this.iValue = iValue;
            Comment = sComment;
            Type = EType.INTEGER;
            return true;
        }
        public bool SetKeyword(string sName, double dValue, string sComment = "XisfFile Manager")
        {
            Name = sName;
            this.dValue = dValue;
            Comment = sComment;
            Type = EType.DOUBLE;
            return true;
        }

        public bool SetKeyword(string sName, bool bValue, string sComment = "XisfFile Manager")
        {
            Name = sName;
            this.bValue = bValue;
            Comment = sComment;
            Type = EType.BOOL;
            return true;
        }

        public object GetKeyword()
        {
            switch (Type)
            {
                case EType.NULL:
                    return null;
                case EType.DOUBLE:
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
