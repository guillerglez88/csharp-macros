using System.Linq.Expressions;
using System.Reflection;

using static CsharpMultimethod.Multi;
using static CsharpDataOriented.CollectionsModule;
using static CsharpMacros.Exp;
using static CsharpMacros.Members;
using CsharpMultimethod;

namespace CsharpMacros;

public static class Macros
{
    public static readonly Multi<Exp, string, Expression?> TranslateMulti;
    public static readonly Multi<(Exp exp, IEnumerable<Exp> args), string, Exp?> ExpandMulti;
    public static readonly Multi<Exp, string, string?> StringifyMulti;

    static Macros()
    {
        TranslateMulti = DefMulti(
            contract: (Exp exp) => default(Expression),
            dispatch: (exp) => exp.Nth<string>(0));

        ExpandMulti = DefMulti(
            contract: ((Exp exp, IEnumerable<Exp> args) arg) => default(Exp),
            dispatch: (arg) => arg.exp.Nth<string>(0));

        StringifyMulti = DefMulti(
            contract: (Exp exp) => default(string),
            dispatch: (exp) => exp.Nth<string>(0));

        ExpandMulti
            .DefMethod("'", (arg) => ExpandQ(arg.exp))
            .DefDefault((_, arg) => ExpandExp(arg.exp, arg.args));

        StringifyMulti
            .DefMethod("'", (exp) => $"'{StringifyQ(E(exp.Skip(1).ToArray()))}");
    }

    public static Func<object[], object> Compile(
        this Exp exp)
    {
        var expanded = exp.Expand();
        var expression = Translate(expanded);
        var lambda = expression is LambdaExpression lambdaExp ? lambdaExp : Expression.Lambda(expression);
        var invocable = lambda.Compile();

        return (args) => invocable.DynamicInvoke(args);
    }

    public static Exp Expand(this Exp exp, IEnumerable<Exp> args = default)
        => ExpandMulti.Invoke((exp, args));

    public static Expression Translate(this Exp exp)
        => TranslateMulti.Invoke(exp);

    public static string Stringify(this Exp exp)
        => StringifyMulti.Invoke(exp);

    public static Exp Q(object exp)
        => E(new object[] { "'", exp }); 

    public static void Macro(string name, Func<Exp, IEnumerable<Exp>, Exp> macro)
        => ExpandMulti.DefMethod(name, (arg) => macro(arg.exp, arg.args).Expand(arg.args));

    private static string StringifyQ(Exp q)
    {
        var strCmps = q.Select(cmp => cmp is Exp exp ? $"({StringifyQ(exp)})" : $"{cmp ?? "null"}");

        return $"{string.Join(", ", strCmps)}\n";
    }

    private static Exp ExpandExp(Exp exp, IEnumerable<Exp> args)
    {
        var expandedComponents = exp
            .Skip(1)
            .Select(cmp => cmp is Exp inner ? Expand(inner, args) : cmp)
            .ToArray();

        var expanded = E(new[] { exp.First() }.Concat(expandedComponents).ToArray());

        return expanded;
    }

    private static Exp ExpandQ(Exp q)
    {
        if (q.Count() > 2)
            throw new ArgumentException("Quote exp admites only 2 items, the quote literal('), and the quoted exp");

        var exp = q.Nth<Exp>(-1);
        var cmps = exp
            .Select(cmp => cmp is Exp exp ? Q(exp).Expand() : cmp)
            .ToArray();

        return E(new object[] { "list" }.Concat(cmps).ToArray());
    }
}
