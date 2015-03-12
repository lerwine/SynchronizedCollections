using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SynchronizedCollections;
using System.Collections;

namespace SynchronizedListsUnitTestProject
{
    [TestClass]
    public class SynchronizedReadOnlyListUnitTest
    {
        [TestMethod]
        public void ConstructorTestMethod()
        {
            SynchronizedReadOnlyList<HelperObject1> target1 = new SynchronizedReadOnlyList<HelperObject1>(7);

            Assert.AreEqual(7, target1.Count);

            for (int i = 0; i < 7; i++)
            {
                Assert.IsNotNull(target1[i]);
                Assert.AreEqual(i, target1[i].Id);
            }

            SynchronizedReadOnlyList<HelperObject2> target2 = new SynchronizedReadOnlyList<HelperObject2>(new HelperObject2(1), 
                new HelperObject2(1), new HelperObject2(3));

            Assert.AreEqual(3, target2.Count);

            Assert.AreEqual(1, target2[0].Id);
            Assert.AreEqual(1, target2[1].Id);
            Assert.AreNotSame(target2[0], target2[1]);
            Assert.AreEqual(3, target2[2].Id);

            try
            {
                target2 = new SynchronizedReadOnlyList<HelperObject2>(3);
            }
            catch (MissingMethodException) { }
        }

        [TestMethod]
        public void MutationsTest()
        {
            SynchronizedReadOnlyList<HelperObject1> list = new SynchronizedReadOnlyList<HelperObject1>(7);
            IList target = list;

            HelperObject1[] items = list.ToArray();

            try
            {
                target.Add(new HelperObject1());
            }
            catch (NotSupportedException) { }

            Assert.AreEqual(items.Length, list.Count);
            for (int i = 0; i < items.Length; i++)
            {
                Assert.IsNotNull(items[i]);
                Assert.AreSame(items[i], list[i]);
            }

            try
            {
                target.Clear();
            }
            catch (NotSupportedException) { }

            Assert.AreEqual(items.Length, list.Count);
            for (int i = 0; i < items.Length; i++)
            {
                Assert.IsNotNull(items[i]);
                Assert.AreSame(items[i], list[i]);
            }

            try
            {
                target.Insert(1, new HelperObject1());
            }
            catch (NotSupportedException) { }

            Assert.AreEqual(items.Length, list.Count);
            for (int i = 0; i < items.Length; i++)
            {
                Assert.IsNotNull(items[i]);
                Assert.AreSame(items[i], list[i]);
            }

            try
            {
                target.Remove(target[1]);
            }
            catch (NotSupportedException) { }

            Assert.AreEqual(items.Length, list.Count);
            for (int i = 0; i < items.Length; i++)
            {
                Assert.IsNotNull(items[i]);
                Assert.AreSame(items[i], list[i]);
            }

            try
            {
                target.RemoveAt(1);
            }
            catch (NotSupportedException) { }

            Assert.AreEqual(items.Length, list.Count);
            for (int i = 0; i < items.Length; i++)
            {
                Assert.IsNotNull(items[i]);
                Assert.AreSame(items[i], list[i]);
            }

            try
            {
                target[1] = new HelperObject1();
            }
            catch (NotSupportedException) { }

            Assert.AreEqual(items.Length, list.Count);
            for (int i = 0; i < items.Length; i++)
            {
                Assert.IsNotNull(items[i]);
                Assert.AreSame(items[i], list[i]);
            }
        }
    }
}
