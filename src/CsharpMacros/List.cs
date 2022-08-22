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

		ExpandMulti
			.DefMethod("'", (arg) => ExpandQ(arg.exp));
	}

	public static Exp ExpandQ(Exp q)
	{
		if (q.Count() > 2)
			throw new ArgumentException("Quote exp admites only 2 items, the quote literal('), and the quoted exp");

		var cmp = q.Last();
		var exp = cmp is Exp e ? Q(e).Expand() : E("const", cmp);

		return E("list", exp);
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
