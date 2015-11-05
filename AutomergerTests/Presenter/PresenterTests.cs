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

            LoadFileTest(presenter.LoadSource);
        }

        [TestMethod()]
        public void LoadChanged1Test()
        {
            var merger = new Model.DummyMerger();
            var view = new DummyView();

            var presenter = new Presenter(merger, view);

            LoadFileTest(presenter.LoadChanged1);
        }

        [TestMethod()]
        public void LoadChanged2Test()
        {
            var merger = new Model.DummyMerger();
            var view = new DummyView();

            var presenter = new Presenter(merger, view);

            LoadFileTest(presenter.LoadChanged2);
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
            presenter.LoadSource(path);
            File.Delete(path);
            Assert.IsFalse(presenter.IsReadyForMerge());

            path = Path.GetTempFileName();
            Assert.IsTrue(File.Exists(path));
            presenter.LoadChanged1(path);
            File.Delete(path);
            Assert.IsFalse(presenter.IsReadyForMerge());

            path = Path.GetTempFileName();
            Assert.IsTrue(File.Exists(path));
            presenter.LoadChanged2(path);
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
            presenter.LoadSource(path);
            File.Delete(path);
            MyAssert.Throws<ArgumentNullException>(() => presenter.Merge());

            path = Path.GetTempFileName();
            Assert.IsTrue(File.Exists(path));
            presenter.LoadChanged1(path);
            File.Delete(path);
            MyAssert.Throws<ArgumentNullException>(() => presenter.Merge());

            path = Path.GetTempFileName();
            Assert.IsTrue(File.Exists(path));
            presenter.LoadChanged2(path);
            File.Delete(path);
            MyAssert.ThrowsNothing(() => presenter.Merge());
        }

        [TestMethod()]
        public void SaveResultTest()
        {
            var merger = new Model.DummyMerger();
            var view = new DummyView();

            var presenter = new Presenter(merger, view);

            MyAssert.Throws<ArgumentNullException>(() => presenter.SaveResult(null));

            string resultPath = Path.GetTempFileName();
            Assert.IsTrue(File.Exists(resultPath));
            MyAssert.Throws<ArgumentNullException>(() => presenter.SaveResult(resultPath));

            string path = Path.GetTempFileName();
            Assert.IsTrue(File.Exists(path));
            File.WriteAllText(path, "0");
            presenter.LoadSource(path);
            File.Delete(path);
            MyAssert.Throws<ArgumentNullException>(() => presenter.SaveResult(resultPath));

            path = Path.GetTempFileName();
            Assert.IsTrue(File.Exists(path));
            File.WriteAllText(path, "1");
            presenter.LoadChanged1(path);
            File.Delete(path);
            MyAssert.Throws<ArgumentNullException>(() => presenter.SaveResult(resultPath));

            path = Path.GetTempFileName();
            Assert.IsTrue(File.Exists(path));
            File.WriteAllText(path, "1");
            presenter.LoadChanged2(path);
            File.Delete(path);
            MyAssert.Throws<ArgumentNullException>(() => presenter.SaveResult(resultPath));

            presenter.Merge();
            presenter.SaveResult(resultPath);

            string[] result = File.ReadAllLines(resultPath);
            File.Delete(resultPath);

            Assert.IsTrue(result.SequenceEqual(new string[1] { "1" }));
        }

        private void LoadFileTest(Action<string> loadFile)
        {
            MyAssert.Throws<ArgumentNullException>(() => loadFile(null));
            MyAssert.Throws<ArgumentException>(() => loadFile(""));

            string path = "LoadSourceTest";
            Assert.IsFalse(File.Exists(path));
            MyAssert.Throws<FileNotFoundException>(() => loadFile(path));

            path = Path.GetTempFileName();
            Assert.IsTrue(File.Exists(path));
            MyAssert.ThrowsNothing(() => loadFile(path));
            File.Delete(path);
        }
    }
}