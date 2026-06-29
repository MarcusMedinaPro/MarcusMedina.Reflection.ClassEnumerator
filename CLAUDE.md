# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

**MMPClassEnumerator** - A C# NuGet package for discovering and instantiating classes by interface or inheritance using reflection. Perfect for plugin systems, dynamic class loading, and educational purposes.

- **Language:** C# 14.0
- **Framework:** .NET 10.0+
- **Pattern:** Generic static class with reflection-based discovery
- **License:** MIT

## Quick Commands

### Development
```bash
# Restore and build
cd csharp
dotnet restore
dotnet build --configuration Release

# Run tests
dotnet test --configuration Release

# Pack NuGet package
dotnet pack src/MMPClassEnumerator/MMPClassEnumerator.csproj --configuration Release

# Run single test
dotnet test --filter ClassName.MethodName
```

### Release & Publishing
```bash
# Create version tag (triggers GitHub Actions)
git tag -a v1.2.0 -m "Release v1.2.0"
git push origin v1.2.0

# The multi-stage GitHub Actions pipeline will:
# 1. Build and test
# 2. Run CodeQL security analysis
# 3. Sign packages with certificate
# 4. Publish to NuGet.org
```

## Architecture

### Core Components

**EnumerateClasses<T>** (`EnumerateClasses.cs`)
- Static generic class for type discovery
- `GetClassesByInterface()` - Find and instantiate classes implementing interface T
- `GetClassesByInheritance()` - Find and instantiate classes inheriting from T
- `ListClassesByInterface()` - Get Type definitions without instantiation
- `ListClassesByInheritance()` - Get Type definitions without instantiation
- `ClearCache()` - Clear cached discovery results

### Key Patterns

**Interface-based Discovery:**
```csharp
using MarcusMedinaPro.ClassEnumerator;

var plugins = EnumerateClasses<IPlugin>.GetClassesByInterface().ToList();
```

**Inheritance-based Discovery:**
```csharp
var handlers = EnumerateClasses<BaseHandler>.GetClassesByInheritance().ToList();
```

**With Caching:**
```csharp
var plugins = EnumerateClasses<IPlugin>.GetClassesByInterface(useCache: true);
```

**Custom Assembly Scanning:**
```csharp
var assembly = Assembly.LoadFrom("plugins.dll");
var plugins = EnumerateClasses<IPlugin>.GetClassesByInterface(assembly: assembly);
```

### File Structure
```
csharp/
├── MMPClassEnumerator.slnx
├── src/MMPClassEnumerator/          # Main library
│   └── EnumerateClasses.cs          # Core implementation
└── src/MMPClassEnumerator.Tests/    # xUnit tests
    └── *Tests.cs
samples/
└── MMPClassEnumerator.Demo/         # Demo console app
    └── Program.cs
```

## Testing Strategy

- **Framework:** xUnit with coverlet for coverage
- **Approach:** Unit tests for all discovery methods, caching, and edge cases
- Tests verify:
  - Interface-based class discovery
  - Inheritance-based class discovery
  - Type listing without instantiation
  - Caching behaviour
  - Custom assembly scanning
  - Edge cases (abstract classes, interfaces excluded)

## GitHub Actions Workflow

**Release Pipeline** (`.github/workflows/release.yml`)

Triggers on: `git push` with `v*` tags or to `main`/`release` branches

4-stage pipeline:
1. **Build & Test** - Restore, build, test, pack (produces unsigned packages)
2. **Quality Gate** - CodeQL analysis, vulnerability scanning
3. **Package Signing** - Sign packages, verify signatures, generate SHA256 checksums
4. **Publish to NuGet** - Only runs on version tags (refs/tags/v*)

**Required Secrets:**
- `NUGET_API_KEY` - NuGet.org API key
- `NUGET_SIGNING_CERT` - Base64-encoded signing certificate (.pfx)
- `NUGET_SIGNING_CERT_PASSWORD` - Certificate password

## Important Implementation Details

### Reflection-based Discovery

The library uses .NET reflection to:
1. Scan the calling assembly (or specified assembly) for types
2. Filter by interface implementation or base class inheritance
3. Exclude abstract classes and interfaces
4. Verify parameterless constructors exist
5. Create instances via `Activator.CreateInstance()`
6. Return results sorted by type name

### Thread-Safe Caching

Uses `ConcurrentDictionary` for cache storage. Cache key is based on the type parameter T and the discovery method used.

### NuGet Package Configuration

Key settings in `.csproj`:
- **Package ID:** `MMPClassEnumerator`
- **Namespace:** `MarcusMedinaPro.ClassEnumerator`
- **License:** MIT
- **Repository:** GitHub repository URL

## Documentation

- **README.md** - Quick start, features, installation, API reference
- **CHANGELOG.md** - Version history with breaking changes
- **documents/** - Additional documentation (SEO, detailed README)
- **LICENSE** - MIT License file

## Common Issues & Solutions

**Build Fails on Demo Project:**
- Verify path in demo .csproj: `../../csharp/src/MMPClassEnumerator/MMPClassEnumerator.csproj`

**Tests Not Found:**
- Ensure test project is included in the solution file
- Run from `csharp/` directory: `dotnet test`

**Caching Not Working:**
- Pass `useCache: true` parameter explicitly
- Call `ClearCache()` if assembly has changed

## Semantic Versioning

Version format: `MAJOR.MINOR.PATCH`

- **MAJOR** - Breaking changes to API
- **MINOR** - New features, backwards compatible
- **PATCH** - Bug fixes, no API changes

Breaking changes are documented in CHANGELOG with migration guides.
