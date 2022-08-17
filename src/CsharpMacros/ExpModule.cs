using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpMacros;

public static class ExpModule
{
    public static Exp E(params object[] exp)
        => new Exp(exp); 
}
