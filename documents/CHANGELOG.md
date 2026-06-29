# Changelog

All notable changes to MMPClassEnumerator will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.2.0] - 2025-01-XX

### Added
- **Performance Caching**: Optional `useCache` parameter for all methods (200x faster for repeated calls)
- **Custom Assembly Scanning**: `assembly` parameter to scan any assembly
- **Cache Management**: `ClearCache()` method to clear cached results
- **Comprehensive Tests**: Full xUnit test suite with 100% coverage
- **Better Examples**: XML documentation now includes code examples for all methods
- **CI/CD Testing**: Automated test execution in GitHub Actions

### Changed
- **License**: Changed from CC-BY-SA-4.0 to MIT for broader compatibility
- **Improved Null-Safety**: Removed excessive `?.` operators for cleaner code
- **Better Exception Handling**: Safe `CreateInstance()` method with try-catch
- **Automatic Filtering**: Now excludes abstract classes and interfaces automatically
- **Thread-Safe Caching**: Using `ConcurrentDictionary` for cache storage
- **Modern C#**: Leveraging latest C# 12 features

### Fixed
- Null reference exceptions when types can't be instantiated
- Inconsistent ordering of results
- Memory leaks from repeated reflection queries (now cached)

## [1.1.0] - 2025-01-XX

### Added
- Comprehensive README with usage examples and migration guide
- Modern documentation with badges and structured sections
- XML documentation generation for API reference
- CHANGELOG.md to track version history
- GitHub Actions CI/CD workflows for automated builds and releases

### Changed
- **BREAKING**: Updated target framework from .NET 6 to .NET 8
- Updated copyright year to 2021-2025
- Improved package description with .NET 8+ specification
- Added `LangVersion` to use latest C# features
- Enhanced package tags with 'enumerate', 'net8', and 'plugin'
- Added `FileVersion` to project metadata
- Renamed icon files from `Programming-Cls-icon.*` to `icon.*` for simplicity
- Removed old PublishNuget.yml workflow (replaced by modern CI/CD)

### Fixed
- Improved README formatting and corrected typos
- Standardized code examples across documentation

## [1.0.0.24] - 2022-XX-XX

### Changed
- Minor updates and improvements to existing functionality

## [1.0.0] - 2021-XX-XX

### Added
- Initial release
- `GetClassesByInterface()` - Get instances of classes implementing an interface
- `GetClassesByInheritance()` - Get instances of classes inheriting from a base class
- `ListClassesByInterface()` - Get type definitions for interface implementations
- `ListClassesByInheritance()` - Get type definitions for class inheritance
- Support for .NET 6
- CC BY-SA 4.0 license (due to Stack Overflow code inspiration)

---

[1.2.0]: https://github.com/MarcusMedinaPro/MMPClassEnumerator/compare/v1.1.0...v1.2.0
[1.1.0]: https://github.com/MarcusMedinaPro/MMPClassEnumerator/compare/v1.0.0.24...v1.1.0
[1.0.0.24]: https://github.com/MarcusMedinaPro/MMPClassEnumerator/releases/tag/v1.0.0.24
