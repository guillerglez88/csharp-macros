using CsharpDataOriented;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using static CsharpMacros.ExpModule;

namespace CsharpMacros;

public static class ArithmeticsModule
{
    public static Exp ExpandSum(Func<Exp, Exp> expand, Exp sum)
    {
        var expanded = sum.Skip(1)
            .Select(arg => arg is Exp expArg ? expand(expArg) : E("const", arg))
            .Aggregate(E("const", 0), (acc, curr) => E("sum", acc, curr));

        return expanded;
    }

    public static Expression TranslateSum(Func<Exp, Expression> translate, Exp sum)
    {
        var left = translate(sum.Nth<Exp>(1));
        var right = translate(sum.Nth<Exp>(2));

        return Expression.Add(left, right);
    }
}
