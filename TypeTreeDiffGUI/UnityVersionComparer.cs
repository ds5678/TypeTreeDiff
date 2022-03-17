using System.Collections.Generic;
using System.IO;
using AssetRipper.VersionUtilities;

namespace TypeTreeDiff.GUI.Comparer
{
    public class UnityVersionComparer : IComparer<string>
    {
        public int Compare(string left, string right)
        {
            string leftFileName = Path.GetFileNameWithoutExtension(left);
            string rightFileName = Path.GetFileNameWithoutExtension(right);

            UnityVersion leftVersion = UnityVersion.Parse(leftFileName);
            UnityVersion rightVersion = UnityVersion.Parse(rightFileName);

            return leftVersion.CompareTo(rightVersion);
        }
    }
}
