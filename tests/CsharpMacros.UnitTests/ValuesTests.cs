using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using static CsharpMacros.MacrosModule;
using static CsharpMacros.ExpModule;

namespace CsharpMacros.UnitTests;

public class ValuesTests
{
    [Fact]
    public void CanCreateConsts()
    {
        var value = E("const", 5)
            .Compile(contract: () => default(int));

        var result = value();

        Assert.Equal(5, result);
    }

    [Fact]
    public void CanCast()
    {
        var castToInt =
            E("fn",
                E("a", typeof(double)),
                E("cast", typeof(int), 
                    E("param", "a")))
            .Compile(contract: (double a) => default(int));

        var result = castToInt(Math.PI);

        Assert.Equal(3, result);
    }
}
