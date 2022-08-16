using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using static CsharpMacros.ExpressionModule;

namespace CsharpMacros.UnitTests;

public class LetTests
{
    [Fact]
    public void CanTranslateConst()
    {
        var value = E("const", 5)
            .Compile(contract: () => default(int));

        var result = value();

        Assert.Equal(5, result);
    }
}
