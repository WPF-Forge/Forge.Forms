using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Forge.Forms
{
    /// <summary>
    /// Static transformation actions
    /// </summary>
    public class TransformationBase
    {
        static TransformationBase()
        {
            var moduleInit = AppDomain.CurrentDomain.GetAssemblies().SelectMany(i => i.GetReferencedAssemblies())
                .Select(Assembly.Load).SelectMany(assembly => assembly.GetTypes())
                .Where(i => i.Namespace == "Forge.Forms.Mapper")
                .FirstOrDefault(i => i.Name == "ModuleInitializer");

            moduleInit?.GetMethod("Initialize")?.Invoke(null, null);
        }

        /// <summary>
        /// Gets the default.
        /// </summary>
        /// <value>
        /// The default fall back value.
        /// </value>
        public static Transformation Default { get; } = new Transformation();

        private static Dictionary<Type, Transformation> Transformations { get; } =
            new Dictionary<Type, Transformation>();

        /// <summary>
        /// Adds a transformation.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="transformation">The transformation.</param>
        public static void AddTransformation(Type type, Transformation transformation)
        {
            Transformations.Add(type, transformation);
        }

        /// <summary>
        /// Gets a transformation from an object.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public static Transformation GetTransformation(object model)
        {
            return model != null ? GetTransformation(model.GetType()) : null;
        }

        /// <summary>
        /// Gets a transformation from a type.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static Transformation GetTransformation(Type type)
        {
            return Transformations.ContainsKey(type) ? Transformations[type] : Default;
        }
    }

    /// <inheritdoc />
    /// <summary>
    /// Represents a transformation
    /// </summary>
    /// <seealso cref="T:Forge.Forms.TransformationBase" />
    public class Transformation : TransformationBase
    {
        /// <summary>
        /// Happens when a non-command action happens.
        /// </summary>
        public Func<object, string, object, object> OnAction { get; set; } = (o, s, arg3) => o;

        /// <summary>
        /// Happens whenever a model update is called.
        /// Expects the new model as return.
        /// </summary>
        public Func<object, object, object> ModelChanged { get; set; } =
            (oldModel, newModel) => newModel;

        /// <summary>
        /// Used to get properties during the form building process.
        /// </summary>
        public Func<Type, IEnumerable<PropertyInfo>> GetProperties { get; set; } = o => o
            .GetProperties(BindingFlags.Public | BindingFlags.Instance)
            .Where(p => p.CanRead && p.GetGetMethod(true).IsPublic)
            .OrderBy(p => p.MetadataToken);
    }
}
