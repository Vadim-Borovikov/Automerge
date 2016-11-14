using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutomergeTests
{
    internal static class MyAssert
    {
        internal static void Throws<T>(Action action) where T : Exception
        {
            try
            {
                action();
                Assert.Fail();
            }
            catch (Exception e)
            {
                Assert.IsInstanceOfType(e, typeof(T));
            }
        }
    }
}
