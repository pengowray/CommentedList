using System;

namespace CommentedList.CList {

    public class TaggedItem {
        
        public string Item { get; private set; } // was called Name
        public Tags Tags { get; private set; } 

        //public Tags? CategoryTags; //TODO? generic categories applied to a number of items?
        //bool CopyOnWrite = true; // TODO: duplicate tags if edited after setup (or have separate Fixed tags)

        public TaggedItem(string item) {
            //TODO: require non-null
            this.Item = item?.Trim();
        }

        public TaggedItem(string item, Tags tags) {
            this.Item = item?.Trim();
            this.Tags = tags;
        }
        public override string ToString() {
            return Item;
        }

        public string ToLine() {
            if (Tags != null) {
                return $"{Item} // {Tags}";
            }
            return $"{Item}";
        }

        public string GetTag(string tag) {
            if (Tags != null) {
                return null;
            }
            return Tags.GetTag(tag);
        }

        [Obsolete]
        public bool IsRare() {
            //TODO: check CategoryTags too?
            return (Tags != null && (Tags.HasTrueTag("rarify") || Tags.HasTrueTag("rare")));
        }

    }
}