using CsharpDataOriented;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static CsharpMacros.Exp;
using static CsharpMacros.Macros;

namespace CsharpMacros.UnitTests;

public class MacroTests
{
    public MacroTests()
    {
        Module.InitializeAllModules();
    }

    [Fact]
    public void CanExpandQuotedExp()
    {
        var expanded =
            E("list", "sum", 1, 2, 3,
                E("list", "sum", 4, 5));

        var exp =
            E("'",
                E("sum", 1, 2, 3,
                    E("sum", 4, 5)));

        var expandedExp = exp.Expand();

        Assert.Equal(
            JsonConvert.SerializeObject(expanded),
            JsonConvert.SerializeObject(expandedExp));
    }

    [Fact]
    public void CanEvalListExpression()
    {
        var exp =
            E("list", "sum", 1, 2, 3,
                E("list", "sum", 4, 5));

        var tree = exp.Compile(contract: () => default(object[])).Invoke();
        var expCmp = tree.Select(item => typeof(object[]).IsAssignableFrom(item.GetType()) ? E((object[])item) : item);
        var revExp = E(expCmp.ToArray());
        var result = revExp.Compile(contract: () => default(int)).Invoke();

        Assert.Equal(15, result);
    }

    [Fact]
    public void CanExpandMacros()
    {
        var expanded =
            E("if",
                E("eq", 
                    E("mod", 
                        E("const", 4), 
                        E("const", 2)), 
                    E("const", 0)),
                E("const", "even"),
                E("const", default(string)));

        Macro("when", (exp, _args) =>
            E("if",
                exp.Nth<Exp>(1),
                exp.Nth<Exp>(2),
                E("const", default(object))));

        var exp =
            E("when",
                E("eq", 
                    E("mod", 
                        E("const", 4), 
                        E("const", 2)), 
                    E("const", 0)),
                E("const", "even"))
            .Expand();

        Assert.Equal(
            JsonConvert.SerializeObject(expanded),
            JsonConvert.SerializeObject(exp));
    }

    [Fact]
    public void CanUseMacros()
    {
        Macro("when", (exp, _args) =>
            E("if",
                exp.Nth<Exp>(1),
                E("cast", typeof(object), exp.Nth<Exp>(2)),
                E("const", default(object))));

        var even =
            E("when",
                E("eq", 
                    E("mod", 
                        E("const", 4), 
                        E("const", 2)), 
                    E("const", 0)),
                E("const", "even"))
            .Compile(contract: () => default(string));
        var result = even();

        Assert.Equal("even", result);
    }
}
