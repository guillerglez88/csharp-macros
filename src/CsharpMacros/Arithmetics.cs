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
            .DefMethod("sum", (exp) => TranslateSum(exp))
            .DefMethod("sub", (exp) => TranslateSub(exp))
            .DefMethod("mod", (exp) => TranslateMod(exp));

        ExpandMulti
            .DefMethod("sum", (arg) => ExpandSum(arg.exp, arg.args))
            .DefMethod("sub", (arg) => ExpandSub(arg.exp, arg.args))
            .DefMethod("mod", (arg) => ExpandMod(arg.exp, arg.args));
    }

    public static Expression TranslateSum(Exp sum)
    {
        var left = sum.Nth<Exp>(1).Translate();
        var right = sum.Nth<Exp>(2).Translate();

        return Expression.Add(left, right);
    }

    public static Expression TranslateSub(Exp sum)
    {
        var left = sum.Nth<Exp>(1).Translate();
        var right = sum.Nth<Exp>(2).Translate();

        return Expression.Subtract(left, right);
    }

    public static Expression TranslateMod(Exp mod)
    {
        var left = mod.Nth<Exp>(1).Translate();
        var right = mod.Nth<Exp>(2).Translate();

        return Expression.Modulo(left, right);
    }

    public static Exp ExpandSum(Exp sum, IEnumerable<Exp> args)
    {
        var expanded = sum.Skip(1)
            .Select(cmp => cmp is Exp exp ? exp.Expand(args) : E("const", cmp))
            .Aggregate(E("const", 0), (acc, curr) => E("sum", acc, curr));

        return expanded;
    }

    public static Exp ExpandSub(Exp sum, IEnumerable<Exp> args)
    {
        var expanded = sum.Skip(1)
            .Select(cmp => cmp is Exp exp ? exp.Expand(args) : E("const", cmp))
            .Aggregate((acc, curr) => E("sub", acc, curr));

        return expanded;
    }

    public static Exp ExpandMod(Exp sum, IEnumerable<Exp> args)
    {
        var expandedCmps = sum.Skip(1)
            .Select(cmp => cmp is Exp exp ? exp.Expand(args) : E("const", cmp));

        var expanded = E(new object[] { "mod" }.Concat(expandedCmps).ToArray());

        return expanded;
    }
}
