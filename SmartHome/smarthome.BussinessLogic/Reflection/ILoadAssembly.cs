using System.Diagnostics.CodeAnalysis;

namespace smarthome.BussinessLogic
{
    public interface ILoadAssembly<IInterface>
    where IInterface : class
    {
        List<string> GetImplementations(string path);
        IInterface GetImplementation(string implementationMethod, string path, params object[] args);
    }
}
