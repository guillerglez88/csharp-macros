using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using static CsharpMacros.Arithmetics;
using static CsharpMacros.Exp;

namespace CsharpMacros.UnitTests;

public class SubTests
{
    public SubTests()
    {
        Module.InitializeAllModules();
    }

    [Fact]
    public void CanSubtract()
    {
        var sub =
            E("fn",
                E("a", typeof(int),
                  "b", typeof(int)),
                E("sub",
                    E("param", "a"),
                    E("param", "b")))
            .Compile(contract: (int a, int b) => default(int));

        var result = sub(20, 7);

        Assert.Equal(13, result);
    }

    [Fact]
    public void CanSubtractMultipleParams()
    {
        var sub = E("sub", 15, 1, 2, 3, 4)
            .Compile(contract: () => default(int));

        var result = sub();

        Assert.Equal(5, result);
    }

    [Fact]
    public void CanTranslateSub()
    {
        var expSum =
            E("sub",
                E("const", 2),
                E("const", 3));

        var expressionSum = TranslateSub(expSum);

        Assert.IsAssignableFrom<BinaryExpression>(expressionSum);
    }

    [Fact]
    public void CanExpandSub()
    {
        var expanded =
            E("sub",
                E("sub",
                    E("sub",
                        E("sub",
                            E("const", 1),
                            E("const", 2)),
                        E("const", 3)),
                    E("const", 4)),
                E("const", 5));

        var exp = E("sub", 1, 2, 3, 4, 5).Expand();

        Assert.Equal(
            JsonConvert.SerializeObject(expanded), 
            JsonConvert.SerializeObject(exp));
    } 
}
