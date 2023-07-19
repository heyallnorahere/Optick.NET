# Optick.NET

C# bindings for the [Optick profiler](https://github.com/bombomby/optick), written for .NET Standard 2.0 and 2.1.

## Which packages should I reference?

The `Optick.NET.Redist` package is required **separately** from `Optick.NET`. Along with the binary for `Optick.NET`, it contains the base `OptickCore` DLL, for the base C api.

To use the bindings in your project, you will need to add the following XML:
```xml
...
<ItemGroup>
    ...
    <PackageReference Include="Optick.NET" Version="$(OptickNETVersion)" />
    <PackageReference Include="Optick.NET.Redist" Version="$(OptickNETVersion)" />
    ...
</ItemGroup>
...
```

Additionally, you will need to add a reference to the `https://nuget.pkg.github.com/yodasoda1219/index.json` NuGet registry via [`nuget.config`](https://learn.microsoft.com/en-us/nuget/reference/nuget-config-file).