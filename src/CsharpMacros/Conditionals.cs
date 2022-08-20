using CsharpDataOriented;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using static CsharpMacros.Exp;
using static CsharpMacros.Macros;

namespace CsharpMacros;

public class Conditionals
{
    static Conditionals()
    {
        TranslateMulti
            .DefMethod("if", TranslateIf)
            .DefMethod("eq", TranslateEq)
            .DefMethod("not", TranslateNot)
            .DefMethod("and", TranslateAnd)
            .DefMethod("or", TranslateOr)
            .DefMethod("gt", TranslateGt)
            .DefMethod("lt", TranslateLt);

        ExpandMulti
            .DefMethod("neq", (arg) => ExpandNeq(arg.exp, arg.args))
            .DefMethod("geq", (arg) => ExpandGeq(arg.exp, arg.args))
            .DefMethod("leq", (arg) => ExpandLeq(arg.exp, arg.args));
    }

    public static Expression TranslateIf(Exp ifElse)
    {
        var test = ifElse.Nth<Exp>(1).Translate();
        var ifTrue = ifElse.Nth<Exp>(2).Translate();
        var ifFalse = ifElse.Nth<Exp>(3).Translate();

        var exp = Expression.Condition(test, ifTrue, ifFalse);

        return exp;
    }

    public static Expression TranslateEq(Exp eq)
    {
        var left = eq.Nth<Exp>(1).Translate();
        var right = eq.Nth<Exp>(2).Translate();

        var exp = Expression.Equal(left, right);

        return exp;
    }

    public static Expression TranslateNot(Exp not)
    {
        var arg = not.Nth<Exp>(1).Translate();

        var exp = Expression.Not(arg);

        return exp;
    }

    public static Expression TranslateAnd(Exp and)
    {
        var left = and.Nth<Exp>(1).Translate();
        var right = and.Nth<Exp>(2).Translate();

        var exp = Expression.And(left, right);

        return exp;
    }

    public static Expression TranslateOr(Exp or)
    {
        var left = or.Nth<Exp>(1).Translate();
        var right = or.Nth<Exp>(2).Translate();

        var exp = Expression.And(left, right);

        return exp;
    }

    public static Expression TranslateGt(Exp gt)
    {
        var left = gt.Nth<Exp>(1).Translate();
        var right = gt.Nth<Exp>(2).Translate();

        var exp = Expression.And(left, right);

        return exp;
    }

    public static Expression TranslateLt(Exp lt)
    {
        var left = lt.Nth<Exp>(1).Translate();
        var right = lt.Nth<Exp>(2).Translate();

        var exp = Expression.And(left, right);

        return exp;
    }

    public static Exp ExpandNeq(Exp neq, IEnumerable<Exp> args)
    {
        var expandedCmps = neq
            .Skip(1)
            .Select(cmp => cmp is Exp exp ? exp.Expand(args) : cmp);

        var expanded = E("not", E(new object[] { "eq" }.Concat(expandedCmps).ToArray()));

        return expanded;
    }

    public static Exp ExpandGeq(Exp geq, IEnumerable<Exp> args)
    {
        var expandedCmps = geq
            .Skip(1)
            .Select(cmp => cmp is Exp exp ? exp.Expand(args) : cmp);

        var expanded = 
            E("or", 
                E(new object[] { "gt" }.Concat(expandedCmps).ToArray()),
                E(new object[] { "eq" }.Concat(expandedCmps).ToArray()));

        return expanded;
    }

    public static Exp ExpandLeq(Exp leq, IEnumerable<Exp> args)
    {
        var expandedCmps = leq
            .Skip(1)
            .Select(cmp => cmp is Exp exp ? exp.Expand(args) : cmp);

        var expanded = 
            E("or", 
                E(new object[] { "lt" }.Concat(expandedCmps).ToArray()),
                E(new object[] { "eq" }.Concat(expandedCmps).ToArray()));

        return expanded;
    }
}
