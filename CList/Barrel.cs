using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommentedList.WeightedRandomizer;

namespace CommentedList.CList;
public class Barrel {
    static Random RNG = new();

    //TODO: combine to allow for simpler roundtrips
    //TODO: allow more than 2 collections of items
    public IWeightedRandomizer<TaggedItem>? Items;
    public IWeightedRandomizer<TaggedItem>? RareItems;

    float RareChance = 1.0f/35f; // 1 in 35

    //public Tags HeritableTags; // copied to things in PickOne()

    public Barrel() { }

    public TaggedItem? PickOne() {
        TaggedItem thing;

        //todo: check RareChance doesn't make rare items more than half as common as regular

        var rare = RNG.NextSingle() < RareChance;
        if (rare && RareItems != null && RareItems.Any()) {
            thing = RareItems.NextWithReplacement();
        } else if (Items != null && Items.Any()) {
            thing = Items.NextWithReplacement();
        } else {
            return null;
        }
        if (thing == null) return null;

        //thing.CategoryTags = this.HeritableTags;

        return thing;
    }

    internal void Add(TaggedItem item, int weight = 1000) {
        if (item == null) return;

        if (item.IsRare()) {
            if (RareItems == null) RareItems = new StaticWeightedRandomizer<TaggedItem>();
            RareItems.Add(item, weight);
        } else {
            if (Items == null) Items = new StaticWeightedRandomizer<TaggedItem>();
            Items.Add(item, weight);
        }
    }
}
