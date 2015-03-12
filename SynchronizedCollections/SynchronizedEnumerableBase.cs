using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace SynchronizedCollections
{
    /// <summary>
    /// Base class to provide syncronized (thread-safe) collections.
    /// </summary>
    /// <typeparam name="T">The type of elements in the collection.</typeparam>
    public abstract class SynchronizedEnumerableBase<T> : IEnumerable<T>, IList
    {
        private IList _innerSynchronizedList;

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/> class that is empty and has the 
        /// default initial capacity.
        /// </summary>
        /// <exception cref="System.InvalidOperationException"><see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;.CreateSynchronizedList()"/>
        /// was overridden and did not return a syncrhonzized list.</exception>
        protected SynchronizedEnumerableBase() : this(new List<T>()) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/> class that contains elements copied from the 
        /// specified list and has sufficient capacity to accommodate the number of elements copied.
        /// </summary>
        /// <param name="list">The list whose elements are copied to the new list.</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="list"/> is null.</exception>
        /// <exception cref="System.InvalidOperationException"><see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;.CreateSynchronizedList()"/>
        /// was overridden and did not return a syncrhonzized list.</exception>
        protected SynchronizedEnumerableBase(IList<T> list)
        {
            if (list == null)
                throw new ArgumentNullException("list");

            this.Initialize(list);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/> class that contains elements copied from the 
        /// specified collection and has sufficient capacity to accommodate the number of elements copied.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the new list.</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="collection"/> is null.</exception>
        /// <exception cref="System.InvalidOperationException"><see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;.CreateSynchronizedList()"/>
        /// was overridden and did not return a syncrhonzized list.</exception>
        protected SynchronizedEnumerableBase(ICollection<T> collection)
        {
            this.Initialize(collection);
        }

        /// <summary>
        /// Called from within the base constructor to initialize the inner synchronized list.
        /// </summary>
        /// <param name="collection">The collection whose elements are copied to the new list.</param>
        /// <exception cref="System.InvalidOperationException"><see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;.CreateSynchronizedList()"/>
        /// was overridden and did not return a syncrhonzized list.</exception>
        protected virtual void Initialize(ICollection<T> collection)
        {
            if ((this._innerSynchronizedList = this.CreateSynchronizedList(collection)) == null)
                this._innerSynchronizedList = this._CreateSynchronizedList(collection);
            else if (!this._innerSynchronizedList.IsSynchronized)
                throw new InvalidOperationException("CreateSynchronizedList() did not return a synchronized list.");
        }

        /// <summary>
        /// Called from within the <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;.CreateSynchronizedList()"/> method to create a 
        /// syncrhonized <see cref="System.Collections.IList"/>.
        /// </summary>
        /// <param name="collection">The collection from which the new synchronized <see cref="System.Collections.IList"/> is to be created.</param>
        /// <returns>A new synchronized <see cref="System.Collections.IList"/>.</returns>
        protected virtual IList CreateSynchronizedList(ICollection<T> collection)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");

            return this._CreateSynchronizedList(collection);
        }

        private IList _CreateSynchronizedList(ICollection<T> collection)
        {
            if (collection is ArrayList)
                return ArrayList.Synchronized(collection as ArrayList);

            return ArrayList.Synchronized((collection is IList) ? collection as IList : collection.ToList());
        }

        #region IList<T> Support Members

        /// <summary>
        /// Determines the index of a specific item in the <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/>.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/>.</param>
        /// <returns>The zero-based index of <paramref name="item"/> if found in the list; otherwise, -1.</returns>
        public int IndexOf(T item) { return this.InnerIndexOf(item); }

        /// <summary>
        /// Gets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get.</param>
        /// <returns>The element at the specified index.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in
        /// the <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/>.</exception>
        public T this[int index] { get { return (T)(this.InnerGet(index)); } }

        /// <summary>
        /// Inserts an item to the <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which <paramref name="item"/> should be inserted.</param>
        /// <param name="item">The object to insert into the <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/>.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in
        /// the <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/>.</exception>
        /// <exception cref="System.NotSupportedException">The <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/> is read-only
        /// <para>-or-</para>
        /// <para>The <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/> has a fixed size.</para></exception>
        /// <exception cref="System.NullReferenceException"><paramref name="item"/> is null reference in 
        /// the <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/>.</exception>
        protected void BaseInsert(int index, T item) { this.InnerInsert(index, item); }
        
        /// <summary>
        /// Removes the S<see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/> item at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the item to remove.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in
        /// the <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/>.</exception>
        /// <exception cref="System.NotSupportedException">The <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/> is read-only.</exception>
        protected virtual void BaseRemoveAt(int index) { this._innerSynchronizedList.RemoveAt(index); }

        /// <summary>
        /// Sets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to set.</param>
        /// <param name="item">The object to set at the specified index in the <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/>.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in
        /// the <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/>.</exception>
        /// <exception cref="System.NotSupportedException">The property is set and the <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/>
        /// is read-only.</exception>
        protected void BaseSet(int index, T item) { this.InnerSet(index, item); }

        #endregion

        #region ICollection<T> Support Members

        /// <summary>
        /// Determines whether the <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/> contains a specific value.
        /// </summary>
        /// <param name="item">The object to locate in the <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/>.</param>
        /// <returns>true if <paramref name="item"/> is found in the <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/>; otherwise, false.</returns>
        public bool Contains(T item) { return this.InnerContains(item); }

        /// <summary>
        /// Copies the elements of the <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/> to an <see cref="System.Array"/>,
        /// starting at a particular <see cref="System.Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="System.Array"/> that is the destination of the elements copied 
        /// from <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/>. The <see cref="System.Array"/> must have zero-based indexing.</param>
        /// <param name="arrayIndex">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="array"/> is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="arrayIndex"/> is less than 0.</exception>
        /// <exception cref="System.ArgumentException">The number of elements in the source <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/>
        /// is greater than the available space from <paramref name="arrayIndex"/> to the end of the destination array.</exception>
        public void CopyTo(T[] array, int arrayIndex) { this.InnerCopyTo(array, arrayIndex); }

        /// <summary>
        /// Gets the number of elements contained in the <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/>.
        /// </summary>
        public int Count { get { return this._innerSynchronizedList.Count; } }

        /// <summary>
        /// Adds an item to the <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/>.
        /// </summary>
        /// <param name="item">The object to add to the <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/>.</param>
        /// <exception cref="System.NotSupportedException">The <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/> is read-only.
        /// <para>-or-</para>
        /// <para>The <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/> has a fixed size.</para></exception>
        protected void BaseAdd(T item) { this.InnerAdd(item); }

        /// <summary>
        /// Removes all items from the <see cref="SynchronizedCollections.SyncrhonizedEnumerableBase&lt;T&gt;"/>.
        /// </summary>
        /// <exception cref="System.NotSupportedException">The <see cref="SynchronizedCollections.SyncrhonizedEnumerableBase&lt;T&gt;"/> is read-only.
        /// <para>-or-</para>
        /// <para>The <see cref="SynchronizedCollections.SyncrhonizedEnumerableBase&lt;T&gt;"/> has a fixed size.</exception>
        protected virtual void BaseClear() { this._innerSynchronizedList.Clear(); }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/>.
        /// </summary>
        /// <param name="item">The object to remove from the <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/>.</param>
        /// <returns>true if item was successfully removed from the <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/>;
        /// otherwise, false. This method also returns false if item is not found in the 
        /// original <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/>.</returns>
        /// <exception cref="System.NotSupportedException">The <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/> is read-only
        /// <para>-or-</para>
        /// <para>The <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/> has a fixed size.</para></exception>
        protected virtual bool BaseRemove(T item) { return this.InnerRemove(item); }

        #endregion

        #region IList Members

        /// <summary>
        /// Gets a value indicating whether the <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/> has a fixed size.
        /// </summary>
        public bool IsFixedSize { get { return this._innerSynchronizedList.IsFixedSize; } }

        /// <summary>
        /// Gets a value indicating whether the <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/> is read-only.
        /// </summary>
        public bool IsReadOnly { get { return this._innerSynchronizedList.IsReadOnly; } }

        /// <summary>
        /// Determines whether the <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/> contains a specific value.
        /// </summary>
        /// <param name="value">The object to locate in the <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/>.</param>
        /// <returns>true if the <see cref="System.Object"/> is found in the <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/>; 
        /// otherwise, false.</returns>
        protected bool InnerContains(object value) { return this._innerSynchronizedList.Contains(value); }

        /// <summary>
        /// Determines the index of a specific item in the <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/>.
        /// </summary>
        /// <param name="value">The object to locate in the <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/>.</param>
        /// <returns>The index of value if found in the list; otherwise, -1.</returns>
        protected int InnerIndexOf(object value) { return this._innerSynchronizedList.IndexOf(value); }

        /// <summary>
        /// Gets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to get.</param>
        /// <returns>The element at the specified index.</returns>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in
        /// the <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/>.</exception>
        protected object InnerGet(int index) { return this._innerSynchronizedList[index]; }

        /// <summary>
        /// Adds an item to the <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/>.
        /// </summary>
        /// <param name="value">The object to add to the <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/>.</param>
        /// <returns>The position into which the new element was inserted, or -1 to indicate that the item was not inserted into the collection.</returns>
        /// <exception cref="System.NotSupportedException">The <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/> is read-only
        /// <para>-or-</para>
        /// <para>The <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/> has a fixed size.</para></exception>
        /// <exception cref="InvalidCastException"><paramref name="value"/> could not be cast to <typeparamref name="T"/>.</exception>
        protected virtual int InnerAdd(object value) { return this._innerSynchronizedList.Add((T)value); }

        /// <summary>
        /// Inserts an item to the <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which value should be inserted.</param>
        /// <param name="value">The object to insert into the <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/>.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in 
        /// the <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/>.</exception>
        /// <exception cref="System.NotSupportedException">The <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/> is read-only
        /// <para>-or-</para>
        /// <para>The <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/> has a fixed size.</para></exception>
        /// <exception cref="System.NullReferenceException"><paramref name="value"/> is null reference in 
        /// the <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/>.</exception>
        /// <exception cref="InvalidCastException"><paramref name="value"/> could not be cast to <typeparamref name="T"/>.</exception>
        protected virtual void InnerInsert(int index, object value) { this._innerSynchronizedList.Insert(index, (T)value); }

        /// <summary>
        /// Removes the first occurrence of a specific object from the <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/>.
        /// </summary>
        /// <param name="value">The object to remove from the <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/>.</param>
        /// <returns>true if item is successfully removed; otherwise, false. This method also returns false if item was not found in 
        /// the <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/>.</returns>
        /// <exception cref="System.NotSupportedException">The <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/> is read-only
        /// <para>-or-</para>
        /// <para>The <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/> has a fixed size.</para></exception>
        protected virtual bool InnerRemove(object value)
        {
            if (!this._innerSynchronizedList.Contains(value))
                return false;

            this._innerSynchronizedList.Remove(value);

            return true;
        }

        /// <summary>
        /// Sets the element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the element to set.</param>
        /// <param name="value">The object to set at the specified index in the <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/>.</param>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="index"/> is not a valid index in
        /// the <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/>.</exception>
        /// <exception cref="System.NotSupportedException">The property is set and the <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/>
        /// is read-only.</exception>
        /// <exception cref="InvalidCastException"><paramref name="value"/> could not be cast to <typeparamref name="T"/>.</exception>
        protected virtual void InnerSet(int index, object value) { this._innerSynchronizedList[index] = (T)value; }

        #region Explicit Members

        bool IList.Contains(object value) { return this.InnerContains(value); }

        int IList.IndexOf(object value) { return this.InnerIndexOf(value); }

        object IList.this[int index]
        {
            get { return this.InnerGet(index); }
            set { this.InnerSet(index, value); }
        }

        int IList.Add(object value) { return this.InnerAdd(value); }

        void IList.Clear() { this.BaseClear(); }

        void IList.Insert(int index, object value) { this.InnerInsert(index, value); }

        void IList.Remove(object value) { this.InnerRemove(value); }

        void IList.RemoveAt(int index) { this.BaseRemoveAt(index); }

        #endregion

        #endregion

        #region ICollection Members

        /// <summary>
        /// This always returns true because the <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/> is synchronized (thread safe).
        /// </summary>
        public bool IsSynchronized { get { return this._innerSynchronizedList.IsSynchronized; } }

        /// <summary>
        /// Gets an object that can be used to synchronize access to the <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/>.
        /// </summary>
        public object SyncRoot { get { return this._innerSynchronizedList.SyncRoot; } }

        /// <summary>
        /// Copies the elements of the <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/> to an <see cref="System.Array"/>, 
        /// starting at a particular <see cref="System.Array"/> index.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="System.Array"/> that is the destination of the elements copied 
        /// from <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/>. The <see cref="System.Array"/> must have zero-based indexing.</param>
        /// <param name="index">The zero-based index in <paramref name="array"/> at which copying begins.</param>
        /// <exception cref="System.ArgumentNullException"><paramref name="array"/> is null.</exception>
        /// <exception cref="System.ArgumentOutOfRangeException"><paramref name="index"/> is less than zero.</exception>
        /// <exception cref="System.ArgumentException"><paramref name="array"/> is multidimensional.
        /// <para>-or-</para>
        /// <para>The number of elements in the source <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/> is greater than the available space 
        /// from <paramref name="index"/> to the end of the destination array.</para>
        /// <para>-or-</para>
        /// <para>The type of the source <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/> cannot be cast automatically to the type of the 
        /// destination array.</para></exception>
        protected void InnerCopyTo(Array array, int index) { this._innerSynchronizedList.CopyTo(array, index); }

        void ICollection.CopyTo(Array array, int index) { this.InnerCopyTo(array, index); }

        #endregion

        #region IEnumerable Members

        /// <summary>
        /// Returns an enumerator that iterates through the <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/>.
        /// </summary>
        /// <returns>A <see cref="System.Collections.Generic.IEnumerator&lt;T&gt;"/> that can be used to iterate through 
        /// the <see cref="SynchronizedCollections.SynchronizedEnumerableBase&lt;T&gt;"/>.</returns>
        public IEnumerator<T> GetEnumerator() { return new TypedEnumeratorWrapper<T>(this._innerSynchronizedList); }

        IEnumerator IEnumerable.GetEnumerator() { return this.InnerGetEnumerator(); }

        /// <summary>
        /// Returns an enumerator that iterates through the inner collection.
        /// </summary>
        /// <returns>An <see cref="System.Collections.IEnumerator"/> object that can be used to iterate through the collection.</returns>
        protected IEnumerator InnerGetEnumerator() { return this._innerSynchronizedList.GetEnumerator(); }

        #endregion
    }
}
