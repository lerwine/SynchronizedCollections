using System;
using System.Collections.Generic;
using System.Linq;

namespace SynchronizedCollections
{
    /// <summary>
    /// Represents a syncrhonized (thread-safe), strongly typed list of class objects whose elements are not permitted to have null values.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    public class NonNullSynchronizedList<T> : SynchronizedList<T>
        where T : class
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronizedCollections.NonNullSynchronizedList&lt;T&gt;"/> class that is empty and has the 
        /// default initial capacity.
        /// </summary>
        /// <exception cref="System.InvalidOperationException"><see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;.CreateSynchronizedList()"/>
        /// was overridden and did not return a syncrhonzized list.</exception>
        public NonNullSynchronizedList() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronizedCollections.NonNullSynchronizedList&lt;T&gt;"/> class that contains elements copied from the 
        /// specified list and has sufficient capacity to accommodate the number of elements copied.
        /// </summary>
        /// <param name="list">The list whose elements are copied to the new list.</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="list"/> is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="list"/> contains one or more null values.</exception>
        /// <exception cref="System.InvalidOperationException"><see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;.CreateSynchronizedList()"/>
        /// was overridden and did not return a syncrhonzized list.</exception>
        public NonNullSynchronizedList(IList<T> list) : base(list) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronizedCollections.NonNullSynchronizedList&lt;T&gt;"/> class that contains elements copied from the 
        /// specified collection and has sufficient capacity to accommodate the number of elements copied.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the new list.</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="collection"/> is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="collection"/> contains one or more null values.</exception>
        /// <exception cref="System.InvalidOperationException"><see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;.CreateSynchronizedList()"/>
        /// was overridden and did not return a syncrhonzized list.</exception>
        public NonNullSynchronizedList(ICollection<T> collection) : base(collection) { }

        protected override void Initialize(ICollection<T> collection)
        {
            base.Initialize(collection);

            if (this.Any(i => i == null))
                throw new ArgumentOutOfRangeException("collection", "Source collection cannot have null values.");
        }

        #region Overrides to deny null values

        protected override int InnerAdd(object value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            return base.InnerAdd(value);
        }

        protected override void InnerInsert(int index, object value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            base.InnerInsert(index, value);
        }

        protected override void InnerSet(int index, object value)
        {
            if (value == null)
                throw new ArgumentNullException("value");

            base.InnerSet(index, value);
        }

        #endregion
    }
}
