using System.Management.Automation;
using System.Reflection;
using System.Runtime.Loader;

namespace Jagabata;

internal class AclModuleAssemblyLoadContext(string dependencyDirPath) : AssemblyLoadContext
{
    protected override Assembly? Load(AssemblyName assemblyName)
    {
        string assemblyPath = Path.Combine(dependencyDirPath, $"{assemblyName.Name}.dll");
        return File.Exists(assemblyPath)
            ? LoadFromAssemblyPath(assemblyPath)
            : null;
    }
}

public class AclModuleResolveEventHandler : IModuleAssemblyInitializer, IModuleAssemblyCleanup
{
    private static readonly string s_dependencyDirPath =
        Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "libs"));

    private static readonly AclModuleAssemblyLoadContext s_dependencyAcl = new(s_dependencyDirPath);

    public void OnImport()
    {
        AssemblyLoadContext.Default.Resolving += ResolveAclEngine;
    }

    public void OnRemove(PSModuleInfo psModuleInfo)
    {
        AssemblyLoadContext.Default.Resolving -= ResolveAclEngine;
    }

    private static Assembly? ResolveAclEngine(AssemblyLoadContext defaultAlc, AssemblyName assemblyToResolve)
    {
        return assemblyToResolve.Name == "Jagabata.Yaml"
            ? s_dependencyAcl.LoadFromAssemblyName(assemblyToResolve)
            : null;
    }
}
