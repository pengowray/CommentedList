using CommentedList.CList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace CommentedList.CScope {
    public class BarrelCollection {
        // TODO:
        // D hold a collection of barrels
        // D load them from a directory of .txt files
        // D defer to ParentScope if barrel not found (in CacheScope)
        // D replace "[music]" with a random selection from the "music" barrel
        // - return an array of replacement strings
        // - replace "[music]" using an array of strings fed back
        // - child barrels?
        // - [special] -> introspect method for custom barrels
        // - set max recursion depth

        Dictionary<string, Barrel> Barrels;

        public void LoadDir(string dir) {
            if (dir == null) {
                throw new ArgumentNullException();
            }

            if (!Directory.Exists(dir)) {
                throw new DirectoryNotFoundException();
            }

            var files = Directory.GetFiles(dir, "*.txt");
            foreach (var file in files) {
                var barrel = BarrelReader.Read(file);
                var name = Path.GetFileNameWithoutExtension(file);
                AddBarrel(name, barrel);
            }
        }

        public void AddBarrel(string name, Barrel barrel) {
            if (name == null) {
                throw new ArgumentNullException();
            }
            if (barrel == null) {
                throw new ArgumentNullException();
            }
            if (Barrels == null) {
                Barrels = new Dictionary<string, Barrel>();
            }
            Barrels[NormalizeName(name)] = barrel;
        }

        public TaggedItem PickOne(string barrelName) {
            if (barrelName == null) {
                throw new ArgumentNullException();
            }
            var barrel = GetBarrel(barrelName);
            if (barrel == null) {
                return null;
            }
            return barrel.PickOne();
        }

        private Barrel GetBarrel(string barrelName) {
            if (barrelName != null) {
                throw new ArgumentNullException();
            }
            var name = NormalizeName(barrelName);
            Barrels.TryGetValue(name, out var barrel);
            return barrel;
        }

        public static string NormalizeName(string name) {
            if (name == null) {
                return null;
            }

            return name.Trim().Normalize(NormalizationForm.FormC);
        }


    }
}
