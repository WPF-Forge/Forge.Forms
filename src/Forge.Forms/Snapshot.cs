using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Forge.Forms
{
    /// <inheritdoc />
    /// <summary>
    ///     Generic version of snapshot
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <seealso cref="T:Forge.Forms.Snapshot" />
    public class Snapshot<T> : Snapshot
    {
        /// <summary>
        /// Gets the object.
        /// </summary>
        /// <value>
        /// The object.
        /// </value>
        public new T Object
        {
            get
            {
                var obj = ((Snapshot) this).Object;
                if (!(obj is IList objList)) return (T) obj;
                var method = typeof(Enumerable).GetMethod("OfType");
                if (method == null) return (T) obj;

                var result = (IEnumerable<object>) method.MakeGenericMethod(EnumerableType).Invoke
                    (null, new object[] {objList});

                if (!(Activator.CreateInstance(typeof(T)) is IList newList))
                    return (T)obj;

                foreach (var o in result.ToArray())
                {
                    newList.Add(o);
                }

                return (T) newList;
            }
        }

        /// <inheritdoc />
        public Snapshot(T o) : base(o)
        {
        }
    }


    /// <summary>
    ///     Stores an object in its current state
    /// </summary>
    public class Snapshot
    {
        /// <inheritdoc />
        public Snapshot(object o)
        {
            Object = o ?? throw new ArgumentNullException(nameof(o));

            if (!HasParameterlessConstructor())
                throw new Exception($"Type {SnapshotType.Name} does not contain a parameterless constructor");

            Object = CreateSnapshot(o);
        }

        /// <summary>
        ///     Gets the type of the snapshot.
        /// </summary>
        /// <value>
        ///     The type of the snapshot.
        /// </value>
        public Type SnapshotType => Object.GetType();

        /// <summary>
        /// Gets the type of the enumerable. Null if the object is not a enumerable.
        /// </summary>
        /// <value>
        /// The type of the enumerable.
        /// </value>
        public Type EnumerableType { get; private set; }

        /// <summary>
        ///     Gets the object.
        /// </summary>
        /// <value>
        ///     The object.
        /// </value>
        public object Object { get; }

        private List<object> CreateSnapshot(IEnumerable o, Type enumerableType)
        {
            return o.OfType<object>().Select(CreateSnapshot).ToList();
        }

        private object CreateSnapshot(object o)
        {
            if (o is IEnumerable enumerable)
            {
                var genericArguments = GetEnumerableGenericArguments(o);

                if (genericArguments.Length <= 0)
                    throw new Exception($"Couldnt determine the type of an enumerable. {enumerable}");

                EnumerableType = genericArguments.First();
                return CreateSnapshot(enumerable, EnumerableType);
            }

            return FastDeepCloner.DeepCloner.Clone(o);
        }

        /// <summary>
        /// Gets the enumerable generic arguments.
        /// </summary>
        /// <param name="o">The o.</param>
        /// <returns></returns>
        public static Type[] GetEnumerableGenericArguments(object o)
        {
            return o.GetType().GetGenericArguments();
        }

        /// <summary>
        ///     Determines whether [has parameterless constructor].
        /// </summary>
        /// <returns>
        ///     <c>true</c> if [has parameterless constructor]; otherwise, <c>false</c>.
        /// </returns>
        private bool HasParameterlessConstructor()
        {
            return SnapshotType.GetConstructor(Type.EmptyTypes) != null;
        }
    }
}