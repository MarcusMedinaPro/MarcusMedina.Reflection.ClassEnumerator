// -----------------------------------------------------------------------------------------------
//  EnumerateClassesTests.cs by Marcus Medina, Copyright (C) 2025, http://MarcusMedina.Pro.
//  Published under Attribution-ShareAlike 4.0 International (CC BY-SA 4.0)
//  https://creativecommons.org/licenses/by-sa/4.0/
// -----------------------------------------------------------------------------------------------

namespace MarcusMedinaPro.ClassEnumerator.Tests;

using Xunit;

// Test interfaces and classes
file interface ITestPlugin
{
    string Name { get; }
}

file sealed class TestPlugin1 : ITestPlugin
{
    public string Name => "Plugin1";
}

file sealed class TestPlugin2 : ITestPlugin
{
    public string Name => "Plugin2";
}

file sealed class TestPlugin3 : ITestPlugin
{
    public string Name => "Plugin3";
}

// Test base class hierarchy
file abstract class BaseService
{
    public abstract string ServiceName { get; }
}

file sealed class EmailService : BaseService
{
    public override string ServiceName => "Email";
}

file sealed class SmsService : BaseService
{
    public override string ServiceName => "SMS";
}

// Class without parameterless constructor (should be excluded)
file sealed class InvalidPlugin(string required) : ITestPlugin
{
    public string Name { get; } = required;
}

// Interface with no implementations (for testing empty results)
file interface INonExistentInterface { }

public class EnumerateClassesTests
{
    [Fact]
    public void GetClassesByInterface_FindsAllImplementations()
    {
        // Act
        var result = EnumerateClasses<ITestPlugin>.GetClassesByInterface();

        // Assert
        Assert.NotNull(result);
        var list = result.ToList();

        // Should find TestPlugin1, TestPlugin2, TestPlugin3 (but not InvalidPlugin - no parameterless constructor)
        Assert.Equal(3, list.Count);
        Assert.Contains(list, p => p.Name == "Plugin1");
        Assert.Contains(list, p => p.Name == "Plugin2");
        Assert.Contains(list, p => p.Name == "Plugin3");
    }

    [Fact]
    public void GetClassesByInterface_ReturnsEmptyForNoMatches()
    {
        // Arrange
        // Act
        var result = EnumerateClasses<INonExistentInterface>.GetClassesByInterface();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void GetClassesByInterface_ReturnsSortedResults()
    {
        // Act
        var result = EnumerateClasses<ITestPlugin>.GetClassesByInterface().ToList();

        // Assert - should be sorted by type name
        Assert.Equal("Plugin1", result[0].Name);
        Assert.Equal("Plugin2", result[1].Name);
        Assert.Equal("Plugin3", result[2].Name);
    }

    [Fact]
    public void GetClassesByInheritance_FindsAllDerivedClasses()
    {
        // Act
        var result = EnumerateClasses<BaseService>.GetClassesByInheritance();

        // Assert
        Assert.NotNull(result);
        var list = result.ToList();

        Assert.Equal(2, list.Count);
        Assert.Contains(list, s => s.ServiceName == "Email");
        Assert.Contains(list, s => s.ServiceName == "SMS");
    }

    [Fact]
    public void GetClassesByInheritance_ExcludesAbstractClasses()
    {
        // Act
        var result = EnumerateClasses<BaseService>.GetClassesByInheritance().ToList();

        // Assert - should not include BaseService itself (abstract)
        Assert.DoesNotContain(result, s => s.GetType() == typeof(BaseService));
    }

    [Fact]
    public void ListClassesByInterface_ReturnsTypes()
    {
        // Act
        var result = EnumerateClasses<ITestPlugin>.ListClassesByInterface();

        // Assert
        Assert.NotNull(result);
        var types = result.ToList();

        Assert.Equal(3, types.Count);
        Assert.Contains(types, type => type.Name.EndsWith(nameof(TestPlugin1), StringComparison.Ordinal));
        Assert.Contains(types, type => type.Name.EndsWith(nameof(TestPlugin2), StringComparison.Ordinal));
        Assert.Contains(types, type => type.Name.EndsWith(nameof(TestPlugin3), StringComparison.Ordinal));
    }

    [Fact]
    public void ListClassesByInterface_TypesImplementInterface()
    {
        // Act
        var types = EnumerateClasses<ITestPlugin>.ListClassesByInterface().ToList();

        // Assert - all types should implement ITestPlugin
        Assert.All(types, type =>
        {
            Assert.True(typeof(ITestPlugin).IsAssignableFrom(type));
        });
    }

    [Fact]
    public void ListClassesByInheritance_ReturnsTypes()
    {
        // Act
        var result = EnumerateClasses<BaseService>.ListClassesByInheritance();

        // Assert
        Assert.NotNull(result);
        var types = result.ToList();

        Assert.Equal(2, types.Count);
        Assert.Contains(types, type => type.Name.EndsWith(nameof(EmailService), StringComparison.Ordinal));
        Assert.Contains(types, type => type.Name.EndsWith(nameof(SmsService), StringComparison.Ordinal));
    }

    [Fact]
    public void ListClassesByInheritance_TypesInheritFromBase()
    {
        // Act
        var types = EnumerateClasses<BaseService>.ListClassesByInheritance().ToList();

        // Assert - all types should inherit from BaseService
        Assert.All(types, type =>
        {
            Assert.True(type.IsSubclassOf(typeof(BaseService)));
        });
    }

    [Fact]
    public void GetClassesByInterface_CreatesNewInstances()
    {
        // Act
        var result = EnumerateClasses<ITestPlugin>.GetClassesByInterface().ToList();

        // Assert - each call should create new instances
        var result2 = EnumerateClasses<ITestPlugin>.GetClassesByInterface().ToList();

        for (int i = 0; i < result.Count; i++)
        {
            // Same type but different instance
            Assert.Equal(result[i].GetType(), result2[i].GetType());
            Assert.NotSame(result[i], result2[i]);
        }
    }

    [Fact]
    public void GetClassesByInterface_ExcludesClassesWithoutParameterlessConstructor()
    {
        // Act
        var result = EnumerateClasses<ITestPlugin>.GetClassesByInterface().ToList();

        // Assert - should not contain InvalidPlugin (requires constructor parameter)
        Assert.DoesNotContain(result, p => p.GetType() == typeof(InvalidPlugin));
    }
}
