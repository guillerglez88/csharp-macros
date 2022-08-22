using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using static CsharpMacros.Exp;
using static CsharpMacros.Macros;

namespace CsharpMacros;

public class List
{
	static List()
	{
		TranslateMulti
			.DefMethod("list", TranslateList);
	} 

	public static Expression TranslateList(Exp array)
	{
		var expCmps = array
			.Skip(1)
			.Select(cmp => cmp is Exp exp ? E("cast", typeof(object), exp) : E("cast", typeof(object), E("const", cmp)))
			.Select(cmp => cmp.Translate());

		var exp = Expression.NewArrayInit(typeof(object), expCmps);

		return exp;
	}
}
