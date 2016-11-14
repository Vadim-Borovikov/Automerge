using System.Collections.Generic;

namespace Automerge
{
    /// <summary>
    /// Utility
    /// </summary>
    internal static class Utils
    {
        /// <summary>
        /// Gets the sub array.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="start">The start.</param>
        /// <param name="afterFinish">The number of the line that is one past last.</param>
        /// <returns></returns>
        internal static IEnumerable<string> GetSubArray(IReadOnlyList<string> source, int start, int afterFinish)
        {
            for (int i = start; i < afterFinish; ++i)
            {
                yield return source[i];
            }
        }

        /// <summary>
        /// Tries the cast one of objects to T
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="o1">The first object.</param>
        /// <param name="o2">The second object.</param>
        /// <returns></returns>
        internal static T TryCastOneOf<T>(object o1, object o2)
            where T : class
        {
            var result = o1 as T;
            if (result != null)
            {
                return result;
            }
            return o2 as T;
        }
    }
}
