using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpMacros;

public static class CompileExtensions
{
    public static Func<T> Compile<T>(this Exp exp,
        Func<T> contract)
    {
        var compiled = exp.Compile();

        return () => (T)compiled.Invoke(arg: null);
    }

    public static Func<T, W> Compile<T, W>(this Exp exp,
        Func<T, W> contract)
    {
        var compiled = exp.Compile();

        return (t) => (W)compiled.Invoke(new object[] { t });
    }

    public static Func<T1, T2, W> Compile<T1, T2, W>(this Exp exp,
        Func<T1, T2, W> contract)
    {
        var compiled = exp.Compile();

        return (t1, t2) => (W)compiled.Invoke(new object[] { t1, t2 });
    }

    public static Func<T1, T2, T3, W> Compile<T1, T2, T3, W>(this Exp exp,
        Func<T1, T2, T3, W> contract)
    {
        var compiled = exp.Compile();

        return (t1, t2, t3) => (W)compiled.Invoke(new object[] { t1, t2, t3 });
    }

    public static Func<T1, T2, T3, T4, W> Compile<T1, T2, T3, T4, W>(this Exp exp,
        Func<T1, T2, T3, T4, W> contract)
    {
        var compiled = exp.Compile();

        return (t1, t2, t3, t4) => (W)compiled.Invoke(new object[] { t1, t2, t3, t4 });
    }

    public static Func<T1, T2, T3, T4, T5, W> Compile<T1, T2, T3, T4, T5, W>(this Exp exp,
        Func<T1, T2, T3, T4, T5, W> contract)
    {
        var compiled = exp.Compile();

        return (t1, t2, t3, t4, t5) => (W)compiled.Invoke(new object[] { t1, t2, t3, t4, t5 });
    }

    public static Func<T1, T2, T3, T4, T5, T6, W> Compile<T1, T2, T3, T4, T5, T6, W>(this Exp exp,
        Func<T1, T2, T3, T4, T5, T6, W> contract)
    {
        var compiled = exp.Compile();

        return (t1, t2, t3, t4, t5, t6) => (W)compiled.Invoke(new object[] { t1, t2, t3, t4, t5, t6 });
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, W> Compile<T1, T2, T3, T4, T5, T6, T7, W>(this Exp exp,
        Func<T1, T2, T3, T4, T5, T6, T7, W> contract)
    {
        var compiled = exp.Compile();

        return (t1, t2, t3, t4, t5, t6, t7) => (W)compiled.Invoke(new object[] { t1, t2, t3, t4, t5, t6, t7 });
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, W> Compile<T1, T2, T3, T4, T5, T6, T7, T8, W>(this Exp exp,
        Func<T1, T2, T3, T4, T5, T6, T7, T8, W> contract)
    {
        var compiled = exp.Compile();

        return (t1, t2, t3, t4, t5, t6, t7, t8) => (W)compiled.Invoke(new object[] { t1, t2, t3, t4, t5, t6, t7, t8 });
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, W> Compile<T1, T2, T3, T4, T5, T6, T7, T8, T9, W>(this Exp exp,
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, W> contract)
    {
        var compiled = exp.Compile();

        return (t1, t2, t3, t4, t5, t6, t7, t8, t9) => (W)compiled.Invoke(new object[] { t1, t2, t3, t4, t5, t6, t7, t8, t9 });
    }

    public static Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, W> Compile<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, W>(this Exp exp,
        Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, W> contract)
    {
        var compiled = exp.Compile();

        return (t1, t2, t3, t4, t5, t6, t7, t8, t9, t10) => (W)compiled.Invoke(new object[] { t1, t2, t3, t4, t5, t6, t7, t8, t9, t10 });
    }
}
