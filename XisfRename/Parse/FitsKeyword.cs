using System;
using System.Collections.Generic;

namespace XisfRename.Parse
{
    public class FitsKeyword
    {
        public enum KeywordType {NULL, COPY, INTEGER, FLOAT, STRING, BOOL}
        public KeywordType Type = KeywordType.NULL;

        public string Name { get; set; } = string.Empty;

        private int iValue;
        private string sValue;
        private double dValue;

        public string SetValue
        {
            set 
            {
                sValue = value.Replace("'", "").Trim();

                if (Type == KeywordType.INTEGER)
                {
                    iValue = Convert.ToInt32(sValue);
                }

                if (Type == KeywordType.FLOAT)
                {
                    dValue = Convert.ToDouble(sValue);
                }
            }
        }

        public dynamic GetValue<T>()
        {
            if (typeof(T) == typeof(int))
            {
                return iValue;
            }
            else if (typeof(T) == typeof(string))
            {
                return sValue;
            }
            else if (typeof(T) == typeof(double))
            {
                return dValue;
            }
            else
            {
                return false;
            }
        }

        public string Comment { get; set; } = string.Empty;
    }
}
