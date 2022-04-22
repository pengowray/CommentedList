using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommentedList.WeightedRandomizer;

namespace CommentedList.CList;

public class Tags {
    public Dictionary<string, string> Items; // json-esque

    public const string DefaultVal = "true";

    //Tags:
    //"Cat:Noun"
    //etc
    //TODO: allow quotes, and maybe even braces e.g. { "Cat" : "Noun" }

    public Tags() { }

    public void Add(string tag) {
        if (tag == null) return;
        if (Items == null) Items = new();
        Items[tag] = DefaultVal;
    }
    public void Add(string tag, string val) {
        if (tag == null) return;
        if (val == null) {
            Add(tag);
            return;
        }
        if (Items == null) Items = new();
        Items[tag] = val;
    }
    public void Remove(string tag) {
        Items?.Remove(tag);
    }

    public string? GetValue(string tag) {
        return Items?.GetValueOrDefault(tag);
    }

    public bool HasTag(string tag) {
        //TODO: don't count if value is 0 or false?
        return Items != null && Items.ContainsKey(tag);
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