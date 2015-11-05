using AutomergerTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Automerger.Model.Tests
{
    [TestClass()]
    public class ChangeSetTests
    {
        [TestMethod()]
        public void ChangeSetTest()
        {
            string[] source = new string[0];
            string[] changed = new string[0];

            MyAssert.Throws<ArgumentNullException>(() => new ChangeSet(null, null)); 
            MyAssert.Throws<ArgumentNullException>(() => new ChangeSet(null, changed)); 
            MyAssert.Throws<ArgumentNullException>(() => new ChangeSet(source, null));

            var set = new ChangeSet(source, changed);
            Assert.IsTrue(set.Changes.Count == 0);

            // Simple addition
            changed = new string[1] { "Test" };
            set = new ChangeSet(source, changed);
            Assert.IsTrue(set.Changes.Count == 1);
            var addition = set.Changes[0] as Addition;
            Assert.IsNotNull(addition);
            Assert.IsTrue(addition.Start == 0);
            Assert.IsTrue(addition.RemovedAmount == 0);
            Assert.IsTrue(addition.Finish == 0);
            Assert.IsTrue(addition.NewContent.SequenceEqual(changed));

            // Simple removal
            source = new string[1] { "Test" };
            changed = new string[0];
            set = new ChangeSet(source, changed);
            Assert.IsTrue(set.Changes.Count == 1);
            var removal = set.Changes[0] as Removal;
            Assert.IsNotNull(removal);
            Assert.IsTrue(removal.Start == 0);
            Assert.IsTrue(removal.RemovedAmount == 1);
            Assert.IsTrue(removal.Finish == 1);

            // Simple replacement
            changed = new string[1] { "Test2" };
            set = new ChangeSet(source, changed);
            Assert.IsTrue(set.Changes.Count == 1);
            var replacement = set.Changes[0] as Replacement;
            Assert.IsNotNull(replacement);
            Assert.IsTrue(replacement.Start == 0);
            Assert.IsTrue(replacement.RemovedAmount == 1);
            Assert.IsTrue(replacement.Finish == 1);
            Assert.IsTrue(replacement.NewContent.SequenceEqual(changed));

            // Trimming
            source = new string[1] { "    Test" };
            changed = new string[1] { "Test\t\t" };
            set = new ChangeSet(source, changed);
            Assert.IsTrue(set.Changes.Count == 0);

            // Addition - removal - replacement
            source  = new string[4] {      "1", "2", "3", "4" };
            changed = new string[5] { "0", "1",      "3", "5", "6" };
            set = new ChangeSet(source, changed);
            Assert.IsTrue(set.Changes.Count == 3);
            addition = set.Changes[0] as Addition;
            Assert.IsNotNull(addition);
            Assert.IsTrue(addition.Start == 0);
            Assert.IsTrue(addition.RemovedAmount == 0);
            Assert.IsTrue(addition.Finish == 0);
            Assert.IsTrue(addition.NewContent.SequenceEqual(new string[1] { "0" }));
            removal = set.Changes[1] as Removal;
            Assert.IsNotNull(removal);
            Assert.IsTrue(removal.Start == 1);
            Assert.IsTrue(removal.RemovedAmount == 1);
            Assert.IsTrue(removal.Finish == 2);
            replacement = set.Changes[3] as Replacement;
            Assert.IsNotNull(replacement);
            Assert.IsTrue(replacement.Start == 3);
            Assert.IsTrue(replacement.RemovedAmount == 1);
            Assert.IsTrue(replacement.Finish == 4);
            Assert.IsTrue(replacement.NewContent.SequenceEqual(new string[2] { "5", "6" }));

            // Removal - replacement - addition
            source  = new string[4] { "1", "2", "3", "4" };
            changed = new string[5] {      "2", "5", "4", "6", "7" };
            set = new ChangeSet(source, changed);
            Assert.IsTrue(set.Changes.Count == 3);
            removal = set.Changes[0] as Removal;
            Assert.IsNotNull(removal);
            Assert.IsTrue(removal.Start == 0);
            Assert.IsTrue(removal.RemovedAmount == 1);
            Assert.IsTrue(removal.Finish == 1);
            replacement = set.Changes[2] as Replacement;
            Assert.IsNotNull(replacement);
            Assert.IsTrue(replacement.Start == 2);
            Assert.IsTrue(replacement.RemovedAmount == 1);
            Assert.IsTrue(replacement.Finish == 3);
            Assert.IsTrue(replacement.NewContent.SequenceEqual(new string[1] { "5" }));
            addition = set.Changes[4] as Addition;
            Assert.IsNotNull(addition);
            Assert.IsTrue(addition.Start == 4);
            Assert.IsTrue(addition.RemovedAmount == 0);
            Assert.IsTrue(addition.Finish == 4);
            Assert.IsTrue(addition.NewContent.SequenceEqual(new string[2] { "6", "7" }));

            // Replacement - addition - removal
            source  = new string[4] { "1", "2",      "3", "4" };
            changed = new string[4] { "5", "2", "6", "3" };
            set = new ChangeSet(source, changed);
            Assert.IsTrue(set.Changes.Count == 3);
            replacement = set.Changes[0] as Replacement;
            Assert.IsNotNull(replacement);
            Assert.IsTrue(replacement.Start == 0);
            Assert.IsTrue(replacement.RemovedAmount == 1);
            Assert.IsTrue(replacement.Finish == 1);
            Assert.IsTrue(replacement.NewContent.SequenceEqual(new string[1] { "5" }));
            addition = set.Changes[2] as Addition;
            Assert.IsNotNull(addition);
            Assert.IsTrue(addition.Start == 2);
            Assert.IsTrue(addition.RemovedAmount == 0);
            Assert.IsTrue(addition.Finish == 2);
            Assert.IsTrue(addition.NewContent.SequenceEqual(new string[1] { "6" }));
            removal = set.Changes[3] as Removal;
            Assert.IsNotNull(removal);
            Assert.IsTrue(removal.Start == 3);
            Assert.IsTrue(removal.RemovedAmount == 1);
            Assert.IsTrue(removal.Finish == 4);
        }
    }
}