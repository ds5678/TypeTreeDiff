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
                    Logger.Info($"Read dump file of type{dump.Type} and version {dump.Version}");
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
