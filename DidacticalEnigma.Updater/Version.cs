using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Optional;

namespace DidacticalEnigma.Updater
{
    public class Version : IComparable<Version>, IComparable
    {
        public int CompareTo(Version other)
        {
            if (ReferenceEquals(this, other)) return 0;
            if (ReferenceEquals(null, other)) return 1;
            var majorComparison = Major.CompareTo(other.Major);
            if (majorComparison != 0) return majorComparison;
            var minorComparison = Minor.CompareTo(other.Minor);
            if (minorComparison != 0) return minorComparison;
            return Patch.CompareTo(other.Patch);
        }

        public int CompareTo(object obj)
        {
            if (ReferenceEquals(null, obj)) return 1;
            if (ReferenceEquals(this, obj)) return 0;
            return obj is Version other ? CompareTo(other) : throw new ArgumentException($"Object must be of type {nameof(Version)}");
        }

        public static bool operator <(Version left, Version right)
        {
            return Comparer<Version>.Default.Compare(left, right) < 0;
        }

        public static bool operator >(Version left, Version right)
        {
            return Comparer<Version>.Default.Compare(left, right) > 0;
        }

        public static bool operator <=(Version left, Version right)
        {
            return Comparer<Version>.Default.Compare(left, right) <= 0;
        }

        public static bool operator >=(Version left, Version right)
        {
            return Comparer<Version>.Default.Compare(left, right) >= 0;
        }

        private static readonly Regex versionMatcher = new Regex(@"^v?([0-9]+)\.?([0-9]+)?\.?([0-9]+)?");
        
        public int Major { get; }

        public int Minor { get; }

        public int Patch { get; }

        public Version(int major, int minor, int patch)
        {
            Major = major;
            Minor = minor;
            Patch = patch;
        }

        public static Option<Version> Parse(string versionText)
        {
            var match = versionMatcher.Match(versionText);
            if (match.Success)
            {
                var majorGroup = match.Groups[1];
                var major = majorGroup.Success ? int.Parse(majorGroup.Value) : 0;
                var minorGroup = match.Groups[2];
                var minor = minorGroup.Success ? int.Parse(minorGroup.Value) : 0;
                var patchGroup = match.Groups[3];
                var patch = patchGroup.Success ? int.Parse(patchGroup.Value) : 0;
                return new Version(major, minor, patch).Some();
            }
            else
            {
                return Option.None<Version>();
            }
        }

        public override string ToString()
        {
            return $"{Major}.{Minor}.{Patch}";
        }
    }
}