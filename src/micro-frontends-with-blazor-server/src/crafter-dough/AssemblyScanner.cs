// See the LICENSE.TXT file in the project root for full license information.

using System.Reflection;

namespace Crafter.Dough.WebApp
{
    internal class AssemblyScanner
    {
        private readonly IEnumerable<Assembly> assemblies;

        public AssemblyScanner(IEnumerable<Assembly> assemblies, Func<Assembly, bool> scanReferencedAssembliesWhen)
        {
            var evaluated = new HashSet<string>();
            var stack = new Stack<Assembly>(assemblies);
            var set = new HashSet<Assembly>();
            do
            {
                var assembly = stack.Pop();

                set.Add(assembly);

                foreach (var reference in assembly.GetReferencedAssemblies())
                {
                    if (!evaluated.Contains(reference.FullName))
                    {
                        evaluated.Add(reference.FullName);
                        var referencedAssembly = Assembly.Load(reference);
                        if (scanReferencedAssembliesWhen(referencedAssembly))
                        {
                            stack.Push(referencedAssembly);
                        }
                    }
                }
            }
            while (stack.Any());
            this.assemblies = set;
        }

        public IEnumerable<T> Provide<T>(IServiceProvider provider, bool includeAbstract = false)
        {
            return this.assemblies.SelectMany(a => a.GetTypes()).Where(t => t.IsAssignableTo(typeof(T)) && t.IsAbstract.Equals(includeAbstract))
                .Select(t => ActivatorUtilities.CreateInstance(provider, t))
                .Cast<T>();
        }

        public IEnumerable<T> Provide<T>(bool includeAbstract = false)
        {
            return this.assemblies.SelectMany(a => a.GetTypes()).Where(t => t.IsAssignableTo(typeof(T)) && t.IsAbstract.Equals(includeAbstract))
                .Select(t => Activator.CreateInstance(t))
                .Cast<T>();
        }
    }
}
