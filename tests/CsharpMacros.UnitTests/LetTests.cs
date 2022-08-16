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
    public void CanTranslateLet()
    {
        var exp = E("const", 5).Translate();

        var result = Expression.Lambda<Func<int>>(exp).Compile().Invoke();

        Assert.Equal(5, result);
    }
}
