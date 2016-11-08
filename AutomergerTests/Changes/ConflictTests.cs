using System;
using System.Collections.Generic;
using System.Linq;
using Automerger;
using Automerger.Changes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace AutomergerTests.Changes
{
    [TestClass]
    public class ConflictTests
    {
        [TestMethod]
        public void ConflictTest()
        {
            var emptyContent = new string[0];
            var content1 = new[] { "1" };
            var content2 = new[] { "2" };

            var source = emptyContent;
            IMergableChange change1 = new Addition(0, content1);
            IMergableChange change2 = new Addition(0, content2);

            CheckPair<ArgumentNullException>(null, change2, source);
            IMergableChange change3 = change1;
            IMergableChange change4 = change2;
            MyAssert.Throws<ArgumentNullException>(() => new Conflict(change3, change4, null));

            change1 = new Addition(0, content1);
            change2 = new Addition(1, content2);
            CheckPair<ArgumentOutOfRangeException>(change1, change2, source);

            source = new[] { "0" };
            change1 = new Addition(0, content1);
            change2 = new Addition(1, content2);
            CheckPair<ArgumentException>(change1, change2, source);

            change1 = new Addition(0, content1);
            change2 = new Addition(0, content2);
            var conflict = new Conflict(change1, change2, source);
            Assert.IsTrue(conflict.Start == 0);
            Assert.IsTrue(conflict.RemovedAmount == 0);
            Assert.IsTrue(conflict.AfterFinish == 0);
            CheckConflictContent(conflict, emptyContent, content1, content2);

            source = new[] { "0", "1", "2" };
            change1 = new Replacement(0, 2, new[] { "10" });
            change2 = new Replacement(1, 2, new[] { "20", "30" });
            conflict = new Conflict(change1, change2, source);
            Assert.IsTrue(conflict.Start == 0);
            Assert.IsTrue(conflict.RemovedAmount == 3);
            Assert.IsTrue(conflict.AfterFinish == 3);
            CheckConflictContent(conflict, source, new[] { "10", "2" }, new[] { "0", "20", "30" });
        }

        private static void CheckPair<T>(IMergableChange change1, IMergableChange change2, string[] source)
            where T : Exception
        {
            MyAssert.Throws<T>(() => new Conflict(change1, change2, source));
            MyAssert.Throws<T>(() => new Conflict(change2, change1, source));
        }

        private static void CheckConflictContent(IChange conflict, IEnumerable<string> source,
                                                 IEnumerable<string> changed1, IEnumerable<string> changed2)
        {
            var expected = new List<string>
            {
                Consts.CONFLICT_BLOCK_BEGIN,
                Consts.CONFLICT_BLOCK_SOURCE
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