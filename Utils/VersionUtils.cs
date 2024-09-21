
using Foxworks.Version;

namespace Foxworks.Utils
{
    public static class VersionUtils
    {
        /// <summary>
        /// Compares two specified string versions and returns an integer that indicates their relative position in the sort order.
        /// Strings versions MUST take the form X.Y.Z where X, Y, and Z are non-negative integers.
        /// </summary>
        /// <param name="versionA">The first version to compare.</param>
        /// <param name="versionB">The second version to compare.</param>
        /// <returns>An integer less, equal or greater than zero depending on whether versionA is older, equal or newer than versionB.</returns>
        public static int CompareVersion(string versionA, string versionB)
        {
            SemVersion semVersionA = SemVersion.Parse(versionA);
            SemVersion semVersionB = SemVersion.Parse(versionB);

            return semVersionA.CompareTo(semVersionB);
        }
    }
}