using System;

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
    }
}
