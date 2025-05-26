using JsonPathToModel.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JsonPathToModel.Tests.Extensions;

public class HackingExtensionsTests
{
    [Fact]
    public void HackingExtensions_Should_Update_Privates()
    {
        var target = new HackingExtensionsTestsSample();
        target.WithHack("_privateProperty", "test1");
        target.WithHack("_private", "test2");

        Assert.Equal("test1", target.StealValue("_privateProperty"));
        Assert.Equal("test2", target.StealValue("_private"));
    }

    [Fact]
    public void HackingExtensions_Should_Update_Readonly()
    {
        var target = new HackingExtensionsTestsSample();
        target.WithHack("_readonlyDependency", "test");
        Assert.Equal("test", target.StealValue("_readonlyDependency"));
    }

    [Fact]
    public void HackingExtensions_Should_Update_PrivateSet()
    {
        var target = new HackingExtensionsTestsSample();
        target.WithHack("_privateSetProperty", "test");
        Assert.Equal("test", target.StealValue("_privateSetProperty"));
    }

    [Fact]
    public void HackingExtensions_Should_Update_Internal()
    {
        var target = new HackingExtensionsTestsSample();
        target.WithHack("_internal", "test");
        Assert.Equal("test", target.StealString("_internal"));
    }

    [Fact]
    public void HackingExtensions_Should_Update_Protected_FullNotation()
    {
        var target = new HackingExtensionsTestsSample();
        target.WithHack("$._protected", "test");
        Assert.Equal("test", target.StealString("$._protected"));
    }

    [Fact]
    public void HackingExtensions_Should_Update_Protected()
    {
        var target = new HackingExtensionsTestsSample();
        target.WithHack("_protected", "test");
        Assert.Equal("test", target.StealString("_protected"));
    }

    [Fact]
    public void HackingExtensions_ShouldNot_Update_GetProperty()
    {
        var target = new HackingExtensionsTestsSample();
        var exc = Assert.Throws<NavigationException>(() =>  target.WithHack("_getProperty", "test"));
        Assert.Contains("_getProperty", exc.Message);
    }

    [Fact]
    public void HackingExtensions_ExecutePrivate_Throws_Exception()
    {
        var target = new HackingExtensionsTestsSample();
        var exc = Assert.Throws<ArgumentException>(() => target.ExecutePrivate("WrongMethod", "test", 3.0));
        Assert.Contains("WrongMethod", exc.Message);
    }

    [Fact]
    public void HackingExtensions_ExecutePrivate_Executes_Method_With_Params()
    {
        var target = new HackingExtensionsTestsSample();
        var result = target.ExecutePrivate("PrivateMethod", "test", 3M);
        Assert.Equal("test3", result);
    }

    [Fact]
    public void HackingExtensions_ExecutePrivateStatic_Throws_Exception()
    {
        var target = new HackingExtensionsTestsSample();
        var exc = Assert.Throws<ArgumentException>(() => target.ExecutePrivateStatic("WrongStaticMethod", "test", 3.0));
        Assert.Contains("WrongStaticMethod", exc.Message);
    }

    [Fact]
    public void HackingExtensions_ExecutePrivateStatic_Executes_Method_With_Params()
    {
        var target = new HackingExtensionsTestsSample();
        var result = target.ExecutePrivateStatic("PrivateStaticMethod", "test", 3M);
        Assert.Equal("test3static", result);
    }

    [Fact]
    public void HackingExtensions_Type_ExecutePrivateStatic_Throws_Exception()
    {
        var exc = Assert.Throws<ArgumentException>(() => typeof(HackingExtensionsTestsSample).ExecutePrivateStatic("WrongStaticMethod", "test", 3.0));
        Assert.Contains("WrongStaticMethod", exc.Message);
    }

    [Fact]
    public void HackingExtensions_Type_ExecutePrivateStatic_Executes_Method_With_Params()
    {
        var result = typeof(HackingExtensionsTestsSample).ExecutePrivateStatic("PrivateStaticMethod", "test", 3M);
        Assert.Equal("test3static", result);
    }
}

public class HackingExtensionsTestsSample
{
    private readonly object? _readonlyDependency = null;
    private object? _privateProperty { get; set; } = null;
    private object? _private = null;
    public object? _privateSetProperty { get; private set; } = null;
    internal object? _internal = null;
    protected object? _protected = null;
    public object? _getProperty { get; } = null;

    private string PrivateMethod(string s, decimal d)
    {
        return s + d.ToString(); 
    }

    private static string PrivateStaticMethod(string s, decimal d)
    {
        return s + d.ToString() + "static";
    }
}
