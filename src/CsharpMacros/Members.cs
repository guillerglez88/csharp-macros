using CsharpDataOriented;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using static CsharpMacros.Macros;
namespace CsharpMacros;

public class Members
{
    static Members()
    {
        TranslateMulti
            .DefMethod("get", TranslateGet);

        StringifyMulti
            .DefMethod("get", StringifyGet);
    }

    public static Expression TranslateGet(Exp get)
    {
        var name = get.Nth<string>(1);
        var inner = get.Nth<Exp>(2);

        var transInner = inner.Translate();

        var prop = transInner.Type.GetProperty(name, BindingFlags.Public | BindingFlags.Instance);

        return Expression.MakeMemberAccess(transInner, prop);
    }

    public static string StringifyGet(Exp get)
    {
        var name = get.Nth<string>(1);
        var strInner = get.Nth<Exp>(2).Stringify();

        return $"{strInner}.{name}";
    }
}
