using Microsoft.VisualStudio.TestTools.UnitTesting;
using Automerger.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Authomerger.Tests;

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
            Addition addition = set.Changes[0] as Addition;
            Assert.IsNotNull(addition);
            Assert.IsTrue(addition.Line == 0);
            Assert.IsTrue(addition.LinesAmount == 0);
            Assert.IsTrue(addition.Content.SequenceEqual(changed));

            // Simple removal
            source = new string[1] { "Test" };
            changed = new string[0];
            set = new ChangeSet(source, changed);
            Assert.IsTrue(set.Changes.Count == 1);
            Removal removal = set.Changes[0] as Removal;
            Assert.IsNotNull(removal);
            Assert.IsTrue(removal.Line == 0);
            Assert.IsTrue(removal.LinesAmount == 1);

            // Simple replacement
            changed = new string[1] { "Test2" };
            set = new ChangeSet(source, changed);
            Assert.IsTrue(set.Changes.Count == 1);
            Replacement replacement = set.Changes[0] as Replacement;
            Assert.IsNotNull(replacement);
            Assert.IsTrue(replacement.Line == 0);
            Assert.IsTrue(replacement.LinesAmount == 1);
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
            Assert.IsTrue(addition.Line == 0);
            Assert.IsTrue(addition.LinesAmount == 0);
            Assert.IsTrue(addition.Content.SequenceEqual(new string[1] { "0" }));
            removal = set.Changes[1] as Removal;
            Assert.IsNotNull(removal);
            Assert.IsTrue(removal.Line == 1);
            Assert.IsTrue(removal.LinesAmount == 1);
            replacement = set.Changes[2] as Replacement;
            Assert.IsNotNull(replacement);
            Assert.IsTrue(replacement.Line == 3);
            Assert.IsTrue(replacement.LinesAmount == 1);
            Assert.IsTrue(replacement.NewContent.SequenceEqual(new string[2] { "5", "6" }));

            // Removal - replacement - addition
            source  = new string[4] { "1", "2", "3", "4" };
            changed = new string[5] {      "2", "5", "4", "6", "7" };
            set = new ChangeSet(source, changed);
            Assert.IsTrue(set.Changes.Count == 3);
            removal = set.Changes[0] as Removal;
            Assert.IsNotNull(removal);
            Assert.IsTrue(removal.Line == 0);
            Assert.IsTrue(removal.LinesAmount == 1);
            replacement = set.Changes[1] as Replacement;
            Assert.IsNotNull(replacement);
            Assert.IsTrue(replacement.Line == 2);
            Assert.IsTrue(replacement.LinesAmount == 1);
            Assert.IsTrue(replacement.NewContent.SequenceEqual(new string[1] { "5" }));
            addition = set.Changes[2] as Addition;
            Assert.IsNotNull(addition);
            Assert.IsTrue(addition.Line == 4);
            Assert.IsTrue(addition.LinesAmount == 0);
            Assert.IsTrue(addition.Content.SequenceEqual(new string[2] { "6", "7" }));

            // Replacement - addition - removal
            source  = new string[4] { "1", "2",      "3", "4" };
            changed = new string[4] { "5", "2", "6", "3" };
            set = new ChangeSet(source, changed);
            Assert.IsTrue(set.Changes.Count == 3);
            replacement = set.Changes[0] as Replacement;
            Assert.IsNotNull(replacement);
            Assert.IsTrue(replacement.Line == 0);
            Assert.IsTrue(replacement.LinesAmount == 1);
            Assert.IsTrue(replacement.NewContent.SequenceEqual(new string[1] { "5" }));
            addition = set.Changes[1] as Addition;
            Assert.IsNotNull(addition);
            Assert.IsTrue(addition.Line == 2);
            Assert.IsTrue(addition.LinesAmount == 0);
            Assert.IsTrue(addition.Content.SequenceEqual(new string[1] { "6" }));
            removal = set.Changes[2] as Removal;
            Assert.IsNotNull(removal);
            Assert.IsTrue(removal.Line == 3);
            Assert.IsTrue(removal.LinesAmount == 1);
        }
    }
}