using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SynchronizedCollections
{
    /// <summary>
    /// Represents a syncrhonized (thread-safe), strongly typed list of objects that can be accessed by index.
    /// </summary>
    /// <typeparam name="T">The type of elements in the list.</typeparam>
    public class SynchronizedList<T> : SynchronizedEnumerableBase<T>, IList<T>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronizedCollections.SynchronizedList&lt;T&gt;"/> class that is empty and has the 
        /// default initial capacity.
        /// </summary>
        /// <exception cref="System.InvalidOperationException"><see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;.CreateSynchronizedList()"/>
        /// was overridden and did not return a syncrhonzized list.</exception>
        public SynchronizedList() : base() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronizedCollections.SynchronizedList&lt;T&gt;"/> class that contains elements copied from the 
        /// specified list and has sufficient capacity to accommodate the number of elements copied.
        /// </summary>
        /// <param name="list">The list whose elements are copied to the new list.</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="list"/> is null.</exception>
        /// <exception cref="System.InvalidOperationException"><see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;.CreateSynchronizedList()"/>
        /// was overridden and did not return a syncrhonzized list.</exception>
        public SynchronizedList(IList<T> list) : base(list) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/> class that contains elements copied from the 
        /// specified collection and has sufficient capacity to accommodate the number of elements copied.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the new list.</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="collection"/> is null.</exception>
        /// <exception cref="System.InvalidOperationException"><see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;.CreateSynchronizedList()"/>
        /// was overridden and did not return a syncrhonzized list.</exception>
        public SynchronizedList(ICollection<T> collection) : base(collection) { }

        #region IList<T> Members

        /// <summary>
        /// Inserts an item to the <see cref="SynchronizedCollections.SynchronizedList&lt;T&gt;"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
        /// <param name="item">The object to insert into the <see cref="SynchronizedCollections.SynchronizedList&lt;T&gt;"/>.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in
        /// the <see cref="SynchronizedCollections.SynchronizedList&lt;T&gt;"/>.</exception>
        /// <exception cref="System.NotSupportedException">The <see cref="SynchronizedCollections.SynchronizedList&lt;T&gt;"/> is read-only
        /// <para>-or-</para>
        /// <para>The <see cref="SynchronizedCollections.SynchronizedList&lt;T&gt;"/> has a fixed size.</para></exception>
        /// <exception cref="System.NullReferenceException"><paramref name="item"/> is null reference in 
        /// the <see cref="SynchronizedCollections.SynchronizedList&lt;T&gt;"/>.</exception>
        public void Insert(int index, T item) { base.BaseInsert(index, item); }

        /// <summary>
        /// Removes the S<see cref="SynchronizedCollections.SynchronizedList&lt;T&gt;"/> item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in
        /// the <see cref="SynchronizedCollections.SynchronizedList&lt;T&gt;"/>.</exception>
        /// <exception cref="System.NotSupportedException">The <see cref="SynchronizedCollections.SynchronizedList&lt;T&gt;"/> is read-only.</exception>
        public void RemoveAt(int index) { base.BaseRemoveAt(index); }

        /// <summary>
        /// Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get or set.</param>
        /// <returns>The element at the specified index.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in
        /// the <see cref="SynchronizedCollections.SynchronizedList&lt;T&gt;"/>.</exception>
        /// <exception cref="System.NotSupportedException">The property is set and the <see cref="SynchronizedCollections.SynchronizedList&lt;T&gt;"/>
        /// is read-only.</exception>
        public new T this[int index]
        {
            get { return base[index]; }
            set { base.BaseSet(index, value); }
        }

        #endregion

        #region ICollection<T> Members

        /// <summary>
        /// Adds an item to the <see cref="SynchronizedCollections.SynchronizedList&lt;T&gt;"/>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="SynchronizedCollections.SynchronizedList&lt;T&gt;"/>.</param>
        /// <exception cref="System.NotSupportedException">The <see cref="SynchronizedCollections.SynchronizedList&lt;T&gt;"/> is read-only
        /// <para>-or-</para>
        /// <para>The <see cref="SynchronizedCollections.SynchronizedList&lt;T&gt;"/> has a fixed size.</para></exception>
        public void Add(T item) { base.BaseAdd(item); }

        /// <summary>
        /// Removes all items from the <see cref="SynchronizedCollections.SynchronizedList&lt;T&gt;"/>.
        /// </summary>
        /// <exception cref="System.NotSupportedException">The <see cref="SynchronizedCollections.SynchronizedList&lt;T&gt;"/> is read-only.
        /// <para>-or-</para>
        /// <para>The <see cref="SynchronizedCollections.SynchronizedList&lt;T&gt;"/> has a fixed size.</exception>
        public void Clear() { base.BaseClear(); }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="SynchronizedCollections.SynchronizedList&lt;T&gt;"/>.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="SynchronizedCollections.SynchronizedList&lt;T&gt;"/>.</param>
        /// <returns>true if item was successfully removed from the <see cref="SynchronizedCollections.SynchronizedList&lt;T&gt;"/>;
        /// otherwise, false. This method also returns false if item is not found in the 
        /// original <see cref="SynchronizedCollections.SynchronizedList&lt;T&gt;"/>.</returns>
        /// <exception cref="System.NotSupportedException">The <see cref="SynchronizedCollections.SynchronizedList&lt;T&gt;"/> is read-only
        /// <para>-or-</para>
        /// <para>The <see cref="SynchronizedCollections.SynchronizedList&lt;T&gt;"/> has a fixed size.</para></exception>
        public bool Remove(T item) { return base.BaseRemove(item); }

        #endregion

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator() { return base.InnerGetEnumerator(); }

        #endregion
    }
}
