﻿using System;

namespace XisfFileManager.XisfKeywords
{
    public class Keyword
    {
        public enum EType  {NULL, COPY, STRING, INTEGER, FLOAT, BOOL }
        public EType Type { get; set; } = EType.NULL;
        public string Name { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
    }
}
