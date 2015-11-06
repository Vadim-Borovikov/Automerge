using System;
using System.Collections.Generic;
using System.Linq;

namespace Automerger.Model
{
    public static class Utils
    {
        public static string[] GetSubArray(string[] source, int start, int afterFinish)
        {
            int length = afterFinish - start;
            var result = new string[length];
            Array.Copy(source, start, result, 0, length);
            return result;
        }

        public static Dictionary<T1, T2> ToDictionary<T1, T2>(IReadOnlyDictionary<T1, T2> dict)
        {
            return dict.ToDictionary(p => p.Key, p => p.Value);
        }

        public static void SwapDictionaries<T1, T2>(ref Dictionary<T1, T2> dict1,
                                                    ref Dictionary<T1, T2> dict2)
        {
            Dictionary<T1, T2> temp = dict1;
            dict1 = dict2;
            dict2 = temp;
        }
    }
}
