using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using static CsharpMacros.ExpressionModule;

namespace CsharpMacros.UnitTests;

public class SumTests
{
    [Fact]
    public void CanTranslateSum()
    {
        var sum =
            E("sum",
                E("const", 2),
                E("const", 3))
            .Compile(contract: () => default(int));

        var result = sum();

        Assert.Equal(5, result);
    }

    [Fact]
    public void CanExpandSum()
    {
        var expanded =
            E("sum",
                E("sum",
                    E("sum",
                        E("sum",
                            E("sum", 
                                E("const", 0), 
                                E("const", 1)),
                            E("const", 2)),
                        E("const", 3)),
                    E("const", 4)),
                E("const", 5));

        var exp = E("sum", 1, 2, 3, 4, 5).Expand();

        Assert.Equal(
            JsonConvert.SerializeObject(expanded), 
            JsonConvert.SerializeObject(exp));
    }

    [Fact]
    public void CanSumMultipleParams()
    {
        var sum = E("sum", 1, 2, 3, 4, 5)
            .Compile(contract: () => default(int));

        var result = sum();

        Assert.Equal(15, result);
    }
}
