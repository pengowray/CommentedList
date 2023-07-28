using CommentedList.CList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Xml.Linq;
using static CommentedList.CScope.RecursiveBarrelSearch;

namespace CommentedList.CScope {
    public class CacheScope {

        public CacheScope Parent;
        public CacheScope Debug; // a child scope for debugging
        public BarrelCollection Barrels { get; set; }

        private Dictionary<string, string> Strings;
        private Dictionary<string, TaggedItem> Items;

        public const string SEPARATOR = "#";

        public CacheScope MakeChildScope() {
            return new CacheScope() {
                Parent = this
            };
        }

        public void AbsorbString(CacheScope other, string key) {
            key = BarrelCollection.NormalizeName(key);
            Strings[key] = other.Strings[key];
        }

        public void Absorb(CacheScope other) {
            if (other == null) return;

            if (Items == null) {
                this.Items = other.Items;
            } else {
                foreach (var item in other.Items) {
                    this.Items[item.Key] = item.Value;
                }
            }

            if (this.Strings == null) {
                this.Strings = other.Strings;
            } else {
                foreach (var item in other.Strings) {
                    this.Strings[item.Key] = item.Value;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="barrelPath">The name of a barrel, or "barrel# tag"</param>
        /// <param name="saveAs">null = don't cache;
        /// "[auto]" = cache as barrelPath
        /// "myVar" = cache barrel name part as myVar
        /// "myVar#tag" = cache as myVar#tag, and also cache the barrel as myVar
        /// if there's a tag, will also 
        /// </param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public string GetString(string barrelPath, bool hasBrackets, int maxRecurrsion = 6) {
            //TODO: recursion

            if (barrelPath == null) throw new ArgumentNullException();

            var placeholder = Splitter(barrelPath, hasBrackets);

            if (!string.IsNullOrEmpty(placeholder.varVal)) {
                var str = GetStringFromCache(placeholder.varVal);
                if (str != null) return str;
            }

            if (placeholder.barrel != null) {
                var item = GetItem(placeholder.barrel);
                if (placeholder.tag != null) { 
                    var tag = item.GetTag(placeholder.tag);
                    if (tag != null) {
                        if (placeholder.varVal != null) {
                            if (Strings == null) Strings = new Dictionary<string, string>();
                            Strings[placeholder.varVal] = tag;
                        }
                        return tag;
                    }
                } else {
                    if (placeholder.varVal != null) {
                        if (Strings == null) Strings = new Dictionary<string, string>();
                        Strings[placeholder.varVal] = item.Item;
                    }
                    return item.Item;
                }
            }

            return null;
        }

        public TaggedItem GetItem(string barrelName, string cacheName = null) {
            if (barrelName == null) throw new ArgumentNullException();
            var barrel = BarrelCollection.NormalizeName(barrelName);
            var cache = BarrelCollection.NormalizeName(cacheName);

            var fromCache = GetItemFromCache(cache);
            if (fromCache != null) return fromCache;

            var fromBarrels = GetTaggedItemFromBarrels(barrel);
            if (fromBarrels == null) {
                return null;
            }

            if (cache != null) {
                if (Strings == null) Strings = new Dictionary<string, string>();
                Strings[cache] = fromBarrels.Item;

                if (Items == null) Items = new Dictionary<string, TaggedItem>();
                Items[cache] = fromBarrels;
            }

            return fromBarrels;
        }

        public string GetStringFromCache(string path) {
            if (path == null) throw new ArgumentNullException();
            var name = BarrelCollection.NormalizeName(path);

            if (Strings?.TryGetValue(BarrelCollection.NormalizeName(name), out var value) ?? false) {
                return value;
            }
            if (Parent != null) {
                return Parent.GetStringFromCache(name);
            }
            return null;
        }

        public TaggedItem GetItemFromCache(string path) {
            if (path == null) throw new ArgumentNullException();
            var name = BarrelCollection.NormalizeName(path);

            if (Items?.TryGetValue(name, out var item) ?? false) {
                return item;
            }
            if (Parent != null) {
                return Parent.GetItemFromCache(name);
            }
            return null;
        }

        /// <summary>
        /// get item without caching result
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public TaggedItem GetTaggedItemFromBarrels(string barrel) {
            if (barrel == null) throw new ArgumentNullException();

            var name = BarrelCollection.NormalizeName(barrel);
            var item = Barrels?.PickOne(barrel);
            if (item != null) {
                return item;
            }
            if (Parent != null) {
                return Parent.GetTaggedItemFromBarrels(name);
            }
            return null;
        }

        public void SetString(string key, string val) {
            Strings.Add(key, val);
        }

        private static Placeholder Splitter(string text, bool hasBrackets) {
            // split text into [barrel] and [barrel#tag] and [var=barrel#tag] and [var=barrel]

            if (text == null) {
                throw new ArgumentNullException();
            }

            var result = new Placeholder();

            var name = BarrelCollection.NormalizeName(text);

            if (hasBrackets) {
                if (text.StartsWith("[")) {
                    name = name.Substring(1);
                }
                if (text.EndsWith("]")) {
                    name = name.Substring(0, name.Length - 1);
                }
            }

            var left = name;
            var right = name;
            var eq = name.IndexOf('=');
            if (eq > 0) {
                left = BarrelCollection.NormalizeName(name.Substring(0, eq)); // var
                right = BarrelCollection.NormalizeName(name.Substring(eq + 1));
                result.varName = left;
            }
            var hash = right.IndexOf('#');
            if (hash > 0) {
                result.barrel = BarrelCollection.NormalizeName(right.Substring(0, hash));
                result.tag = BarrelCollection.NormalizeName(right.Substring(hash + 1));
            } else {
                result.barrel = right;
            }

            return result;
        }
    }
}