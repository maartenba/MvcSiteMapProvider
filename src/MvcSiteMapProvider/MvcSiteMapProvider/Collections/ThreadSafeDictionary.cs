using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace MvcSiteMapProvider.Collections
{
    /// <summary>
    /// Thread-safe dictionary implementation.
    /// </summary>
    /// <typeparam name="TKey">The type of the key.</typeparam>
    /// <typeparam name="TValue">The type of the value.</typeparam>
    [Serializable]
    public class ThreadSafeDictionary<TKey, TValue> : IThreadSafeDictionary<TKey, TValue>
    {
        // This is the internal dictionary that we are wrapping
        readonly IDictionary<TKey, TValue> dict = new Dictionary<TKey, TValue>();

        [NonSerialized]
        readonly ReaderWriterLockSlim dictionaryLock = Locks.GetLockInstance(LockRecursionPolicy.NoRecursion); //setup the lock;

        /// <summary>
        /// This is a blind remove. Prevents the need to check for existence first.
        /// </summary>
        /// <param name="key">Key to remove</param>
        public void RemoveSafe(TKey key)
        {
            using (new ReadLock(dictionaryLock))
            {
                if (dict.ContainsKey(key))
                {
                    using (new WriteLock(dictionaryLock))
                    {
                        dict.Remove(key);
                    }
                }
            }
        }

        /// <summary>
        /// Merge does a blind remove, and then add.  Basically a blind Upsert.  
        /// </summary>
        /// <param name="key">Key to lookup</param>
        /// <param name="newValue">New Value</param>
        public void MergeSafe(TKey key, TValue newValue)
        {
            using (new WriteLock(dictionaryLock)) // take a writelock immediately since we will always be writing
            {
                if (dict.ContainsKey(key))
                {
                    dict.Remove(key);
                }

                dict.Add(key, newValue);
            }
        }

        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public virtual bool Remove(TKey key)
        {
            using (new WriteLock(dictionaryLock))
            {
                return dict.Remove(key);
            }
        }

        /// <summary>
        /// Determines whether the dictionary contains the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns>
        /// 	<c>true</c> if the dictionary contains the specified key; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool ContainsKey(TKey key)
        {
            using (new ReadOnlyLock(dictionaryLock))
            {
                return dict.ContainsKey(key);
            }
        }

        /// <summary>
        /// Tries to get value.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        public virtual bool TryGetValue(TKey key, out TValue value)
        {
            using (new ReadOnlyLock(dictionaryLock))
            {
                return dict.TryGetValue(key, out value);
            }
        }

        /// <summary>
        /// Gets or sets the value with the specified key.
        /// </summary>
        /// <value>The value</value>
        public virtual TValue this[TKey key]
        {
            get
            {
                using (new ReadOnlyLock(dictionaryLock))
                {
                    return dict[key];
                }
            }
            set
            {
                using (new WriteLock(dictionaryLock))
                {
                    dict[key] = value;
                }
            }
        }

        /// <summary>
        /// Gets the keys.
        /// </summary>
        /// <value>The keys.</value>
        public virtual ICollection<TKey> Keys
        {
            get
            {
                using (new ReadOnlyLock(dictionaryLock))
                {
                    return new List<TKey>(dict.Keys);
                }
            }
        }

        /// <summary>
        /// Gets the values.
        /// </summary>
        /// <value>The values.</value>
        public virtual ICollection<TValue> Values
        {
            get
            {
                using (new ReadOnlyLock(dictionaryLock))
                {
                    return new List<TValue>(dict.Values);
                }
            }
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public virtual void Clear()
        {
            using (new WriteLock(dictionaryLock))
            {
                dict.Clear();
            }
        }

        /// <summary>
        /// Gets the count.
        /// </summary>
        /// <value>The count.</value>
        public virtual int Count
        {
            get
            {
                using (new ReadOnlyLock(dictionaryLock))
                {
                    return dict.Count;
                }
            }
        }

        /// <summary>
        /// Determines whether [contains] [the specified item].
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns>
        /// 	<c>true</c> if [contains] [the specified item]; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool Contains(KeyValuePair<TKey, TValue> item)
        {
            using (new ReadOnlyLock(dictionaryLock))
            {
                return dict.Contains(item);
            }
        }

        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        public virtual void Add(KeyValuePair<TKey, TValue> item)
        {
            if (!dict.ContainsKey(item.Key))
            {
                using (new WriteLock(dictionaryLock))
                {
                    if (!dict.ContainsKey(item.Key))
                    {
                        dict.Add(item);
                    }
                }
            }
        }

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public virtual void Add(TKey key, TValue value)
        {
            if (!dict.ContainsKey(key))
            {
                using (new WriteLock(dictionaryLock))
                {
                    if (!dict.ContainsKey(key))
                    {
                        dict.Add(key, value);
                    }
                }
            }
        }

        /// <summary>
        /// Removes the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <returns></returns>
        public virtual bool Remove(KeyValuePair<TKey, TValue> item)
        {
            using (new WriteLock(dictionaryLock))
            {
                return dict.Remove(item);
            }
        }

        /// <summary>
        /// Copies to.
        /// </summary>
        /// <param name="array">The array.</param>
        /// <param name="arrayIndex">Index of the array.</param>
        public virtual void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            using (new ReadOnlyLock(dictionaryLock))
            {
                dict.CopyTo(array, arrayIndex);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is read only.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance is read only; otherwise, <c>false</c>.
        /// </value>
        public virtual bool IsReadOnly
        {
            get
            {
                using (new ReadOnlyLock(dictionaryLock))
                {
                    return dict.IsReadOnly;
                }
            }
        }

        /// <summary>
        /// Gets the enumerator.
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            throw new NotSupportedException(Resources.Messages.CannotEnumerateThreadSafeDictionary);
        }


        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        /// <returns>
        /// An <see cref="T:System.Collections.IEnumerator"/> object that can be used to iterate through the collection.
        /// </returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotSupportedException(Resources.Messages.CannotEnumerateThreadSafeDictionary);
        }
    }

    /// <summary>
    /// Locks class
    /// </summary>
    public static class Locks
    {
        /// <summary>
        /// Gets the read lock.
        /// </summary>
        /// <param name="locks">The locks.</param>
        public static void GetReadLock(ReaderWriterLockSlim locks)
        {
            bool lockAcquired = false;
            while (!lockAcquired)
                lockAcquired = locks.TryEnterUpgradeableReadLock(1);
        }

        /// <summary>
        /// Gets the read only lock.
        /// </summary>
        /// <param name="locks">The locks.</param>
        public static void GetReadOnlyLock(ReaderWriterLockSlim locks)
        {
            bool lockAcquired = false;
            while (!lockAcquired)
                lockAcquired = locks.TryEnterReadLock(1);
        }

        /// <summary>
        /// Gets the write lock.
        /// </summary>
        /// <param name="locks">The locks.</param>
        public static void GetWriteLock(ReaderWriterLockSlim locks)
        {
            bool lockAcquired = false;
            while (!lockAcquired)
                lockAcquired = locks.TryEnterWriteLock(1);
        }

        /// <summary>
        /// Releases the read only lock.
        /// </summary>
        /// <param name="locks">The locks.</param>
        public static void ReleaseReadOnlyLock(ReaderWriterLockSlim locks)
        {
            if (locks.IsReadLockHeld)
                locks.ExitReadLock();
        }

        /// <summary>
        /// Releases the read lock.
        /// </summary>
        /// <param name="locks">The locks.</param>
        public static void ReleaseReadLock(ReaderWriterLockSlim locks)
        {
            if (locks.IsUpgradeableReadLockHeld)
                locks.ExitUpgradeableReadLock();
        }

        /// <summary>
        /// Releases the write lock.
        /// </summary>
        /// <param name="locks">The locks.</param>
        public static void ReleaseWriteLock(ReaderWriterLockSlim locks)
        {
            if (locks.IsWriteLockHeld)
                locks.ExitWriteLock();
        }

        /// <summary>
        /// Releases the lock.
        /// </summary>
        /// <param name="locks">The locks.</param>
        public static void ReleaseLock(ReaderWriterLockSlim locks)
        {
            ReleaseWriteLock(locks);
            ReleaseReadLock(locks);
            ReleaseReadOnlyLock(locks);
        }

        /// <summary>
        /// Gets the lock instance.
        /// </summary>
        /// <returns></returns>
        public static ReaderWriterLockSlim GetLockInstance()
        {
            return GetLockInstance(LockRecursionPolicy.SupportsRecursion);
        }

        /// <summary>
        /// Gets the lock instance.
        /// </summary>
        /// <param name="recursionPolicy">The recursion policy.</param>
        /// <returns></returns>
        public static ReaderWriterLockSlim GetLockInstance(LockRecursionPolicy recursionPolicy)
        {
            return new ReaderWriterLockSlim(recursionPolicy);
        }
    }


    /// <summary>
    /// Base lock
    /// </summary>
    public abstract class BaseLock : IDisposable
    {
        /// <summary>
        /// Locks
        /// </summary>
        protected ReaderWriterLockSlim _Locks;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseLock"/> class.
        /// </summary>
        /// <param name="locks">The locks.</param>
        public BaseLock(ReaderWriterLockSlim locks)
        {
            _Locks = locks;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public abstract void Dispose();
    }


    /// <summary>
    /// Read lock
    /// </summary>
    public class ReadLock : BaseLock
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadLock"/> class.
        /// </summary>
        /// <param name="locks">The locks.</param>
        public ReadLock(ReaderWriterLockSlim locks)
            : base(locks)
        {
            Locks.GetReadLock(_Locks);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            Locks.ReleaseReadLock(_Locks);
        }
    }

    /// <summary>
    /// Readonly lock
    /// </summary>
    public class ReadOnlyLock : BaseLock
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyLock"/> class.
        /// </summary>
        /// <param name="locks">The locks.</param>
        public ReadOnlyLock(ReaderWriterLockSlim locks)
            : base(locks)
        {
            Locks.GetReadOnlyLock(_Locks);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            Locks.ReleaseReadOnlyLock(_Locks);
        }
    }

    /// <summary>
    /// Write lock
    /// </summary>
    public class WriteLock : BaseLock
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WriteLock"/> class.
        /// </summary>
        /// <param name="locks">The locks.</param>
        public WriteLock(ReaderWriterLockSlim locks)
            : base(locks)
        {
            Locks.GetWriteLock(_Locks);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            Locks.ReleaseWriteLock(_Locks);
        }
    }
}