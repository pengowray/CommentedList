using System;
using System.Collections.Generic;
using System.Text;

using CommentedList.CList;

namespace CommentedList.CScope {
    static public class DoubleBarrel {


        //TODO


        static public BarrelCollection MakeDoubleBarrel(string name, Barrel common, Barrel rare, int commonMultipler = 35) {
            var doubleB = new BarrelCollection();
            doubleB.AddBarrel(name + "-common", common);
            doubleB.AddBarrel(name + "-rare", rare);

            var main = new Barrel();
            main.Add(new TaggedItem($"[{name}-common]"), commonMultipler);
            main.Add(new TaggedItem($"[{name}-rare]"), 1);
            doubleB.AddBarrel(name, main);

            return doubleB;
        }


        static public BarrelCollection MakeDoubleBarrel(string name, Barrel barrel, int commonMultipler = 35) {
            var doubleB = new BarrelCollection();
            var common = new Barrel();
            var rare = new Barrel();
            foreach (var item in barrel) {
                if (item.Tags != null && item.Tags.HasTag("rare") || item.Tags.HasTag("rarify")) {
                    rare.Add(item);
                } else {
                    common.Add(item);
                }
            }

            doubleB.AddBarrel(name + "-common", common);
            doubleB.AddBarrel(name + "-rare", rare);

            var main = new Barrel();
            main.Add(new TaggedItem($"[{name}-common]"), commonMultipler);
            main.Add(new TaggedItem($"[{name}-rare]"), 1);
            doubleB.AddBarrel(name, main);

            return doubleB;
        }

    }
}
