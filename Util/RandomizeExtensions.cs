using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading;

namespace CommentedList.Util {

    public static class ThreadSafeRandom {
        [ThreadStatic] private static Random Local;

        public static Random ThisThreadsRandom {
            get { return Local ?? (Local = new Random(unchecked(Environment.TickCount * 31 + Thread.CurrentThread.ManagedThreadId))); }
        }
    }

    public static class RandomizeExtensions {
        public static void ShuffleInPlace<T>(this IList<T> list) {
            int n = list.Count;
            while (n > 1) {
                n--;
                int k = ThreadSafeRandom.ThisThreadsRandom.Next(n + 1);
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }

        public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> list) {
            var newList = list.ToList();
            newList.ShuffleInPlace();
            return newList;
        }

    }
}


