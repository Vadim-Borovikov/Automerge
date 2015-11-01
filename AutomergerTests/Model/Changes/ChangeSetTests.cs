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

            changed = new string[1] { "Test" };
            set = new ChangeSet(source, changed);
            Assert.IsTrue(set.Changes.Count == 1);
            Addition addition = set.Changes[0] as Addition;
            Assert.IsNotNull(addition);
            Assert.IsTrue(addition.Line == 0);
            Assert.IsTrue(addition.LinesAmount == 0);
            Assert.IsTrue(addition.Content.SequenceEqual(changed));

            source = new string[1] { "Test" };
            changed = new string[0];
            set = new ChangeSet(source, changed);
            Assert.IsTrue(set.Changes.Count == 1);
            Removal removal = set.Changes[0] as Removal;
            Assert.IsNotNull(removal);
            Assert.IsTrue(removal.Line == 0);
            Assert.IsTrue(removal.LinesAmount == 1);

            changed = new string[1] { "Test2" };
            set = new ChangeSet(source, changed);
            Assert.IsTrue(set.Changes.Count == 1);
            Replacement replacement = set.Changes[0] as Replacement;
            Assert.IsNotNull(replacement);
            Assert.IsTrue(replacement.Line == 0);
            Assert.IsTrue(replacement.LinesAmount == 1);
            Assert.IsTrue(replacement.NewContent.SequenceEqual(changed));

            source = new string[1] { "    Test" };
            changed = new string[1] { "Test\t\t" };
            set = new ChangeSet(source, changed);
            Assert.IsTrue(set.Changes.Count == 0);
        }
    }
}