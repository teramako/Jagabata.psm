using System.Management.Automation;
using System.Reflection;
using System.Runtime.Loader;

namespace Jagabata;

internal class AclModuleAssemblyLoadContext : AssemblyLoadContext
{
    private readonly string _dependencyDirPath;

    public AclModuleAssemblyLoadContext(string dependencyDirPath)
    {
        _dependencyDirPath = dependencyDirPath;
    }

    protected override Assembly? Load(AssemblyName assemblyName)
    {
        string assemblyPath = Path.Combine(_dependencyDirPath, $"{assemblyName.Name}.dll");
        if (File.Exists(assemblyPath))
        {
            return LoadFromAssemblyPath(assemblyPath);
        }
        return null;
    }
}

public class AclModuleResolveEventHandler : IModuleAssemblyInitializer, IModuleAssemblyCleanup
{
    private static readonly string s_dependencyDirPath =
        Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!, "libs"));

    private static readonly AclModuleAssemblyLoadContext s_dependencyAcl =
        new AclModuleAssemblyLoadContext(s_dependencyDirPath);

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
        if (assemblyToResolve.Name != "Jagabata.Yaml")
        {
            return null;
        }
        return s_dependencyAcl.LoadFromAssemblyName(assemblyToResolve);
    }
}
