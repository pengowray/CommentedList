using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Globalization;
using System.Linq;
using System.Text;

namespace CommentedList.Util {
    
    // was also in WordFinder / Util / StringSearch

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
                int codePoint = char.ConvertToUtf32(s, i);
                if (codePoint > 0xffff) {
                    i++;
                }
                yield return codePoint;
            }
        }

        public static IEnumerable<string> ToCodePointStrings(this string s) {
            foreach (var codePoint in s.ToCodePoints()) {
                yield return char.ConvertFromUtf32(codePoint);
            }
        }

        public static IEnumerable<string> ToLettersWithDiacritics(this string s) {
            // Keeps chars together with following diacritics, regardless how the string is normalized.
            // e.g. パーティ => パ;ー;テ;ィ  (and if パ is composed of two codepoints it will be kept as two)
            // will also keep emoji sequences (containing zwj's) together too
            // variant selectors (e.g. U+FE0F) also are kept with the emoji before it (no additional code needed to be added for this)

            string soFar = "";
            bool zwj = false;
            foreach (var codePointString in ToCodePointStrings(s)) {

                //if (soFar.Any(c => CharUnicodeInfo.GetUnicodeCategory(c) == UnicodeCategory.NonSpacingMark || CharUnicodeInfo.GetUnicodeCategory(c) == UnicodeCategory.SpacingCombiningMark)) {
                if (codePointString == "\x200D") {
                    zwj = true;
                    soFar += codePointString;

                } else if (zwj) {
                    soFar += codePointString;
                    zwj = false;

                } else if (CharUnicodeInfo.GetUnicodeCategory(codePointString, 0) == UnicodeCategory.NonSpacingMark || CharUnicodeInfo.GetUnicodeCategory(codePointString, 0) == UnicodeCategory.SpacingCombiningMark) {
                    soFar += codePointString;

                }  else {
                    if (!string.IsNullOrEmpty(soFar)) yield return soFar;
                    soFar = codePointString;
                }
            }

            if (!string.IsNullOrEmpty(soFar)) yield return soFar;
        }

        public static string DistinctCodePointsAsString(this string letters) {
            // was: DistinctLetters32()
            return string.Join("", letters.ToCodePoints().OrderBy(p => p).Distinct().Select(p => char.ConvertFromUtf32(p)));
        }

        public static string RemoveDiacritics(this string text) {
            if (string.IsNullOrWhiteSpace(text))
                return text;

            text = text.Normalize(NormalizationForm.FormD);
            var chars = text.Where(c => CharUnicodeInfo.GetUnicodeCategory(c) != UnicodeCategory.NonSpacingMark).ToArray();
            return new string(chars).Normalize(NormalizationForm.FormC);
        }


    }
}
