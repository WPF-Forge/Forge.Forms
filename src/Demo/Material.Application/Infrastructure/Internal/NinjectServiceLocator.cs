using System;
using System.Collections.Generic;
using System.Linq;
using Material.Application.Helpers;
using Ninject;

namespace Material.Application.Infrastructure
{
    internal class NinjectServiceLocator : IServiceLocator
    {
        private readonly IKernel kernel;

        public NinjectServiceLocator(IKernel kernel)
        {
            this.kernel = kernel;
        }

        public T Get<T>(IDictionary<string, object> parameters) => parameters == null
            ? kernel.Get<T>()
            : kernel.Get<T>(parameters.Select(IocHelpers.MapParameter).ToArray());

        public object Get(Type type, IDictionary<string, object> parameters) => parameters == null
            ? kernel.Get(type)
            : kernel.Get(type, parameters.Select(IocHelpers.MapParameter).ToArray());
    }
}
