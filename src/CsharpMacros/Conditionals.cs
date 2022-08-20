using CsharpDataOriented;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using static CsharpMacros.Macros;

namespace CsharpMacros;

public class Conditionals
{
	static Conditionals()
	{
		TranslateMulti
			.DefMethod("if", TranslateIf);
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
