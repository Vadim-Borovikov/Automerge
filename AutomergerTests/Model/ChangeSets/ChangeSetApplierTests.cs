using AutomergerTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Automerger.Model.Tests
{
    [TestClass()]
    public class ChangeSetApplierTests
    {
        [TestMethod()]
        public void ApplyTest()
        {
            var changes = new Dictionary<int, IChange>();
            var source = new string[0];

            MyAssert.Throws<ArgumentNullException>(() => ChangeSetApplier.Apply(null, source));
            MyAssert.Throws<ArgumentNullException>(() => ChangeSetApplier.Apply(changes, null));

            changes[0] = new Replacement(0, 1, new string[] { "0" });
            MyAssert.Throws<ArgumentOutOfRangeException>(
                () => ChangeSetApplier.Apply(changes, source));

            source = new string[] { "0" };
            changes[0] = new Replacement(0, 2, new string[] { "0" });
            MyAssert.Throws<ArgumentOutOfRangeException>(
                () => ChangeSetApplier.Apply(changes, source));

            source = new string[0];
            changes[0] = new Addition(0, new string[] { "0" });
            MyAssert.ThrowsNothing(() => ChangeSetApplier.Apply(changes, source));
        }
    }
}
