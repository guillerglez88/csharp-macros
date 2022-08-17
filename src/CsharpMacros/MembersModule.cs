using CsharpDataOriented;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CsharpMacros;

public static class MembersModule
{
    public static Expression TranslateGet(Func<Exp, Expression> translate, Exp get)
    {
        var name = get.Nth<string>(1);
        var inner = get.Nth<Exp>(2);

        var transInner = translate(inner);

        var prop = transInner.Type.GetProperty(name, BindingFlags.Public | BindingFlags.Instance);

        return Expression.MakeMemberAccess(transInner, prop);
    }
}
