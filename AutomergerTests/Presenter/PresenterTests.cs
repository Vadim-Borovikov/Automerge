using Automerger.Presenter;
using Automerger.View.Tests;
using AutomergerTests;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;

namespace Automerger.Presenter.Tests
{
    [TestClass()]
    public class PresenterTests
    {
        [TestMethod()]
        public void PresenterTest()
        {
            var merger = new Model.DummyMerger();
            var view = new DummyView();

            MyAssert.Throws<ArgumentNullException>(() => new Presenter(null, view));
            MyAssert.Throws<ArgumentNullException>(() => new Presenter(merger, null));
        }

        [TestMethod()]
        public void LoadSourceTest()
        {
            var merger = new Model.DummyMerger();
            var view = new DummyView();

            var presenter = new Presenter(merger, view);

            LoadFileTest(presenter.TryLoadSource);
        }

        [TestMethod()]
        public void LoadChanged1Test()
        {
            var merger = new Model.DummyMerger();
            var view = new DummyView();

            var presenter = new Presenter(merger, view);

            LoadFileTest(presenter.TryLoadChanged1);
        }

        [TestMethod()]
        public void LoadChanged2Test()
        {
            var merger = new Model.DummyMerger();
            var view = new DummyView();

            var presenter = new Presenter(merger, view);

            LoadFileTest(presenter.TryLoadChanged2);
        }

        [TestMethod()]
        public void IsReadyForMergeTest()
        {
            var merger = new Model.DummyMerger();
            var view = new DummyView();

            var presenter = new Presenter(merger, view);
            Assert.IsFalse(presenter.IsReadyForMerge());

            string path = Path.GetTempFileName();
            Assert.IsTrue(File.Exists(path));
            Assert.IsTrue(presenter.TryLoadSource(path));
            File.Delete(path);
            Assert.IsFalse(presenter.IsReadyForMerge());

            path = Path.GetTempFileName();
            Assert.IsTrue(File.Exists(path));
            Assert.IsTrue(presenter.TryLoadChanged1(path));
            File.Delete(path);
            Assert.IsFalse(presenter.IsReadyForMerge());

            path = Path.GetTempFileName();
            Assert.IsTrue(File.Exists(path));
            Assert.IsTrue(presenter.TryLoadChanged2(path));
            File.Delete(path);
            Assert.IsTrue(presenter.IsReadyForMerge());
        }

        [TestMethod()]
        public void MergeTest()
        {
            var merger = new Model.DummyMerger();
            var view = new DummyView();

            var presenter = new Presenter(merger, view);

            string path = Path.GetTempFileName();
            Assert.IsTrue(File.Exists(path));
            Assert.IsTrue(presenter.TryLoadSource(path));
            File.Delete(path);
            MyAssert.Throws<ArgumentNullException>(() => presenter.Merge());

            path = Path.GetTempFileName();
            Assert.IsTrue(File.Exists(path));
            Assert.IsTrue(presenter.TryLoadChanged1(path));
            File.Delete(path);
            MyAssert.Throws<ArgumentNullException>(() => presenter.Merge());

            path = Path.GetTempFileName();
            Assert.IsTrue(File.Exists(path));
            Assert.IsTrue(presenter.TryLoadChanged2(path));
            File.Delete(path);
            MyAssert.ThrowsNothing(() => presenter.Merge());
        }

        [TestMethod()]
        public void SaveResultTest()
        {
            var merger = new Model.DummyMerger();
            var view = new DummyView();

            var presenter = new Presenter(merger, view);

            MyAssert.Throws<ArgumentNullException>(() => presenter.TrySaveResult(null));

            string resultPath = Path.GetTempFileName();
            Assert.IsTrue(File.Exists(resultPath));
            MyAssert.Throws<ArgumentNullException>(() => presenter.TrySaveResult(resultPath));

            string path = Path.GetTempFileName();
            Assert.IsTrue(File.Exists(path));
            File.WriteAllText(path, "0");
            Assert.IsTrue(presenter.TryLoadSource(path));
            File.Delete(path);
            MyAssert.Throws<ArgumentNullException>(() => presenter.TrySaveResult(resultPath));

            path = Path.GetTempFileName();
            Assert.IsTrue(File.Exists(path));
            File.WriteAllText(path, "1");
            Assert.IsTrue(presenter.TryLoadChanged1(path));
            File.Delete(path);
            MyAssert.Throws<ArgumentNullException>(() => presenter.TrySaveResult(resultPath));

            path = Path.GetTempFileName();
            Assert.IsTrue(File.Exists(path));
            File.WriteAllText(path, "1");
            Assert.IsTrue(presenter.TryLoadChanged2(path));
            File.Delete(path);
            MyAssert.Throws<ArgumentNullException>(() => presenter.TrySaveResult(resultPath));

            presenter.Merge();
            Assert.IsTrue(presenter.TrySaveResult(resultPath));

            string[] result = File.ReadAllLines(resultPath);
            File.Delete(resultPath);

            Assert.IsTrue(result.SequenceEqual(new string[] { "1" }));
        }

        private void LoadFileTest(Func<string, bool> tryLoadFile)
        {
            MyAssert.Throws<ArgumentNullException>(() => tryLoadFile(null));
            MyAssert.Throws<ArgumentException>(() => tryLoadFile(""));

            string path = Path.GetTempFileName();
            Assert.IsTrue(File.Exists(path));
            Assert.IsTrue(tryLoadFile(path));
            File.Delete(path);
        }
    }
}