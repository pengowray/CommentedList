using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CommentedList.Util {
    
    // also in WordFinder / Util / StringSearch

    public static class StringUtil {
        public static IEnumerable<int> AllIndexesOf(this string str, string value) {
            if (String.IsNullOrEmpty(value))
                throw new ArgumentException("the string to find may not be empty or null", "value");

            for (int index = 0; ; index += value.Length) {
                index = str.IndexOf(value, index);
                if (index == -1)
                    break;
                yield return index;
            }
        }

        public static IEnumerable<int> ToCodePoints(this string s) {
            // was: ToUnicodeCodePoints()
            for (int i = 0; i < s.Length; i++) {
                int unicodeCodePoint = char.ConvertToUtf32(s, i);
                if (unicodeCodePoint > 0xffff) {
                    i++;
                }
                yield return unicodeCodePoint;
            }
        }

        public static string DistinctCodePointsAsString(this string letters) {
            // was: DistinctLetters32()
            return string.Join("", letters.ToCodePoints().OrderBy(p => p).Distinct().Select(p => char.ConvertFromUtf32(p)));
        }


    }
}
