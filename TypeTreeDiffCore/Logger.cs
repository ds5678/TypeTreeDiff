using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TypeTreeDiff.Core
{
    public static class Logger
    {
        public static void Info(object obj) => Info(obj.ToString());
        public static void Info(string message) => Console.WriteLine(message);
    }
}
