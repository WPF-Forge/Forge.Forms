using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using FastMember;

namespace Forge.Forms
{
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
        ///     Gets the object.
        /// </summary>
        /// <value>
        ///     The object.
        /// </value>
        public object Object { get; }

        private List<object> CreateSnapshot(IEnumerable o, Type enumerableType)
        {
            return o.OfType<object>().Select(i => CreateSnapshot(i, enumerableType)).ToList();
        }

        private object CreateSnapshot(object o)
        {
            return CreateSnapshot(o, SnapshotType);
        }

        private object CreateSnapshot(object o, Type type)
        {
            if (o is IEnumerable enumerable)
            {
                var genericArguments = o.GetType().GetGenericArguments();

                if (genericArguments.Length <= 0)
                    throw new Exception($"Couldnt determine the type of an enumerable. {enumerable}");

                var enumerableType = genericArguments.First();
                return CreateSnapshot(enumerable, enumerableType);
            }

            var objectClone = Activator.CreateInstance(type);
            var accessor = TypeAccessor.Create(type);

            foreach (var member in accessor.GetMembers())
                try
                {
                    accessor[objectClone, member.Name] = accessor[o, member.Name];
                }
                catch
                {
                    // ignored
                }

            return objectClone;
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