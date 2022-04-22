using System;
using System.Collections.Generic;
using System.Text;

namespace CommentedList.Util {
    public static class PickOneExtensions {
        private static Random Rand = new Random();

        // via: http://csharphelper.com/blog/2018/04/make-extension-methods-that-pick-random-items-from-arrays-or-lists-in-c/

        // Return a random item from an array.
        public static T PickOne<T>(this T[] items) {
            // Return a random item.
            return items[Rand.Next(0, items.Length)];
        }

        // Return a random item from a list.
        public static T PickOne<T>(this List<T> items) {
            // Return a random item.
            return items[Rand.Next(0, items.Count)];
        }


    }

}
