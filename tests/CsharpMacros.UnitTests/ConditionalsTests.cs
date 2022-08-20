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
    public void CanExpandIf()
    {
        var expanded =
            E("if", E("const", false),
                E("const", "yes"),
                E("const", "no"));

        var exp = E("if", false, "yes", "no").Expand();

        Assert.Equal(
            JsonConvert.SerializeObject(expanded),
            JsonConvert.SerializeObject(exp));
    }
}
