using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SynchronizedCollections;
using System.Collections.Generic;

namespace SynchronizedListsUnitTestProject
{
    [TestClass]
    public class NonNullSynchronizedListUnitTest
    {
        [TestMethod]
        public void ConstructorTestMethod()
        {
            NonNullSynchronizedList<HelperObject2> target = new NonNullSynchronizedList<HelperObject2>();
            HelperObject2 item = new HelperObject2(40);
            target.Add(item);
            Assert.AreEqual(1, target.Count);
            Assert.IsNotNull(target[0]);
            Assert.AreSame(item, target[0]);

            List<HelperObject2> list = new List<HelperObject2>();
            list.Add(item);
            list.Add(null);
            try
            {
                target = new NonNullSynchronizedList<HelperObject2>(list);
            }
            catch (ArgumentOutOfRangeException) { }
        }

        [TestMethod]
        public void ApplyNullTestMethod()
        {
            NonNullSynchronizedList<HelperObject2> target = new NonNullSynchronizedList<HelperObject2>();
            HelperObject2 item = new HelperObject2(40);
            target.Add(item);
            Assert.AreEqual(1, target.Count);
            Assert.IsNotNull(target[0]);
            Assert.AreSame(item, target[0]);

            try
            {
                target.Add(null);
            }
            catch (ArgumentNullException) { }

            Assert.AreEqual(1, target.Count);
            Assert.IsNotNull(target[0]);
            Assert.AreSame(item, target[0]);

            try
            {
                target.Insert(0, null);
            }
            catch (ArgumentNullException) { }

            Assert.AreEqual(1, target.Count);
            Assert.IsNotNull(target[0]);
            Assert.AreSame(item, target[0]);

            try
            {
                target[0] = null;
            }
            catch (ArgumentNullException) { }

            Assert.AreEqual(1, target.Count);
            Assert.IsNotNull(target[0]);
            Assert.AreSame(item, target[0]);
        }
    }
}
