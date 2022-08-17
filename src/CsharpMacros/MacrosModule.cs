using System.Linq.Expressions;
using System.Reflection;

using static CsharpMultimethod.MultimethodsModule;
using static CsharpDataOriented.CollectionsModule;
using static CsharpDataOriented.BasicFuncs;
using static CsharpMacros.ArithmeticsModule;
using static CsharpMacros.FnModule;
using static CsharpMacros.ExpModule;
using static CsharpMacros.MembersModule;
using static CsharpMacros.ValuesModule;

namespace CsharpMacros;

public static class MacrosModule
{
    private static readonly Func<Exp, Expression?> translate;
    private static readonly Func<Exp, IEnumerable<Exp>, Exp> expand;

    static MacrosModule()
    {
        var memoTranslateParam = Memoize<Exp, Expression>((exp) => TranslateParam(translate, exp));

        var translateMulti = DefMulti(
                contract: (Exp exp) => default(Expression),
                dispatch: (exp) => exp.First())
            .DefMethod("fn", (exp) => TranslateFn(translate, exp))
            .DefMethod("param", memoTranslateParam)
            .DefMethod("const", TranslateConst)
            .DefMethod("cast", (exp) => TranslateCast(translate, exp))
            .DefMethod("get", (exp) => TranslateGet(translate, exp))
            .DefMethod("sum", (exp) => TranslateSum(translate, exp));

        var expandMulti = DefMulti(
                contract: ((Exp exp, IEnumerable<Exp> args) arg) => default(Exp),
                dispatch: (arg) => arg.exp.Cast<string>().First())
            .DefMethod("fn", (arg) => ExpandFn(arg.exp))
            .DefMethod("param", (arg) => ExpandParamRef(arg.exp, arg.args))
            .DefMethod("sum", (arg) => ExpandSum((exp) => exp.Expand(arg.args), arg.exp))
            .DefDefault((_, arg) => ExpandExp(arg.exp, arg.args));

        translate = translateMulti.Invoke;
        expand = (exp, args) => expandMulti.Invoke((exp, args));
    } 

    public static Func<object[], object> Compile(
        this Exp exp)
    {
        var expanded = exp.Expand();
        var expression = translate(expanded);
        var lambda = expression is LambdaExpression lambdaExp ? lambdaExp : Expression.Lambda(expression);
        var invocable = lambda.Compile();

        return (args) => invocable.DynamicInvoke(args);
    }

    public static Exp Expand(this Exp exp, IEnumerable<Exp> args = default)
        => expand(exp, args);

    public static Expression Translate(this Exp exp)
        => translate(exp); 

    private static Exp ExpandExp(Exp exp, IEnumerable<Exp> args)
    {
        var expanded = exp
            .Select(cmp => cmp is Exp inner ? Expand(inner, args) : cmp)
            .ToArray();

        return E(expanded);
    } 
}
