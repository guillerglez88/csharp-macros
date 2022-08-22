using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpMacros;

public static class Module
{
    public static void Initialize<T>()
        where T : class, new()
        => new T();

    public static void InitializeAllModules()
    {
        Initialize<Arithmetics>();
        Initialize<Conditionals>();
        Initialize<Values>();
        Initialize<Fn>();
        Initialize<Members>();
        Initialize<List>();
        Initialize<Dict>();
    }
}
