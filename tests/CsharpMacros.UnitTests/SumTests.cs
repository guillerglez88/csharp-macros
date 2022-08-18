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

public class SumTests
{
    public SumTests()
    {
        Module.InitializeAllModules();
    }

    [Fact]
    public void CanSum()
    {
        var sum =
            E("fn",
                E("a", typeof(int),
                  "b", typeof(int)),
                E("+",
                    E("param", "a"),
                    E("param", "b")))
            .Compile(contract: (int a, int b) => default(int));

        var result = sum(2, 3);

        Assert.Equal(5, result);
    }

    [Fact]
    public void CanSumMultipleParams()
    {
        var sum = E("+", 1, 2, 3, 4, 5)
            .Compile(contract: () => default(int));

        var result = sum();

        Assert.Equal(15, result);
    }

    [Fact]
    public void CanTranslateSum()
    {
        var expSum =
            E("+",
                E("const", 2),
                E("const", 3));

        var expressionSum = TranslateSum(expSum);

        Assert.IsAssignableFrom<BinaryExpression>(expressionSum);
    }

    [Fact]
    public void CanExpandSum()
    {
        var expanded =
            E("+",
                E("+",
                    E("+",
                        E("+",
                            E("+", 
                                E("const", 0), 
                                E("const", 1)),
                            E("const", 2)),
                        E("const", 3)),
                    E("const", 4)),
                E("const", 5));

        var exp = ExpandSum((exp) => exp.Expand(), E("+", 1, 2, 3, 4, 5));

        Assert.Equal(
            JsonConvert.SerializeObject(expanded), 
            JsonConvert.SerializeObject(exp));
    } 
}
