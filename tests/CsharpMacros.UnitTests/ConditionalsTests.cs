using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using static CsharpMacros.Exp;

namespace CsharpMacros.UnitTests;

public class ConditionalsTests
{
    public ConditionalsTests()
    {
        Module.InitializeAllModules();
    }

    [Fact]
    public void CanTranslateIf()
    {
        var ifElse =
            E("if", E("const", true),
                E("const", "Yes"),
                E("const", "No"));

        var expression = ifElse.Translate();

        Assert.IsAssignableFrom<ConditionalExpression>(expression);
    }

    [Fact]
    public void CanTranslateEq()
    {
        var eq = E("eq", E("const", 3), E("const", 3));

        var exp = eq.Translate();

        Assert.IsAssignableFrom<BinaryExpression>(exp);
    }

    [Fact]
    public void CanTranslateNot()
    {
        var not = E("not", E("const", false));

        var exp = not.Translate();

        Assert.IsAssignableFrom<UnaryExpression>(exp);
    }

    [Fact]
    public void CanExpandNeq()
    {
        var expanded =
            E("not",
                E("eq", E("const", 3),
                        E("const", 4)));

        var neq = E("neq", E("const", 3), E("const", 4)).Expand();

        Assert.Equal(
            JsonConvert.SerializeObject(expanded),
            JsonConvert.SerializeObject(neq));
    }

    [Fact]
    public void CanTranslateAnd()
    {
        var and =
            E("and",
                E("eq", E("const", 2),
                        E("const", 2)),
                E("const", true));

        var exp = and.Translate();

        Assert.IsAssignableFrom<BinaryExpression>(exp);
    }
}
