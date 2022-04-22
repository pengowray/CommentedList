using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace CommentedList.CList;
public class SimpleListReader {

    public static IEnumerable<string> Read(string filepath) {
        using (StreamReader file = new StreamReader(filepath)) {
            foreach (var item in Read(file)) {
                yield return item;
            }
            file.Close();
        }
    }

    public static IEnumerable<string> Read(TextReader reader) {

        string? line;
        while ((line = reader.ReadLine()) != null) {

            //remove comments (TODO: allow escaped comments)

            // "aaa // bbb"  =>  "aaa"

            var comment = line.IndexOf("//");
            if (comment >= 0) {
                line = line[..comment];
            }
            line = line.Trim();

            if (string.IsNullOrEmpty(line))
                continue;

            yield return line;
        }
    }
}
