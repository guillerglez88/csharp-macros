using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static CsharpMacros.Exp;

namespace CsharpMacros.UnitTests;

public class FnTests
{
    public FnTests()
    {
        Module.InitializeAllModules();
    }

    [Fact]
    public void CanBuildFn()
    {
        var sum = 
            E("fn",
                E("a", typeof(int),
                  "b", typeof(int)),
                E("sum", 
                    E("param", "a"),
                    E("param", "b")))
            .Compile(contract: (int a, int b) => default(int));

        var result = sum(2, 6);

        Assert.Equal(8, result);
    }
}
