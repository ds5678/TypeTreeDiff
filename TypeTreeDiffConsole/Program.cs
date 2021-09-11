using System;
using TypeTreeDiff.Core;
using TypeTreeDiff.Core.Dump;

namespace TypeTreeDiff.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                if(args.Length != 1)
                {
                    Logger.Info("This program takes exactly one argument");
                }
                else
                {
                    var dump = DBDump.Read(args[0]);
                    Logger.Info($"It had {dump.TypeTrees.Count} classes");
                    int count = 0;
                    foreach(var type in dump.TypeTrees)
                    {
                        Logger.Info($"{type.ClassID} : {type.ClassName}");
                        count++;
                    }
                    Logger.Info(count);
                }
            }
            catch(Exception ex)
            {
                Logger.Info(ex.ToString());
            }
            Logger.Info("Done!");
            System.Console.ReadLine();
        }
    }
}
