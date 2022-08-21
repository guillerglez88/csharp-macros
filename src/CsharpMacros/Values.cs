using CsharpDataOriented;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using static CsharpMacros.Macros;

namespace CsharpMacros;

public class Values
{
    static Values()
    {
        TranslateMulti
            .DefMethod("const", TranslateConst)
            .DefMethod("cast", TranslateCast);
        
        StringifyMulti
            .DefMethod("const", StringifyConst)
            .DefMethod("cast", StringifyCast);
    }

    public static Expression TranslateConst(Exp constant)
    {
        var val = constant.Nth<object>(1);

        return Expression.Constant(val);
    } 

    public static Expression TranslateCast(Exp cast)
    {
        var type = cast.Nth<Type>(1);
        var inner = cast.Nth<Exp>(2);

        var transInner = inner.Translate();

        return Expression.Convert(transInner, type);
    } 

    public static string StringifyCast(Exp cast)
    {
        var type = cast.Nth<Type>(1);
        var strInner = cast.Nth<Exp>(2).Stringify();

        return $"({strInner} as {type.Name})";
    }

    public static string StringifyConst(Exp constant)
    {
        var val = constant.Nth<object>(1);

        return $"{val}";
    }
}
