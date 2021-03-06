﻿using System;
using System.Collections.Generic;
using System.IO;

namespace dbexport.Common
{
    public static class FormatterExtensions
    {
        private static readonly Dictionary<int, string> indentCache = new Dictionary<int, string>();
        
        private static string Indentation(int count)
        {
            return new String('\t', count);
        }
        
        public static void WriteLine(this StreamWriter writer, int indent, params string[] values)
        {
            if (!indentCache.TryGetValue(indent, out string indentation))
            {
                indentation = Indentation(indent);
                indentCache.Add(indent, indentation);
            }
            
            writer.Write(indentation);
            foreach (var value in values)
            {
                writer.Write(value);
            }

            writer.WriteLine();
        }
    }
}