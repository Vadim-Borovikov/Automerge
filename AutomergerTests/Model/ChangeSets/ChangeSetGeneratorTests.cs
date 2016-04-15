using System;
using System.Collections.Generic;
using System.Linq;
using AutomergerTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Automerger.Model.Tests
{
    [TestClass]
    public class ChangeSetGeneratorTests
    {
        [TestMethod]
        public void GenerateTest()
        {
            var source = new string[0];
            var changed = new string[0];

            MyAssert.Throws<ArgumentNullException>(() => ChangeSetGenerator.Generate(null, null));
            string[] changed1 = changed;
            MyAssert.Throws<ArgumentNullException>(() => ChangeSetGenerator.Generate(null, changed1));
            string[] source1 = source;
            MyAssert.Throws<ArgumentNullException>(() => ChangeSetGenerator.Generate(source1, null));

            Dictionary<int, IMergableChange> changes = ChangeSetGenerator.Generate(source, changed);
            ChangeSetVerifier.Verify(changes, source.Length);
            Assert.IsTrue(changes.Count == 0);

            // Simple addition
            changed = new[] { "Test" };
            changes = ChangeSetGenerator.Generate(source, changed);
            ChangeSetVerifier.Verify(changes, source.Length);
            Assert.IsTrue(changes.Count == 1);
            var addition = changes[0] as Addition;
            Assert.IsNotNull(addition);
            Assert.IsTrue(addition.Start == 0);
            Assert.IsTrue(addition.RemovedAmount == 0);
            Assert.IsTrue(addition.AfterFinish == 0);
            Assert.IsTrue(addition.NewContent.SequenceEqual(changed));

            // Simple removal
            source = new[] { "Test" };
            changed = new string[0];
            changes = ChangeSetGenerator.Generate(source, changed);
            ChangeSetVerifier.Verify(changes, source.Length);
            Assert.IsTrue(changes.Count == 1);
            var removal = changes[0] as Removal;
            Assert.IsNotNull(removal);
            Assert.IsTrue(removal.Start == 0);
            Assert.IsTrue(removal.RemovedAmount == 1);
            Assert.IsTrue(removal.AfterFinish == 1);

            // Simple replacement
            changed = new[] { "Test2" };
            changes = ChangeSetGenerator.Generate(source, changed);
            ChangeSetVerifier.Verify(changes, source.Length);
            Assert.IsTrue(changes.Count == 1);
            var replacement = changes[0] as Replacement;
            Assert.IsNotNull(replacement);
            Assert.IsTrue(replacement.Start == 0);
            Assert.IsTrue(replacement.RemovedAmount == 1);
            Assert.IsTrue(replacement.AfterFinish == 1);
            Assert.IsTrue(replacement.NewContent.SequenceEqual(changed));

            // Trimming
            source = new[] { "    Test" };
            changed = new[] { "Test\t\t" };
            changes = ChangeSetGenerator.Generate(source, changed);
            ChangeSetVerifier.Verify(changes, source.Length);
            Assert.IsTrue(changes.Count == 0);

            // Addition - removal - replacement
            source = new[] { "1", "2", "3", "4" };
            changed = new[] { "0", "1", "3", "5", "6" };
            changes = ChangeSetGenerator.Generate(source, changed);
            ChangeSetVerifier.Verify(changes, source.Length);
            Assert.IsTrue(changes.Count == 3);
            addition = changes[0] as Addition;
            Assert.IsNotNull(addition);
            Assert.IsTrue(addition.Start == 0);
            Assert.IsTrue(addition.RemovedAmount == 0);
            Assert.IsTrue(addition.AfterFinish == 0);
            Assert.IsTrue(addition.NewContent.SequenceEqual(new[] { "0" }));
            removal = changes[1] as Removal;
            Assert.IsNotNull(removal);
            Assert.IsTrue(removal.Start == 1);
            Assert.IsTrue(removal.RemovedAmount == 1);
            Assert.IsTrue(removal.AfterFinish == 2);
            replacement = changes[3] as Replacement;
            Assert.IsNotNull(replacement);
            Assert.IsTrue(replacement.Start == 3);
            Assert.IsTrue(replacement.RemovedAmount == 1);
            Assert.IsTrue(replacement.AfterFinish == 4);
            Assert.IsTrue(replacement.NewContent.SequenceEqual(new[] { "5", "6" }));

            // Removal - replacement - addition
            source = new[] { "1", "2", "3", "4" };
            changed = new[] { "2", "5", "4", "6", "7" };
            changes = ChangeSetGenerator.Generate(source, changed);
            ChangeSetVerifier.Verify(changes, source.Length);
            Assert.IsTrue(changes.Count == 3);
            removal = changes[0] as Removal;
            Assert.IsNotNull(removal);
            Assert.IsTrue(removal.Start == 0);
            Assert.IsTrue(removal.RemovedAmount == 1);
            Assert.IsTrue(removal.AfterFinish == 1);
            replacement = changes[2] as Replacement;
            Assert.IsNotNull(replacement);
            Assert.IsTrue(replacement.Start == 2);
            Assert.IsTrue(replacement.RemovedAmount == 1);
            Assert.IsTrue(replacement.AfterFinish == 3);
            Assert.IsTrue(replacement.NewContent.SequenceEqual(new[] { "5" }));
            addition = changes[4] as Addition;
            Assert.IsNotNull(addition);
            Assert.IsTrue(addition.Start == 4);
            Assert.IsTrue(addition.RemovedAmount == 0);
            Assert.IsTrue(addition.AfterFinish == 4);
            Assert.IsTrue(addition.NewContent.SequenceEqual(new[] { "6", "7" }));

            // Replacement - addition - removal
            source = new[] { "1", "2", "3", "4" };
            changed = new[] { "5", "2", "6", "3" };
            changes = ChangeSetGenerator.Generate(source, changed);
            ChangeSetVerifier.Verify(changes, source.Length);
            Assert.IsTrue(changes.Count == 3);
            replacement = changes[0] as Replacement;
            Assert.IsNotNull(replacement);
            Assert.IsTrue(replacement.Start == 0);
            Assert.IsTrue(replacement.RemovedAmount == 1);
            Assert.IsTrue(replacement.AfterFinish == 1);
            Assert.IsTrue(replacement.NewContent.SequenceEqual(new[] { "5" }));
            addition = changes[2] as Addition;
            Assert.IsNotNull(addition);
            Assert.IsTrue(addition.Start == 2);
            Assert.IsTrue(addition.RemovedAmount == 0);
            Assert.IsTrue(addition.AfterFinish == 2);
            Assert.IsTrue(addition.NewContent.SequenceEqual(new[] { "6" }));
            removal = changes[3] as Removal;
            Assert.IsNotNull(removal);
            Assert.IsTrue(removal.Start == 3);
            Assert.IsTrue(removal.RemovedAmount == 1);
            Assert.IsTrue(removal.AfterFinish == 4);
        }
    }
}
