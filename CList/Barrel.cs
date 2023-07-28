using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CommentedList.WeightedRandomizer;

namespace CommentedList.CList {
    public class Barrel : IEnumerable<TaggedItem> { // : ICollection<TaggedItem> // TODO: implement ICollection
        static Random RNG = new Random();

        //TODO: use nested barrels via ABScope items.txt => "[items-common] // x35 \n [items-rare]"
        public IWeightedRandomizer<TaggedItem> Items;
        [Obsolete("use other method [todo: make a RareBarrel class or something for easy switchover]")]
        public IWeightedRandomizer<TaggedItem> RareItems;

        double RareChance = 1.0f / 35f; // 1 in 35

        //public Tags HeritableTags; // copied to things in PickOne()

        public Barrel() { }

        public TaggedItem PickOne() {
            TaggedItem thing;

            //todo: check RareChance doesn't make rare items more than half as common as regular

            var rare = RNG.NextDouble() < RareChance;
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

        public IEnumerator<TaggedItem> GetEnumerator() {
            if (Items != null && RareItems != null) {
                return (IEnumerator<TaggedItem>)Items.Concat(RareItems);
            } else if (Items != null) {
                return Items.GetEnumerator();
            } else if (RareItems != null){
                return RareItems.GetEnumerator();
            } else {
                return (IEnumerator<TaggedItem>)Enumerable.Empty<TaggedItem>();
            }
        }

        IEnumerator IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }
    }
}