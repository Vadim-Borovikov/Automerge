using AutomergerTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Automerger.Model.Tests
{
    [TestClass()]
    public class ConflictTests
    {
        [TestMethod()]
        public void ConflictTest()
        {
            var emptyContent = new string[0];
            var content1 = new string[] { "1" };
            var content2 = new string[] { "2" };

            var source = emptyContent;
            IMergableChange change1 = new Addition(0, content1);
            IMergableChange change2 = new Addition(0, content2);

            CheckPair<ArgumentNullException>(null, change2, source, 0);
            MyAssert.Throws<ArgumentNullException>(() => new Conflict(change1, change2, null, 0));
            MyAssert.Throws<ArgumentOutOfRangeException>(
                () => new Conflict(change1, change2, source, -1));

            change1 = new Addition(0, content1);
            change2 = new Addition(1, content2);
            CheckPair<ArgumentOutOfRangeException>(change1, change2, source, 0);

            source = new string[] { "0" };
            change1 = new Addition(0, content1);
            change2 = new Addition(1, content2);
            CheckPair<ArgumentException>(change1, change2, source, 0);

            change1 = new Addition(0, content1);
            change2 = new Addition(0, content2);
            var conflict = new Conflict(change1, change2, source, 0);
            Assert.IsTrue(conflict.Start == 0);
            Assert.IsTrue(conflict.RemovedAmount == 0);
            CheckConflictContent(conflict, emptyContent, content1, content2);

            source = new string[] { "0", "1", "2" };
            change1 = new Replacement(0, 2, new string[] { "10" });
            change2 = new Replacement(1, 2, new string[] { "20", "30" });
            conflict = new Conflict(change1, change2, source, 0);
            Assert.IsTrue(conflict.Start == 0);
            Assert.IsTrue(conflict.RemovedAmount == 3);
            CheckConflictContent(conflict, source,
                                 new string[] { "10", "2" }, new string[] { "0", "20", "30" });
        }

        private void CheckPair<T>(IMergableChange change1, IMergableChange change2,
                                  string[] source, int start) where T : Exception
        {
            MyAssert.Throws<T>(() => new Conflict(change1, change2, source, start));
            MyAssert.Throws<T>(() => new Conflict(change2, change1, source, start));
        }

        private void CheckConflictContent(Conflict conflict, string[] source,
                                          string[] changed1, string[] changed2)
        {
            var expected = new List<string>
            {
                Consts.CONFLICT_BLOCK_BEGIN,
                Consts.CONFLICT_BLOCK_SOURCE,
            };
            expected.AddRange(source);
            expected.Add(Consts.CONFLICT_BLOCK_CHANGED1);
            expected.AddRange(changed1);
            expected.Add(Consts.CONFLICT_BLOCK_CHANGED2);
            expected.AddRange(changed2);
            expected.Add(Consts.CONFLICT_BLOCK_END);

            Assert.IsTrue(conflict.NewContent.SequenceEqual(expected));
        }
    }
}