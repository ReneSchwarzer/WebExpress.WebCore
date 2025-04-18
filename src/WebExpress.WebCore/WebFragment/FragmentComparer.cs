using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace WebExpress.WebCore.WebFragment
{
    /// <summary>
    /// Provides a method to compare two objects of type T for equality.
    /// </summary>
    /// <typeparam name="T">The type of objects to compare.</typeparam>
    [Obsolete("FragmentComparer is obsolete. Use a different comparer implementation.")]
    public class FragmentComparer<T> : IEqualityComparer<T>
    {
        /// <summary>
        /// Tests for equality.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>True if both objects are similar; false otherwise.</returns>
        public bool Equals(T x, T y)
        {
            if (x == null && y == null)
            {
                return true;
            }
            else if (x.GetType() == y.GetType())
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Returns a hash code for the specified object.
        /// </summary>
        /// <param name="obj">The object for which to return a hash code.</param>
        /// <returns>The hash code.</returns>
        public int GetHashCode([DisallowNull] T obj)
        {
            return obj.GetType().GetHashCode();
        }
    }
}
