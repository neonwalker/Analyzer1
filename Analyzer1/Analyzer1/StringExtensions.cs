using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Analyzer1
{
    public static class StringExtensions
    {
        public static String TitleCaseString(this string s)
        {
            if (s == null) return s;
            s = (Regex.Replace(s, @"(\B[A-Z]+?(?=[A-Z][^A-Z])|\B[A-Z]+?(?=[^A-Z]))", " $1"));
            String[] words = s.Split(' ');
            for (int i = 0; i < words.Length; i++)
            {
                if (words[i].Length == 0) continue;

                Char firstChar = Char.ToUpper(words[i][0]);
                String rest = "";
                if (words[i].Length > 1)
                {
                    rest = words[i].Substring(1).ToLower();
                }
                words[i] = firstChar + rest;
            }
            return String.Join("", words);
        }

    public static bool IsUpperCase(this string cs)
    {
        if (string.IsNullOrEmpty(cs))
        {
            return false;
        }
        int sz = cs.Length;
        for (int i = 0; i < sz; i++)
        {
            if (Char.IsUpper(cs[i]) == false)
            {
                return false;
            }
        }
        return true;
    }
    }
}
