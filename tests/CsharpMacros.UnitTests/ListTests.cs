using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static CsharpMacros.Exp;

namespace CsharpMacros.UnitTests;

public class ListTests
{
    public ListTests()
    {
        Module.InitializeAllModules();
    }

    [Fact]
    public void CanBuildList()
    {
        var exp = E("list", 1, 3, "hello", 5.6, false);

        var list = exp
            .Compile(contract: () => default(object[]))
            .Invoke();
        var strList = $"[{string.Join(", ", list)}]";

        Assert.Equal("[1, 3, hello, 5.6, False]", strList);
    }

    [Fact]
    public void CanBuildNestedLists()
    {
        var exp = E("list", 1, E("list", "nested", 78), 3, "hello", 5.6, false);

        var list = exp
            .Compile(contract: () => default(object[]))
            .Invoke()
            .Select(item => typeof(object[]).IsAssignableFrom(item.GetType()) ? $"[{string.Join(", ", (object[])item)}]" : item);
        var strList = $"[{string.Join(", ", list)}]";

        Assert.Equal("[1, [nested, 78], 3, hello, 5.6, False]", strList);
    }
}
