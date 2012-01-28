using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace COMRegistryBrowser
{
    internal class RegistryEntry
    {
        public RegistryEntry(string guid, string name)
        {
            this.Guid = guid;
            this.Name = name;
        }


        public RegistryEntry(string guid)
            : this(guid, string.Empty)
        {
        }

        public bool Exists
        {
            get;
            protected set;
        }

        public string Guid
        {
            get;
            private set;
        }

        public string Name
        {
            get;
            protected set;
        }

        public override string ToString()
        {
            return GetType().Name + ": " + Name + " " + Guid;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return Guid.GetHashCode();
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return InternalEquals(this, obj as RegistryEntry);
        }

        protected static bool InternalEquals(RegistryEntry left, RegistryEntry right)
        {
            if (object.ReferenceEquals(left, right))
                return true;
            if (object.ReferenceEquals(left, null))
                return false;
            if (object.ReferenceEquals(right, null))
                return false;

            return string.Equals(left.Guid, right.Guid, StringComparison.OrdinalIgnoreCase);
        }
    }
}
