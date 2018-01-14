using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Ninject;

namespace Forge.Forms.Utils
{
    public class Transformation
    {
        public static Transformation GlobalTransformation { get; set; } = new Transformation();

        private static Dictionary<Type, Transformation> Transformations { get; } =
            new Dictionary<Type, Transformation>();

        public Func<object, string, object, object> OnAction { get; set; } = (o, s, arg3) => o;

        public Func<object, object, IKernel, object> ModelChanged { get; set; } =
            (oldModel, newModel, kernel) => newModel;

        public Func<object, IKernel, object> KernelChanged { get; set; } = (o, k) => o;

        public Func<Type, IEnumerable<PropertyInfo>> GetProperties { get; set; } = o => o
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanRead && p.GetGetMethod(true).IsPublic)
            .OrderBy(p => p.MetadataToken);

        public static void AddTransformation(Type type, Transformation transformation)
        {
            Transformations.Add(type, transformation);
        }

        public static Transformation GetTransformation(object model)
        {
            return model != null ? GetTransformation(model.GetType()) : null;
        }

        public static Transformation GetTransformation(Type type)
        {
            return Transformations.ContainsKey(type) ? Transformations[type] : GlobalTransformation;
        }
    }
}
