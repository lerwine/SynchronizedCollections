using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace SynchronizedListsUnitTestProject
{
    public class HelperList : IEnumerable<int>
    {
        private int[] _values;

        public bool EnumeratorWasDisposed { get; set; }

        public HelperList(params int[] values)
        {
            this._values = values;
        }

        #region IEnumerable<int> Members

        public IEnumerator<int> GetEnumerator()
        {
            throw new AssertFailedException("Explicit IEnumerator GetEnumerator() method was not invoked.");
        }

        void result_Disposing(object sender, EventArgs e)
        {
            (sender as HelperEnumerator).Disposing -= this.result_Disposing;

            this.EnumeratorWasDisposed = true;
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            HelperEnumerator result = new HelperEnumerator(this._values);
            result.Disposing += this.result_Disposing;
            return result;
        }

        #endregion
    }
}
