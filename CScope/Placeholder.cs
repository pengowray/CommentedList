using CommentedList.CList;
using System.Collections.Generic;

namespace CommentedList.CScope {


    public struct Placeholder {
        // [barrel] and [barrel#tag] and [var=barrel#tag] and [var=barrel]

        public int? index; // pos in document
        public string barrel;
        public string tag;
        public string varName;
        public string varVal;
    }

    public struct PlaceholderArray {
        // [barrel*4] and [barrel#tag*4] and [var=barrel#tag*3] and [var=barrel*4] // unique by default

        public int? index;
        public string barrel;
        public string tag;
        public string varName;
        public string[] varVal;
        public int Count { get => varVal?.Length ?? 0; }
    }

}
