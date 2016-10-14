<Query Kind="Statements">
  <NuGetReference>JetBrains.Mono.Cecil</NuGetReference>
  <Namespace>Mono.Cecil</Namespace>
</Query>

var f = AssemblyDefinition.ReadAssembly(@"C:\project\work\sph\source\unit.test\assembly.test\bin\Debug\assembly.test.dll");
var md = f.MainModule;
var adapter = md.Types.Single(x => x.Name == "AssemblyClassToTest");
var type = adapter.Properties.Single(x => x.Name == "ChildCollection").PropertyType;

TypeDefinition td = type as TypeDefinition;
var generic = type as GenericInstanceType;
var scope = generic.ElementType.Scope.Name;

var child = generic.GenericArguments[0];

Console.WriteLine($"{child.Scope} == {adapter.Scope}");
var childDll = $@"C:\project\work\sph\source\unit.test\assembly.test\bin\Debug\{child.Scope}";
var ca = AssemblyDefinition.ReadAssembly(childDll);
