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
            .DefMethod("ge", TranslateGeq)
            .DefMethod("lt", TranslateLt)
            .DefMethod("le", TranslateLeq);

        ExpandMulti
            .DefMethod("neq", (arg) => ExpandNeq(arg.exp, arg.args));

        StringifyMulti
            .DefMethod("if", StringifyIf)
            .DefMethod("eq", StringifyEq)
            .DefMethod("not", StringifyNot)
            .DefMethod("and", StringifyAnd)
            .DefMethod("or", StringifyOr)
            .DefMethod("gt", StringifyGt)
            .DefMethod("ge", StringifyGeq)
            .DefMethod("lt", StringifyLt)
            .DefMethod("le", StringifyLeq);
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

        var exp = Expression.Or(left, right);

        return exp;
    }

    public static Expression TranslateGt(Exp gt)
    {
        var left = gt.Nth<Exp>(1).Translate();
        var right = gt.Nth<Exp>(2).Translate();

        var exp = Expression.GreaterThan(left, right);

        return exp;
    }

    public static Expression TranslateLt(Exp lt)
    {
        var left = lt.Nth<Exp>(1).Translate();
        var right = lt.Nth<Exp>(2).Translate();

        var exp = Expression.LessThan(left, right);

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


    public static Expression TranslateGeq(Exp gt)
    {
        var left = gt.Nth<Exp>(1).Translate();
        var right = gt.Nth<Exp>(2).Translate();

        var exp = Expression.GreaterThanOrEqual(left, right);

        return exp;
    }

    public static Expression TranslateLeq(Exp gt)
    {
        var left = gt.Nth<Exp>(1).Translate();
        var right = gt.Nth<Exp>(2).Translate();

        var exp = Expression.LessThanOrEqual(left, right);

        return exp;
    }


    public static string StringifyIf(Exp exp)
    {
        var strCond = exp.Nth<Exp>(1).Stringify();
        var strIfTrue = exp.Nth<Exp>(2).Stringify();
        var strIfFalse = exp.Nth<Exp>(3).Stringify();

        return $"if ({strCond})\n    ({strIfTrue})\nelse    ({strIfFalse})";
    }

    public static string StringifyLeq(Exp exp)
    {
        var strLeft = exp.Nth<Exp>(1).Stringify();
        var strRight = exp.Nth<Exp>(2).Stringify();

        return $"{strLeft} <= {strRight}";
    }

    public static string StringifyLt(Exp exp)
    {
        var strLeft = exp.Nth<Exp>(1).Stringify();
        var strRight = exp.Nth<Exp>(2).Stringify();

        return $"{strLeft} < {strRight}";
    }

    public static string StringifyGeq(Exp exp)
    {
        var strLeft = exp.Nth<Exp>(1).Stringify();
        var strRight = exp.Nth<Exp>(2).Stringify();

        return $"{strLeft} >= {strRight}";
    }

    public static string StringifyGt(Exp exp)
    {
        var strLeft = exp.Nth<Exp>(1).Stringify();
        var strRight = exp.Nth<Exp>(2).Stringify();

        return $"{strLeft} > {strRight}";
    }

    public static string StringifyOr(Exp exp)
    {
        var strLeft = exp.Nth<Exp>(1).Stringify();
        var strRight = exp.Nth<Exp>(2).Stringify();

        return $"({strLeft}) or ({strRight})";
    }

    public static string StringifyAnd(Exp exp)
    {
        var strLeft = exp.Nth<Exp>(1).Stringify();
        var strRight = exp.Nth<Exp>(2).Stringify();

        return $"({strLeft}) and ({strRight})";
    }

    public static string StringifyNot(Exp exp)
    {
        var strExp = exp.Nth<Exp>(1).Stringify();

        return $"not ({strExp})";
    }

    public static string StringifyEq(Exp exp)
    {
        var strLeft = exp.Nth<Exp>(1).Stringify();
        var strRight = exp.Nth<Exp>(2).Stringify();

        return $"{strLeft} = {strRight}";
    }
}
