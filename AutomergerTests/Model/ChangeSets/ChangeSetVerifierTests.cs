using AutomergerTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace Automerger.Model.Tests

{
    [TestClass()]
    public class ChangeSetVerifierTests
    {
        [TestMethod()]
        public void VerifyTest()
        {
            var changes = new Dictionary<int, IChange>();

            MyAssert.Throws<ArgumentNullException>(
                () => ChangeSetVerifier.Verify<IChange>(null, 0));
            MyAssert.Throws<ArgumentOutOfRangeException>(
                () => ChangeSetVerifier.Verify(changes, -1));

            changes[1] = new Addition(1, new string[] { "0" });
            MyAssert.Throws<ArgumentOutOfRangeException>(
                () => ChangeSetVerifier.Verify(changes, 0));
            MyAssert.ThrowsNothing(() => ChangeSetVerifier.Verify(changes, 1));

            changes[1] = new Removal(1, 2);
            changes[2] = new Removal(2, 2);
            MyAssert.Throws<ArgumentOutOfRangeException>(
                () => ChangeSetVerifier.Verify(changes, 3));
        }
    }
}