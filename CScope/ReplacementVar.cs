using CommentedList.CList;

namespace CommentedList.CScope {


    public struct Placeholder {
        // [barrel] and [barrel#tag] and [var=barrel#tag] and [var=barrel]

        public int? index;
        public string barrel;
        public string tag;
        public string varName;
        public string varVal;
    }
}

// [animals] // picks one from barrel "animals" and stores in "animals"
// [animals#legs] // picks one from barrel "animals" and returns its #legs tag
// [fav=animals] // returns fav, or otherwise picks one from barrel "animals" and stores in variable "fav"

// fantasy syntax (brainstorming)
// "/*[fav=animals]*/ I'm think of an animal with [fav#legs] legs, it's a [fav]. // picks one from barrel "animals" and stores in variable "fav", then returns its #legs tag
// "[favAnimal='cat']" // set favAnimal to "cat" (ignoring barrels)
// {{Animals}} // pick one from barrel "Animals" and stores in variable "Animals"
// {[auto]=[Animals]}
// {_=[Animals]}

// TODO: work on this formatting
// "{favs=[trees.pick4]#SciName}" //pick 4 (without replacement) from barrel "trees" and return their #SciName tag

// example:
// "I like {favAnimal=[animals]} and {favTree=[trees]}"

// return:
// scope with { favAnimal = "cat", favTree = "oak", "0" = "cat", "1" = "oak" }
// which you can then use to replace the variables in the text, and save back into the scope.

