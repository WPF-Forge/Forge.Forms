using System;
using System.Collections.Generic;
using FastMember;

namespace Forge.Forms
{
    public sealed class Snapshot
    {
        private readonly Dictionary<string, object> values;

        public Snapshot(object model, HashSet<string> properties)
        {
            SnapshotTime = DateTime.Now;
            ObjectType = model.GetType();
            values = new Dictionary<string, object>();
            var reader = ObjectAccessor.Create(model);
            foreach (var property in properties)
            {
                object value = null;
                try
                {
                    value = reader[property];
                }
                catch
                {
                    continue;
                }

                values[property] = value;
            }
        }

        public DateTime SnapshotTime { get; }

        public Type ObjectType { get; }

        public void Apply(object model)
        {
            if (model == null || model.GetType() != ObjectType)
            {
                throw new ArgumentException("Invalid model.");
            }

            var setter = ObjectAccessor.Create(model);
            foreach (var kvp in values)
            {
                try
                {
                    setter[kvp.Key] = kvp.Value;
                }
                catch
                {
                    continue;
                }
            }
        }
    }
}