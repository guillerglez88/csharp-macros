using System.Linq.Expressions;
using System.Reflection;

using static CsharpMultimethod.MultimethodsModule;
using static CsharpDataOriented.CollectionsModule;
using static CsharpDataOriented.BasicFuncs;

namespace CsharpMacros;

public static class ExpressionModule
{
    private static readonly Func<Exp, Expression?> translate;
    private static readonly Func<Exp, IEnumerable<Exp>, Exp> expand;

    static ExpressionModule()
    {
        var memoTranslateParam = Memoize<Exp, Expression>(TranslateParam);

        var translateMulti = DefMulti(
                contract: (Exp exp) => default(Expression),
                dispatch: (exp) => exp.First())
            .DefMethod("fn", TranslateFn)
            .DefMethod("param", memoTranslateParam)
            .DefMethod("cast", TranslateCast)
            .DefMethod("get", TranslateGet);

        var expandMulti = DefMulti(
                contract: ((Exp exp, IEnumerable<Exp> args) arg) => default(Exp),
                dispatch: (arg) => arg.exp.Cast<string>().First())
            .DefMethod("fn", (arg) => ExpandFn(arg.exp))
            .DefMethod("param", (arg) => ExpandParamRef(arg.exp, arg.args))
            .DefDefault((_, arg) => ExpandExp(arg.exp, arg.args));

        translate = translateMulti.Invoke;
        expand = (exp, args) => expandMulti.Invoke((exp, args));
    } 

    public static Exp E(params object[] exp)
        => new Exp(exp);

    public static Func<object, object> Compile(this Exp fn)
    {
        var expanded = fn.Expand();
        var exp = translate(expanded);
        var lambda = ((LambdaExpression)exp).Compile();

        return (arg) => lambda.DynamicInvoke(arg);
    }

    public static Exp Expand(this Exp exp, IEnumerable<Exp> args = default)
        => expand(exp, args);

    private static Exp ExpandFn(Exp fn)
    {
        var args = fn.Nth<Exp>(1)
            .Partition(2)
            .Select(param => E("param", param.Cast<string>().First(), param.Skip(1).Cast<Type>().First()))
            .ToArray();

        var body = fn.Nth<Exp>(-1).Expand(args);
        var expanded = fn.Take(1).Concat(args).Append(body).ToArray();

        return E(expanded);
    }

    private static Exp ExpandParamRef(Exp paramRef, IEnumerable<Exp> args)
    {
        var name = paramRef.Nth<string>(1);
        var param = args.First(arg => arg.Nth<string>(1) == name);

        return param;
    }

    private static Exp ExpandExp(Exp exp, IEnumerable<Exp> args)
    {
        var expanded = exp
            .Select(cmp => cmp is Exp inner ? Expand(inner, args) : cmp)
            .ToArray();

        return E(expanded);
    }

    private static Expression TranslateFn(Exp fn)
    {
        var args = fn.Skip(1).Cast<Exp>().Take(fn.Count() - 2);
        var body = fn.Nth<Exp>(-1);

        var transArgs = args.Select(translate).Cast<ParameterExpression>().ToArray();
        var transBody = translate(body);

        return Expression.Lambda(
            body: transBody,
            parameters: transArgs
        );
    }

    private static Expression TranslateParam(Exp param)
    {
        var name = param.Nth<string>(1);
        var type = param.Nth<Type>(2);

        return Expression.Parameter(type, name);
    }

    private static Expression TranslateGet(Exp get)
    {
        var name = get.Nth<string>(1);
        var inner = get.Nth<Exp>(2);

        var transInner = translate(inner);

        var prop = transInner.Type.GetProperty(name, BindingFlags.Public | BindingFlags.Instance);

        return Expression.MakeMemberAccess(transInner, prop);
    }

    private static Expression TranslateCast(Exp cast)
    {
        var type = cast.Nth<Type>(1);
        var inner = cast.Nth<Exp>(2);

        var transInner = translate(inner);

        return Expression.Convert(transInner, type);
    }
}
