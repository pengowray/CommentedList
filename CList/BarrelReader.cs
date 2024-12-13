using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using CommentedList.WeightedRandomizer;

namespace CommentedList.CList {

    public static class BarrelReader {

        public static Barrel ReadFromStringContents(string txtData, Barrel addTo = null) {
            var stringReader = new StringReader(txtData);
            return Read(stringReader);
        }

        public static Barrel ReadFromStringContents(this Barrel bag, string txtData) {
            // extension method
            var stringReader = new StringReader(txtData);
            return Read(stringReader, bag);
        }

        public static void ReadFile(this Barrel bag, string filepath) {
            // extension method
            Read(filepath, bag);
        }


        public static Barrel Read(string filepath, Barrel addTo = null) {
            using (StreamReader file = new StreamReader(filepath)) {
                return Read(file, addTo);
            }
        }
        public static Barrel Read(TextReader reader, Barrel addTo = null) {

            addTo = addTo ?? new Barrel();
            //addTo = addTo ?? new DynamicWeightedRandomizer<string>();

            var itemReader = new TaggedItemReader(xTags: true);

            foreach (var item in itemReader.Read(reader)) {

                if (item == null)
                    continue;

                int weight = 1000; // [previously 90 for rare]
                if (item.Tags?.HasTag("x") ?? false) {
                    var numStr = item.GetTag("x");
                    if (Double.TryParse(numStr, out double multiplier)) {
                        weight = (int)(multiplier * weight);
                    }
                }

                addTo.Add(item, weight);
            }

            return addTo;
        }

    }
}