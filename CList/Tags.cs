using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommentedList.WeightedRandomizer;

namespace CommentedList.CList {

    public class Tags {
        public Dictionary<string, string> Items; // json-esque

        public const string DefaultVal = "true";
        public const string NoTagVal = "null"; // HasTag() returns false for this value

        //Tags:
        //"Cat:Noun"
        //etc
        //TODO: allow quotes, and maybe even json-style braces e.g. { "Cat" : "Noun" }

        public Tags() { }

        public IEnumerable<string> Keys {
            get {
                if (Items != null) {
                    return Items.Keys;
                }
                return new List<string>();
            }
        }

        public IEnumerable<KeyValuePair<string, string>> Pairs {
            get {
                if (Items != null) {
                    return Items;
                }
                return new List<KeyValuePair<string, string>>();
            }
        }

        public void Add(string tag) {
            if (tag == null) return;
            if (Items == null) Items = new Dictionary<string, string>();
            Items[tag] = DefaultVal;
        }
        public void Add(string tag, string val) {
            if (tag == null) return;
            if (val == null) {
                Add(tag);
                return;
            }
            if (Items == null) Items = new Dictionary<string, string>();
            Items[tag] = val;
        }
        public void Remove(string tag) {
            Items?.Remove(tag);
        }

        public string GetTag(string tag) {
            var success = Items.TryGetValue(tag, out var val);
            if (success) return val;
            return null;
        }

        [Obsolete("renamed to GetTag(tag)")]
        public string GetValue(string tag) { //TODO: rename GetTag()
            return GetTag(tag);
        }

        public bool HasTag(string tag) {
            return Items != null && Items.ContainsKey(tag) && Items[tag] != NoTagVal;
        }

        public bool HasTrueTag(string tag) {
            return Items != null
                && Items.TryGetValue(tag, out string val)
                && val == DefaultVal;
        }


        public override string ToString() {
            if (Items != null && Items.Any()) {
                return string.Join(", ", Items.Select(pair => PrettyTag(pair.Key, pair.Value)));
            }

            return "";
        }

        private string PrettyTag(string tag, string val) {
            if (val == null || val == "" || val == DefaultVal) {
                return $"#{tag}";
            } else {
                return $"#{tag}:{val}";
            }
        }
    }
}