using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using CommentedList.WeightedRandomizer;

namespace CommentedList.CList {

    public class TaggedItemReader {

        // e.g. reads list with lines like "elephant // #Mammal #Legs:4"

        public bool XTags { get; private set; } = false; // treat "// x10.1" as "// #x:10.1"  (case insensitive); for compatibility with lists that specify probabilities this way

        public static readonly TaggedItemReader Reader = new TaggedItemReader();
        public static readonly TaggedItemReader ReaderWithXTags = new TaggedItemReader(xTags: true);

        public TaggedItemReader() {
        }

        public TaggedItemReader(bool xTags) {
            XTags = xTags;
        }


        public IEnumerable<TaggedItem> ReadFromStringContents(string txtData) {
            var stringReader = new StringReader(txtData);
            return Read(txtData);
        }

        public IEnumerable<TaggedItem> Read(string filepath) {
            using (StreamReader file = new StreamReader(filepath)) {
                foreach (var item in Read(file)) {
                    yield return item;
                }
            }
        }

        public IEnumerable<TaggedItem> Read(TextReader reader) {

            // open and parse file
            // and removes "//" comments

            //based on QSOZ.QSO.RandomTextReader
            //originally based on Sprinto.Phrases.PhraseFile

            string line;
            while ((line = reader.ReadLine()) != null) {

                //remove comments (TODO: allow escaped comments)
                if (string.IsNullOrEmpty(line))
                    continue;
                line = line.TrimEnd('\x1A', '\uFFFD'); // remove sub characters

                var commentPos = line.IndexOf("//");
                var comment = "";
                if (commentPos >= 0) {
                    comment = line.Substring(commentPos).Trim();
                    line = line.Substring(0, commentPos).Trim();
                } else {
                    line = line.Trim();
                }

                if (string.IsNullOrEmpty(line))
                    continue;

                Tags tags = null;

                if (comment?.Contains("#") ?? false) {
                    // At least 1 letter, start with letter, may contain numbers or underscore
                    // Allows tag values, e.g. "#cat:noun" or "#country:France"
                    // todo: allow more characters if needed (e.g. dashes, periods), but only in middle for tag names
                    // todo: allow "double-quoted" tags and values
                    // e.g. "#fun" => {fun: 1} or "#country":"France" => {country: France} or #city:"New York"
                    var matches = Regex.Matches(comment, @"[#]([A-Za-z][A-Za-z0-9_]*)(\:[A-Za-z0-9\.\-_]+)?");
                    // for each match, only care about group 1 and 2, and not about captures
                    foreach (Match match in matches) {

                        var groups = match?.Groups;
                        if (groups == null) continue;

                        var groupCount = groups.Count;
                        if (groupCount <= 0) continue;

                        if (tags == null) tags = new Tags();

                        if (groupCount <= 1) continue;

                        if (groupCount == 2) {

                            var hashtag = groups[1].Value;
                            tags.Add(hashtag);

                        } else {
                            // count == 3
                            var hashtag = groups[1]?.Value;
                            var tagvalue = groups[2]?.Value;
                            if (tagvalue?.Length > 1) tagvalue = tagvalue.Substring(1); // remove leading ':'
                            tags.Add(hashtag, tagvalue);
                            //Console.WriteLine($"adding {hashtag}:{tagvalue}"); // debug
                        }
                    }
                }


                if (XTags && comment != null) {
                    //[^|\W] => make sure it's at start or after whitespace
                    //TODO: allow E notation (x5e10)
                    //allows negative/positive (though dubiously helpful)
                    //TODO: full floating point number regex: [+-]?(\d+([.]\d*)?([eE][+-]?\d+)?|[.]\d+([eE][+-]?\d+)?)
                    //TODO: compile regex
                    var matches = Regex.Match(comment, @"(?:^|\s|#)[xX]([-\+]?[0-9\.]+)(?:\W|$)"); // e.g. "x10" or "x3.30."
                    if (matches.Success) {
                        //TODO: more bounds checking
                        var numStr = matches?.Groups[1]?.Captures[0]?.Value;
                        numStr = numStr?.TrimEnd('.'); // remove any trailing dots, e.g. "x3.5."
                        if (Double.TryParse(numStr, out double multiplier)) {
                            //weight = (int)(multiplier * weight);
                            if (tags == null) {
                                tags = new Tags();
                            }
                            tags.Add("x", numStr); // numStr.ToString());
                        }
                    }
                }

                var thing = new TaggedItem(line, tags); // tags may be null but that's fine

                yield return thing;
            }

        }
    }
}