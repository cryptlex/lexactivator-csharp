# lexactivator-csharp
LexActivator - C# licensing library

Refer to following for documentation:

https://docs.cryptlex.com/node-locked-licenses/using-lexactivator

## Dependencies

Install-Package AdvancedDLSupport

Install-Package Mono.DllMap

Install-Package StrictEmit

## In .csproj file add following reference:

<ItemGroup>
    ...
    <Reference Include="netstandard" />
    ...

## In case you are targetting AnyCpu configuration:

Create following folder structure in parallel to the exe file:

lib/x86/LexActivator.dll
lib/x64/LexActivator.dll
