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
    public void CanTestConditions()
    {
        var checkExp =
            E("fn",
                E("new-members", typeof(int),
                  "app-downloads", typeof(int)),
                E("or",
                    E("and",
                        E("gt",
                            E("param", "new-members"),
                            E("const", 225)),
                        E("gt",
                            E("param", "app-downloads"),
                            E("const", 10000))),
                    E("and",
                        E("le",
                            E("param", "new-members"),
                            E("const", 100)),
                        E("gt",
                            E("param", "app-downloads"),
                            E("const", 25000)))));
        var check = checkExp.Compile(contract: (int newMembers, int appDownloads) => default(bool));

        var failedLaundesLatestApp = check(5000, 3000);
        var successLaundesLatestApp = check(5000, 30000);

        Assert.False(failedLaundesLatestApp);
        Assert.True(successLaundesLatestApp);
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

    [Fact]
    public void CanTranslateOr()
    {
        var or =
            E("or",
                E("eq", E("const", 2),
                        E("const", 3)),
                E("const", true));

        var exp = or.Translate();

        Assert.IsAssignableFrom<BinaryExpression>(exp);
    }

    [Fact]
    public void CanTranslateGt()
    {
        var gt =
            E("gt", E("const", 2),
                    E("const", 1));

        var exp = gt.Translate();

        Assert.IsAssignableFrom<BinaryExpression>(exp);
    }

    [Fact]
    public void CanTranslateLt()
    {
        var lt =
            E("lt", E("const", 2),
                    E("const", 1));

        var exp = lt.Translate();

        Assert.IsAssignableFrom<BinaryExpression>(exp);
    }
}
