using AutomergerTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Automerger.Model.Tests
{
    [TestClass()]
    public class ChangeSetApplyerTests
    {
        [TestMethod()]
        public void ApplyTest()
        {
            var changes = new Dictionary<int, IChange>();
            var source = new string[0];

            MyAssert.Throws<ArgumentNullException>(() => ChangeSetApplyer.Apply(null, source));
            MyAssert.Throws<ArgumentNullException>(() => ChangeSetApplyer.Apply(changes, null));

            changes[0] = new Replacement(0, 1, new string[1] { "0" });
            MyAssert.Throws<ArgumentOutOfRangeException>(
                () => ChangeSetApplyer.Apply(changes, source));

            source = new string[1] { "0" };
            changes[0] = new Replacement(0, 2, new string[1] { "0" });
            MyAssert.Throws<ArgumentOutOfRangeException>(
                () => ChangeSetApplyer.Apply(changes, source));

            source = new string[0];
            changes[0] = new Addition(0, new string[1] { "0" });
            MyAssert.ThrowsNothing(() => ChangeSetApplyer.Apply(changes, source));
        }
    }
}
