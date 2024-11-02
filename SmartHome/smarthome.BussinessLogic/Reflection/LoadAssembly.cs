using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace smarthome.BussinessLogic
{
    [ExcludeFromCodeCoverage]
    public sealed class LoadAssembly<IInterface>() : ILoadAssembly<IInterface>
        where IInterface : class
    {
        private List<Type> implementations = [];

        public List<string> GetImplementations(string path)
        {
            var files = new DirectoryInfo(path)
                .GetFiles("*.dll")
                .ToList();

            implementations = [];
            files.ForEach(file =>
            {
                Assembly assemblyLoaded = Assembly.LoadFile(file.FullName);
                var loadedTypes = assemblyLoaded
                .GetTypes()
                .Where(t => t.IsClass && typeof(IInterface).IsAssignableFrom(t))
                .ToList();

                if (loadedTypes.Count == 0)
                {
                    //Console.WriteLine($"Nadie implementa la interfaz: {typeof(IInterface).Name} en el assembly: {file.FullName}");

                    return;
                }

                this.implementations = implementations
                .Union(loadedTypes)
                .ToList();
            });

            return this.implementations.ConvertAll(t => t.Name);
        }

        public IInterface GetImplementation(string implementationMethod, string path, params object[] args)
        {
            var files = new DirectoryInfo(path)
                .GetFiles("*.dll")
                .ToList();

            implementations = [];
            files.ForEach(file =>
            {
                Assembly assemblyLoaded = Assembly.LoadFile(file.FullName);
                var loadedTypes = assemblyLoaded
                .GetTypes()
                .Where(t => t.IsClass && typeof(IInterface).IsAssignableFrom(t))
                .ToList();

                if (loadedTypes.Count == 0)
                {
                    Console.WriteLine($"Nadie implementa la interfaz: {typeof(IInterface).Name} en el assembly: {file.FullName}");

                    return;
                }

                this.implementations = implementations
                .Union(loadedTypes)
                .ToList();
            });

            var type = implementations.FirstOrDefault(x => x.Name == implementationMethod);
            if (type == null)
            {
                throw new InvalidOperationException("Must enter an available name method");
            }
            return Activator.CreateInstance(type, args) as IInterface;
        }
    }
}
