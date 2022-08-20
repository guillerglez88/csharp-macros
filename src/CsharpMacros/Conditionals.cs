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
            .DefMethod("if", TranslateIf);

        ExpandMulti
            .DefMethod("if", (arg) => ExpandIf(arg.exp, arg.args));
    }

    public static Exp ExpandIf(Exp ifElse, IEnumerable<Exp> args)
    {
        var expandedComponents = ifElse
            .Skip(1)
            .Select(cmp => cmp is Exp exp ? exp.Expand(args) : E("const", cmp));

        var expanded = E(new object[] { "if" }.Concat(expandedComponents).ToArray());

        return expanded;
    }

    public static Expression TranslateIf(Exp ifElse)
    {
        var test = ifElse.Nth<Exp>(1).Translate();
        var ifTrue = ifElse.Nth<Exp>(2).Translate();
        var ifFalse = ifElse.Nth<Exp>(3).Translate();

        var exp = Expression.Condition(test, ifTrue, ifFalse);

        return exp;
    }
}
