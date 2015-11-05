using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace AutomergerTests
{
    public static class MyAssert
    {
        public static void Throws<T>(Action action) where T : Exception
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

        public static void ThrowsNothing(Action action)
        {
            try
            {
                action();
            }
            catch
            {
                Assert.Fail();
            }
        }
    }
}
