using CsharpDataOriented;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CsharpMacros;

public class ValuesModule
{
    public static Expression TranslateConst(Exp constant)
    {
        var val = constant.Nth<object>(1);

        return Expression.Constant(val);
    } 

    public static Expression TranslateCast(Func<Exp, Expression> translate, Exp cast)
    {
        var type = cast.Nth<Type>(1);
        var inner = cast.Nth<Exp>(2);

        var transInner = translate(inner);

        return Expression.Convert(transInner, type);
    } 
}
