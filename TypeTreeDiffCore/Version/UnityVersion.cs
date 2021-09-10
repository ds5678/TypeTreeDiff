using System;
using System.IO;

namespace TypeTreeDiff.Core.Version
{
    public struct UnityVersion
    {
        public UnityVersion(int major)
        {
            m_data = (ulong)major << 48 & 0xFFFFUL;
        }

        public UnityVersion(int major, int minor)
        {
            m_data = (ulong)(major & 0xFFFF) << 48 | (ulong)(minor & 0xFF) << 40;
        }

        public UnityVersion(int major, int minor, int build)
        {
            m_data = (ulong)(major & 0xFFFF) << 48 | (ulong)(minor & 0xFF) << 40 | (ulong)(build & 0xFF) << 32;
        }

        public UnityVersion(int major, int minor, int build, UnityVersionType type)
        {
            m_data = (ulong)(major & 0xFFFF) << 48 | (ulong)(minor & 0xFF) << 40 | (ulong)(build & 0xFF) << 32
                | (ulong)((int)type & 0xFF) << 24;
        }

        public UnityVersion(int major, int minor, int build, UnityVersionType type, int typeNumber)
        {
            m_data = (ulong)(major & 0xFFFF) << 48 | (ulong)(minor & 0xFF) << 40 | (ulong)(build & 0xFF) << 32
                | (ulong)((int)type & 0xFF) << 24 | (ulong)(typeNumber & 0xFF) << 16;
        }

        private UnityVersion(ulong data)
        {
            m_data = data;
        }

        public static bool operator ==(UnityVersion left, UnityVersion right)
        {
            return left.m_data == right.m_data;
        }

        public static bool operator !=(UnityVersion left, UnityVersion right)
        {
            return left.m_data != right.m_data;
        }

        public static bool operator >(UnityVersion left, UnityVersion right)
        {
            return left.m_data > right.m_data;
        }

        public static bool operator >=(UnityVersion left, UnityVersion right)
        {
            return left.m_data >= right.m_data;
        }

        public static bool operator <(UnityVersion left, UnityVersion right)
        {
            return left.m_data < right.m_data;
        }

        public static bool operator <=(UnityVersion left, UnityVersion right)
        {
            return left.m_data <= right.m_data;
        }

        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }
            if (obj.GetType() != typeof(UnityVersion))
            {
                return false;
            }
            return this == (UnityVersion)obj;
        }

        public override int GetHashCode()
        {
            return unchecked(827 + 911 * m_data.GetHashCode());
        }

        public override string ToString()
        {
            string result = $"{Major}.{Minor}.{Build}";
            if (Type != UnityVersionType.Base)
            {
                result = $"{result}{Type.ToLiteral()}{TypeNumber}";
            }
            return result;
        }

        public bool IsEqual(int major)
        {
            return this == From(major);
        }

        public bool IsEqual(int major, int minor)
        {
            return this == From(major, minor);
        }

        public bool IsEqual(int major, int minor, int build)
        {
            return this == From(major, minor, build);
        }

        public bool IsEqual(int major, int minor, int build, UnityVersionType type)
        {
            return this == From(major, minor, build, type);
        }

        public bool IsEqual(int major, int minor, int build, UnityVersionType type, int typeNumber)
        {
            return this == new UnityVersion(major, minor, build, type, typeNumber);
        }

        public bool IsEqual(string version)
        {
            UnityVersion compareVersion = new UnityVersion();
            compareVersion.Parse(version);
            return this == compareVersion;
        }

        public bool IsLess(int major)
        {
            return this < From(major);
        }

        public bool IsLess(int major, int minor)
        {
            return this < From(major, minor);
        }

        public bool IsLess(int major, int minor, int build)
        {
            return this < From(major, minor, build);
        }

        public bool IsLess(int major, int minor, int build, UnityVersionType type)
        {
            return this < From(major, minor, build, type);
        }

        public bool IsLess(int major, int minor, int build, UnityVersionType type, int typeNumber)
        {
            return this < new UnityVersion(major, minor, build, type, typeNumber);
        }

        public bool IsLess(string version)
        {
            UnityVersion compareVersion = new UnityVersion();
            compareVersion.Parse(version);
            return this < compareVersion;
        }

        public bool IsLessEqual(int major)
        {
            return this <= From(major);
        }

        public bool IsLessEqual(int major, int minor)
        {
            return this <= From(major, minor);
        }

        public bool IsLessEqual(int major, int minor, int build)
        {
            return this <= From(major, minor, build);
        }

        public bool IsLessEqual(int major, int minor, int build, UnityVersionType type)
        {
            return this <= From(major, minor, build, type);
        }

        public bool IsLessEqual(int major, int minor, int build, UnityVersionType type, int typeNumber)
        {
            return this <= new UnityVersion(major, minor, build, type, typeNumber);
        }

        public bool IsLessEqual(string version)
        {
            UnityVersion compareVersion = new UnityVersion();
            compareVersion.Parse(version);
            return this <= compareVersion;
        }

        public bool IsGreater(int major)
        {
            return this > From(major);
        }

        public bool IsGreater(int major, int minor)
        {
            return this > From(major, minor);
        }

        public bool IsGreater(int major, int minor, int build)
        {
            return this > From(major, minor, build);
        }

        public bool IsGreater(int major, int minor, int build, UnityVersionType type)
        {
            return this > From(major, minor, build, type);
        }

        public bool IsGreater(int major, int minor, int build, UnityVersionType type, int typeNumber)
        {
            return this > new UnityVersion(major, minor, build, type, typeNumber);
        }

        public bool IsGreater(string version)
        {
            UnityVersion compareVersion = new UnityVersion();
            compareVersion.Parse(version);
            return this > compareVersion;
        }

        public bool IsGreaterEqual(int major)
        {
            return this >= From(major);
        }

        public bool IsGreaterEqual(int major, int minor)
        {
            return this >= From(major, minor);
        }

        public bool IsGreaterEqual(int major, int minor, int build)
        {
            return this >= From(major, minor, build);
        }

        public bool IsGreaterEqual(int major, int minor, int build, UnityVersionType type)
        {
            return this >= From(major, minor, build, type);
        }

        public bool IsGreaterEqual(int major, int minor, int build, UnityVersionType type, int typeNumber)
        {
            return this >= new UnityVersion(major, minor, build, type, typeNumber);
        }

        public bool IsGreaterEqual(string version)
        {
            UnityVersion compareVersion = new UnityVersion();
            compareVersion.Parse(version);
            return this >= compareVersion;
        }

        public void Parse(string version)
        {
            if (string.IsNullOrEmpty(version))
            {
                throw new Exception($"Invalid version number {version}");
            }

            using (StringReader reader = new StringReader(version))
            {
                string major = string.Empty;
                while (true)
                {
                    char c = (char)reader.Read();
                    if (c == '.')
                    {
                        Major = int.Parse(major);
                        break;
                    }
                    major += c;
                }

                string minor = string.Empty;
                while (true)
                {
                    int symb = reader.Read();
                    if (symb == -1)
                    {
                        Minor = int.Parse(minor);
                        return;
                    }

                    char c = (char)symb;
                    if (c == '.')
                    {
                        Minor = int.Parse(minor);
                        break;
                    }
                    minor += c;
                }

                string build = string.Empty;
                Type = UnityVersionType.Base;
                TypeNumber = 1;
                while (true)
                {
                    int symb = reader.Read();
                    if (symb == -1)
                    {
                        Build = int.Parse(build);
                        return;
                    }

                    char c = (char)symb;
                    if (!char.IsDigit(c))
                    {
                        Build = int.Parse(build);
                        switch (c)
                        {
                            case 'a':
                                Type = UnityVersionType.Alpha;
                                break;

                            case 'b':
                                Type = UnityVersionType.Beta;
                                break;

                            case 'p':
                                Type = UnityVersionType.Patch;
                                break;

                            case 'f':
                                Type = UnityVersionType.Final;
                                break;

                            default:
                                throw new Exception($"Unsupported version type {c} for version '{version}'");
                        }
                        break;
                    }
                    build += c;
                }

                string typeNumber = string.Empty;
                while (true)
                {
                    int symb = reader.Read();
                    if (symb == -1)
                    {
                        TypeNumber = int.Parse(typeNumber);
                        return;
                    }

                    char c = (char)symb;
                    if (!char.IsDigit(c))
                    {
                        TypeNumber = int.Parse(typeNumber);
                        break;
                    }
                    typeNumber += c;
                }
            }
        }

        private UnityVersion From(int major)
        {
            ulong data = (ulong)(major & 0xFFFF) << 48 | 0x0000FFFFFFFFFFFFUL & m_data;
            return new UnityVersion(data);
        }
        private UnityVersion From(int major, int minor)
        {
            ulong data = (ulong)(major & 0xFFFF) << 48 | (ulong)(minor & 0xFF) << 40 | 0x000000FFFFFFFFFFUL & m_data;
            return new UnityVersion(data);
        }
        private UnityVersion From(int major, int minor, int build)
        {
            ulong data = (ulong)(major & 0xFFFF) << 48 | (ulong)(minor & 0xFF) << 40 | (ulong)(build & 0xFF) << 32 |
                0x00000000FFFFFFFFUL & m_data;
            return new UnityVersion(data);
        }
        private UnityVersion From(int major, int minor, int build, UnityVersionType type)
        {
            ulong data = (ulong)(major & 0xFFFF) << 48 | (ulong)(minor & 0xFF) << 40 | (ulong)(build & 0xFF) << 32 |
                (ulong)((int)type & 0xFF) << 24 | 0x0000000000FFFFFFUL & m_data;
            return new UnityVersion(data);
        }
        private UnityVersion From(int major, int minor, int build, UnityVersionType type, int typeNumber)
        {
            ulong data = (ulong)(major & 0xFFFF) << 48 | (ulong)(minor & 0xFF) << 40 | (ulong)(build & 0xFF) << 32
                | (ulong)((int)type & 0xFF) << 24 | (ulong)(typeNumber & 0xFF) << 16 | 0x000000000000FFFFUL & m_data;
            return new UnityVersion(data);
        }

        public static UnityVersion MinVersion => new UnityVersion(0UL);
        public static UnityVersion MaxVersion => new UnityVersion(ulong.MaxValue);

        public int Major
        {
            get => unchecked((int)(m_data >> 48 & 0xFFFFUL));
            private set => m_data = (ulong)(value & 0xFFFF) << 48 | ~(0xFFFFUL << 48) & m_data;
        }
        public int Minor
        {
            get => unchecked((int)(m_data >> 40 & 0xFFUL));
            private set => m_data = (ulong)(value & 0xFF) << 40 | ~(0xFFUL << 40) & m_data;
        }
        public int Build
        {
            get => unchecked((int)(m_data >> 32 & 0xFFUL));
            private set => m_data = (ulong)(value & 0xFF) << 32 | ~(0xFFUL << 32) & m_data;
        }
        public UnityVersionType Type
        {
            get => (UnityVersionType)unchecked((int)(m_data >> 24 & 0xFFUL));
            private set => m_data = (ulong)((int)value & 0xFF) << 24 | ~(0xFFUL << 24) & m_data;
        }
        public int TypeNumber
        {
            get => unchecked((int)(m_data >> 16 & 0xFFUL));
            private set => m_data = (ulong)(value & 0xFF) << 16 | ~(0xFFUL << 16) & m_data;
        }

        public bool IsSet => m_data != 0;

        private ulong m_data;
    }
}
