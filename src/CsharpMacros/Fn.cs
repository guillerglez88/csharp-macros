using CsharpDataOriented;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using static CsharpDataOriented.BasicFuncs;
using static CsharpMacros.Macros;
using static CsharpMacros.Exp;

namespace CsharpMacros;

public class Fn
{
    static Fn()
    {
        var memoTranslateParam = Memoize<Exp, Expression>(TranslateParam);

        TranslateMulti
            .DefMethod("fn", TranslateFn)
            .DefMethod("param", memoTranslateParam);

        ExpandMulti
            .DefMethod("fn", (arg) => ExpandFn(arg.exp))
            .DefMethod("param", (arg) => ExpandParamRef(arg.exp, arg.args));
    }

    public static Exp ExpandFn(Exp fn)
    {
        var args = fn.Nth<Exp>(1)
            .Partition(2)
            .Select(param => E("param", param.Nth<string>(0), param.Nth<Type>(1)))
            .ToArray();

        var body = fn.Nth<Exp>(-1).Expand(args);
        var expanded = fn.Take(1).Concat(args).Append(body).ToArray();

        return E(expanded);
    } 

    public static Exp ExpandParamRef(Exp paramRef, IEnumerable<Exp> args)
    {
        var name = paramRef.Nth<string>(1);
        var param = args.First(arg => arg.Nth<string>(1) == name);

        return param;
    } 

    public static Expression TranslateFn(Exp fn)
    {
        var args = fn.Skip(1).Cast<Exp>().Take(fn.Count() - 2);
        var body = fn.Nth<Exp>(-1);

        var transArgs = args.Select(arg => arg.Translate()).Cast<ParameterExpression>().ToArray();
        var transBody = body.Translate();

        return Expression.Lambda(
            body: transBody,
            parameters: transArgs
        );
    }

    public static Expression TranslateParam(Exp param)
    {
        var name = param.Nth<string>(1);
        var type = param.Nth<Type>(2);

        return Expression.Parameter(type, name);
    }
}
