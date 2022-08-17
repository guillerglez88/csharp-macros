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

    static Macros()
    {
        TranslateMulti = DefMulti(
            contract: (Exp exp) => default(Expression),
            dispatch: (exp) => exp.Nth<string>(0));

        ExpandMulti = 
             DefMulti(
                contract: ((Exp exp, IEnumerable<Exp> args) arg) => default(Exp),
                dispatch: (arg) => arg.exp.Cast<string>().First())
            .DefDefault((_, arg) => ExpandExp(arg.exp, arg.args)); 
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

    private static Exp ExpandExp(Exp exp, IEnumerable<Exp> args)
    {
        var expanded = exp
            .Select(cmp => cmp is Exp inner ? Expand(inner, args) : cmp)
            .ToArray();

        return E(expanded);
    } 
}
