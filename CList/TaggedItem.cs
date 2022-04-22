namespace CommentedList.CList; 

public class TaggedItem {
    public string Name { get; set; }
    public Tags? Tags;

    //public Tags? CategoryTags; //TODO? generic categories applied to a number of items?
    //bool CopyOnWrite = true; // TODO: duplicate tags if edited after setup (or have separate Fixed tags)

    public TaggedItem(string name) {
        //TODO: require non-null
        this.Name = name?.Trim();
    }

    public TaggedItem(string name, Tags tags) {
        this.Name = name?.Trim();
        this.Tags = tags;
    }
    public override string ToString() {
        return Name;
    }

    public string ToDebugString() {
        if (Tags != null) {
            return $"{Name} ({Tags})";
        }
        return $"{Name}";
    }

    public bool IsRare() {
        //TODO: check CategoryTags too?
        return (Tags != null && (Tags.HasTrueTag("rarify") || Tags.HasTrueTag("rare")));
    }

}
