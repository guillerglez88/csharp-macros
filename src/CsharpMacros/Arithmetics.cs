using CsharpDataOriented;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using static CsharpMacros.Macros;
using static CsharpMacros.Exp;

namespace CsharpMacros;

public class Arithmetics
{
    static Arithmetics()
    {
        TranslateMulti
            .DefMethod("+", (exp) => TranslateSum(exp));

        ExpandMulti
            .DefMethod("+", (arg) => ExpandSum((exp) => exp.Expand(arg.args), arg.exp));
    }

    public static Exp ExpandSum(Func<Exp, Exp> expand, Exp sum)
    {
        var expanded = sum.Skip(1)
            .Select(arg => arg is Exp expArg ? expand(expArg) : E("const", arg))
            .Aggregate(E("const", 0), (acc, curr) => E("+", acc, curr));

        return expanded;
    }

    public static Expression TranslateSum(Exp sum)
    {
        var left = sum.Nth<Exp>(1).Translate();
        var right = sum.Nth<Exp>(2).Translate();

        return Expression.Add(left, right);
    }
}
