using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpMacros;

public class Exp : IEnumerable<object>
{
    private readonly IEnumerable<object> actualExp;

    public Exp(IEnumerable<object> actualExp)
    {
        this.actualExp = actualExp;
    }

    public IEnumerator<object> GetEnumerator()
        => actualExp.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
        => actualExp.GetEnumerator();

    public static Exp E(params object[] exp)
        => new Exp(exp);

    public override string ToString()
        => this.Stringify();
}
