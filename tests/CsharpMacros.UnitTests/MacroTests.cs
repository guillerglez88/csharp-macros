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
    public void CanCreateMacros()
    {
        var whenMacro =
            E("macro", "when",
                E("test", typeof(Exp),
                  "body", typeof(Exp)),
                E("if",
                    E("param", "test"),
                    E("param", "body"),
                    E("const", default(object))));

        var qWhen = Q(whenMacro);
    }
}
