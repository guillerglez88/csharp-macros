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
