# Build

## requirements
- dotnet: `> 8.0`
- PowerShell: `> 7.3`
  - platyPS (See: https://learn.microsoft.com/en-us/powershell/utility-modules/platyps/create-help-using-platyps?view=ps-modules)

## Build C# library

```powershell
cd path\to\project
dotnet build .\src\Jagabata.csproj -t:Build -p:Configuration=Release
```

## Build Powershell module

```powershell
cd path\to\project
dotnet build .\src\Jagabata.csproj -t:PSM -p:Configuration=Release
```

module files are created into `out` directory

## Update documents

```powershell
cd path\to\project
dotnet build .\src\Jagabata.csproj -t:docs -p:Configuration=Release
```

or

```powershell
cd path\to\project
.\docs\Make-Doc.ps1
```

