using System;

namespace XisfFileManager.Keywords
{
    public class Keyword
    {
        public enum eType  {NULL, COPY, STRING, INTEGER, DOUBLE, BOOL }

        public eType Type { get; set; } = eType.NULL;
        public string Name { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        private string sValue { get; set; } = string.Empty;
        private int iValue { get; set; } = int.MinValue;
        private double dValue { get; set; } = double.NaN;
        private bool bValue { get; set; } = false;
        public string Comment { get; set; } = string.Empty;

        public void SetKeyword(string sName, string sValue, string sComment = "XisfFile Manager")
        {
            Name = sName;
            this.sValue = sValue;
            this.Value = sValue;
            Comment = sComment;
            Type = eType.STRING;
        }

        public void SetKeyword(string sName, int iValue, string sComment = "XisfFile Manager")
        {
            Name = sName;
            this.iValue = iValue;
            this.dValue = iValue;
            this.Value = iValue.ToString();
            Comment = sComment;
            Type = eType.INTEGER;
        }
        public void SetKeyword(string sName, double dValue, string sComment = "XisfFile Manager")
        {
            Name = sName;
            this.dValue = dValue;
            this.iValue = (int)Math.Round(dValue);
            this.Value = dValue.ToString();
            Comment = sComment;
            Type = eType.DOUBLE;
        }

        public void SetKeyword(string sName, bool bValue, string sComment = "XisfFile Manager")
        {
            Name = sName;
            this.bValue = bValue;
            this.Value = bValue.ToString();
            Comment = sComment;
            Type = eType.BOOL;
        }
        
        public object GetKeyword()
        {
            switch (Type)
            {
                case eType.NULL:
                    return null;
                case eType.DOUBLE:
                    return dValue;
                case eType.INTEGER:
                    return iValue;
                case eType.BOOL:
                    return bValue;
                default:
                    return Value.Replace("'","");
            }
        }

        public object GetKeyword(eType type)
        {
            switch (type)
            {
                case eType.NULL:
                    return null;
                case eType.DOUBLE:
                    return dValue;
                case eType.INTEGER:
                    return iValue;
                case eType.BOOL:
                    return bValue;
                default:
                    return Value.Replace("'", "");
            }
        }
    }
}
