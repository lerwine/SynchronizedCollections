using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace SynchronizedListsUnitTestProject
{
    public class HelperEnumerator : IEnumerator<int>
    {
        private int[] _values;
        private int _index = -1;
        public event EventHandler Disposing;

        public HelperEnumerator(params int[] values)
        {
            this._values = values;
        }

        #region IEnumerator<int> Members

        public int Current
        {
            get { throw new AssertFailedException("Explicit IEnumerator Current property was not used."); }
        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (this.Disposing != null)
                    this.Disposing(this, EventArgs.Empty);
            }
        }

        #endregion

        #region IEnumerator Members

        object System.Collections.IEnumerator.Current
        {
            get
            {
                if (this._index == -1)
                    throw new InvalidOperationException("The enumeration has not started.");

                if (this._index == this._values.Length)
                    throw new InvalidOperationException("The enumeration has finished.");

                return this._values[this._index];
            }
        }

        public bool MoveNext()
        {
            if (this._index == this._values.Length)
                return false;

            this._index++;

            return (this._index != this._values.Length);
        }

        public void Reset()
        {
            this._index = -1;
        }

        #endregion
    }
}
