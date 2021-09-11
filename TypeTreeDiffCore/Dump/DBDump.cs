using System;
using System.Collections.Generic;
using System.IO;
using TypeTreeDiff.Core.IO;

namespace TypeTreeDiff.Core.Dump
{
    public sealed class DBDump
    {
        private DBDump()
        {
        }

        public static DBDump Read(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new Exception($"File '{filePath}' doesn't exist");
            }

            byte[] data = File.ReadAllBytes(filePath);
            using (MemoryStream stream = new MemoryStream(data))
            {
                return Read(stream);
            }
        }

        public static DBDump Read(Stream stream)
        {
            DBDump dump = new DBDump();
            using (DumpReader reader = new DumpReader(stream))
            {
                System.Diagnostics.Stopwatch watch = new System.Diagnostics.Stopwatch();
                watch.Start();
                dump.ReadInner(reader);
                watch.Stop();

            }
            return dump;
        }

        public DBDump Optimize()
        {
            DBDump db = new DBDump();
            TreeDump[] typeTrees = new TreeDump[TypeTrees.Count];
            for (int i = 0; i < TypeTrees.Count; i++)
            {
                typeTrees[i] = (TreeDump)TypeTrees[i].Optimize();
            }
            db.TypeTrees = typeTrees;
            return db;
        }

        private void ReadInner(DumpReader reader)
        {
             List<TreeDump> trees = new List<TreeDump>();
            while (!ReadValidation(reader, trees))
            {
                TreeDump tree = TreeDump.Read(reader);
                trees.Add(tree);
            }
            TypeTrees = trees.ToArray();
        }

        private bool ReadValidation(DumpReader reader, IReadOnlyList<TreeDump> trees)
        {
            return !reader.FindContent();
        }

        public IReadOnlyList<TreeDump> TypeTrees { get; private set; }
    }
}
