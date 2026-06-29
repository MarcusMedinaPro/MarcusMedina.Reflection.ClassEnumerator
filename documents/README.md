# MMPClassEnumerator

[![NuGet](https://img.shields.io/nuget/v/MMPClassEnumerator.svg)](https://www.nuget.org/packages/MMPClassEnumerator/)
[![.NET](https://img.shields.io/badge/.NET-10.0+-512BD4?logo=dotnet)](https://dotnet.microsoft.com/)
[![License](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT)

A simple utility for discovering and instantiating classes by interface or inheritance using
reflection. Perfect for plugin systems, dynamic class loading, and educational purposes.

## Overview

MMPClassEnumerator eliminates boilerplate code when working with plugins or dynamic class
discovery. Instead of manually instantiating each class with switch statements, discover
and create instances automatically using reflection.

## Features

- ✅ **Interface-based Discovery** - Find classes implementing specific interfaces
- ✅ **Inheritance-based Discovery** - Find classes inheriting from base classes
- ✅ **Instance Creation** - Auto-instantiate discovered classes
- ✅ **Type Listing** - Get type definitions without instantiation
- ✅ **Performance Caching** - Optional caching for improved performance
- ✅ **Custom Assembly Scanning** - Scan any assembly, not just calling assembly
- ✅ **Comprehensive Tests** - Full test coverage with xUnit
- ✅ **Safe Exception Handling** - Gracefully handles instantiation failures
- ✅ **Zero Dependencies** - Pure .NET reflection
- ✅ **Educational Tool** - Great for teaching reflection and plugins

## Requirements

- .NET 8.0 or higher

## Installation

### Package Manager Console
```powershell
Install-Package MMPClassEnumerator
```

### .NET CLI
```bash
dotnet add package MMPClassEnumerator
```

### Package Reference
```xml
<PackageReference Include="MMPClassEnumerator" Version="1.2.0" />
```

## Quick Start

### The Problem

**Before** - Manual instantiation with switch statements:

```csharp
ITopic topic;
switch(choice)
{
    case 0: topic = new Topic0(); break;
    case 1: topic = new Topic1(); break;
    case 2: topic = new Topic2(); break;
    // Long list of cases...
}
```

### The Solution

**After** - Dynamic discovery and instantiation:

```csharp
using MarcusMedinaPro.ClassEnumerator;

// Get all classes implementing ITopic
var topics = EnumerateClasses<ITopic>.GetClassesByInterface().ToList();

// Display options
for (int i = 0; i < topics.Count; i++)
    Console.WriteLine($"{i:00} {topics[i].GetType().Name}");

// User selects
Console.Write("Select a topic: ");
var choice = int.Parse(Console.ReadLine()!);

// Use selected instance directly
ITopic topic = topics[choice];
```

## API Reference

### Get Instances by Interface

Returns instantiated objects of all classes implementing the specified interface:

```csharp
// Basic usage
IEnumerable<T> instances = EnumerateClasses<IMyInterface>.GetClassesByInterface();

// With caching for better performance
IEnumerable<T> cached = EnumerateClasses<IMyInterface>.GetClassesByInterface(useCache: true);

// Scan specific assembly
var assembly = Assembly.Load("MyPlugins");
IEnumerable<T> plugins = EnumerateClasses<IMyInterface>.GetClassesByInterface(assembly: assembly);
```

### Get Instances by Inheritance

Returns instantiated objects of all classes inheriting from the specified base class:

```csharp
IEnumerable<T> instances = EnumerateClasses<MyBaseClass>.GetClassesByInheritance();
```

### List Types by Interface

Returns type definitions without creating instances:

```csharp
IEnumerable<Type> types = EnumerateClasses<IMyInterface>.ListClassesByInterface();
```

### List Types by Inheritance

Returns type definitions without creating instances:

```csharp
IEnumerable<Type> types = EnumerateClasses<MyBaseClass>.ListClassesByInheritance();
```

## Usage Examples

### Plugin System

```csharp
public interface IPlugin
{
    string Name { get; }
    void Execute();
}

// Auto-discover and load all plugins
var plugins = EnumerateClasses<IPlugin>.GetClassesByInterface().ToList();

// Display available plugins
plugins.ForEach(p => Console.WriteLine($"Plugin: {p.Name}"));

// Execute all plugins
plugins.ForEach(p => p.Execute());
```

### List All Classes in Assembly

```csharp
// Get all classes (using object as base)
var allClasses = EnumerateClasses<object>.GetClassesByInheritance().ToList();

// Display class names
allClasses.ForEach(c => Console.WriteLine($"class {c.GetType().Name}()"));
```

### Menu System

```csharp
public interface IMenuItem
{
    string DisplayName { get; }
    void Run();
}

var menuItems = EnumerateClasses<IMenuItem>.GetClassesByInterface().ToList();

// Display menu
for (int i = 0; i < menuItems.Count; i++)
    Console.WriteLine($"{i + 1}. {menuItems[i].DisplayName}");

// Execute selected item
int selection = int.Parse(Console.ReadLine()!) - 1;
menuItems[selection].Run();
```

## How It Works

MMPClassEnumerator uses .NET reflection to:

1. Scan the calling assembly for types
2. Filter types matching the specified interface or base class
3. Verify types have parameterless constructors
4. Optionally create instances via `Activator.CreateInstance()`
5. Return results sorted by type name

## Requirements and Limitations

- **Parameterless Constructor**: All discovered classes must have a public parameterless constructor
- **Calling Assembly**: Only scans the assembly that calls the method
- **Performance**: Reflection has overhead - cache results if calling frequently

## Educational Purpose

This library was created as an educational tool to demonstrate:

- **Reflection API**: How to discover types at runtime
- **Generic Programming**: Using generic type parameters
- **LINQ**: Query syntax for filtering types
- **NuGet Publishing**: Complete package creation workflow

### Performance - With Caching

```csharp
// First call - scans assembly (slower)
var plugins = EnumerateClasses<IPlugin>.GetClassesByInterface(useCache: true);

// Subsequent calls - uses cache (much faster!)
var pluginsAgain = EnumerateClasses<IPlugin>.GetClassesByInterface(useCache: true);

// Clear cache if needed (e.g., after dynamic assembly loading)
EnumerateClasses<IPlugin>.ClearCache();
```

### Advanced - Scan Multiple Assemblies

```csharp
var allPlugins = new List<IPlugin>();

// Scan main assembly
allPlugins.AddRange(EnumerateClasses<IPlugin>.GetClassesByInterface());

// Scan plugin assemblies
var pluginAssemblies = Directory.GetFiles("./plugins", "*.dll");
foreach (var dll in pluginAssemblies)
{
    var assembly = Assembly.LoadFrom(dll);
    allPlugins.AddRange(EnumerateClasses<IPlugin>.GetClassesByInterface(assembly: assembly));
}
```

## Performance Benchmarks

| Method | Items | Time | Speedup |
|--------|-------|------|---------|
| No Cache | 10 classes | ~2ms | 1x |
| With Cache | 10 classes | ~0.01ms | 200x faster |

*Caching is especially beneficial when called frequently (e.g., in request handlers)*

## Migration Guide

### From v1.0.x to v1.1.0

- **.NET 6 → .NET 8**: Update project target framework
- **Improved Descriptions**: Better package metadata
- **XML Documentation**: Full API documentation
- No breaking API changes - drop-in replacement

### From v1.1.0 to v1.2.0

**New Features (backward compatible):**
- `useCache` parameter for all methods (default: false)
- `assembly` parameter to scan custom assemblies
- `ClearCache()` method for cache management
- Comprehensive test suite included

**Improvements:**
- Better null-safety (removed excessive `?.` operators)
- Improved exception handling
- Excludes abstract classes and interfaces automatically
- Better XML documentation with code examples

No breaking changes - existing code works as-is!

## License

This work is licensed under the [MIT License](https://opensource.org/licenses/MIT).

## Credits

- **Author**: Marcus Medina
- **Inspiration**: [Stack Overflow answer by @Jon Skeet](https://stackoverflow.com/a/699871)
- **Icon**: [Icons8 iOS7 Programming CLS Icon](https://iconarchive.com/show/ios7-icons-by-icons8/Programming-Cls-icon.html)

## Source Code

Full source available on [GitHub](https://github.com/MarcusMedinaPro/MMPClassEnumerator)

---

**Made with ❤️ for educational purposes**

---
_For metadata and SEO keywords, see [SEO.md](SEO.md)._
