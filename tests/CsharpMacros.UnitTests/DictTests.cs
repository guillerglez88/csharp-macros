using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using static CsharpMacros.Exp;
using static CsharpMacros.Dict;

namespace CsharpMacros.UnitTests;

public class DictTests
{
    public DictTests()
    {
        Module.InitializeAllModules();
    }

    [Fact]
    public void CanTranslateDict()
    {
        var dict =
            D("a", E("const", 1),
              "b", E("const", 2),
              "greet", E("const", "hello"));

        var exp = TranslateDict(dict);

        Assert.IsAssignableFrom<MemberInitExpression>(exp);
    }
}
