using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using static CsharpMacros.Exp;

namespace CsharpMacros.UnitTests;

public class ArithmeticsTests
{
    public ArithmeticsTests()
    {
        Module.InitializeAllModules();
    }

    [Fact]
    public void CanCalcMod()
    {
        var mod =
            E("mod", E("const", 5),
                     E("const", 2))
            .Compile(contract: () => default(int));

        var result = mod();

        Assert.Equal(1, result);
    }

    [Fact]
    public void CanExpandMod()
    {
        var expanded =
            E("mod", E("const", 5),
                     E("const", 2));

        var mod = E("mod", 5, 2).Expand();

        Assert.Equal(
            JsonConvert.SerializeObject(expanded),
            JsonConvert.SerializeObject(mod));
    }

    [Fact]
    public void CanTranslateMod()
    {
        var mod =
            E("mod", E("const", 5),
                     E("const", 2));

        var exp = mod.Translate();

        Assert.IsAssignableFrom<BinaryExpression>(exp);
    }
}
