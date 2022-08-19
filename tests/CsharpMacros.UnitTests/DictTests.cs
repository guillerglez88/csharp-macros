using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

using static CsharpMacros.Exp;
using static CsharpMacros.Dict;
using Newtonsoft.Json;

namespace CsharpMacros.UnitTests;

public class DictTests
{
    public DictTests()
    {
        Module.InitializeAllModules();
    }

    [Fact]
    public void CanBuildDict()
    {
        var builPerson =
            D("name", D( 
                "first", "John",
                "last", "Doe"),
              "address", D( 
                "line", "Park Ave. #236",
                "postal-code", 53765,
                "validity", D( 
                  "start", new DateTime(2022, 01, 01))))
            .Compile(contract: () => default(dynamic));

        var person = builPerson();

        Assert.Equal(53765, person.Prop2.Prop2);
    }

    [Fact]
    public void CanExpandDict()
    {
        var expanded =
            E("{", "one", E("const", 1),
                   "two", E("const", 2),
                   "greet", E("const", "hello"), "}");

        var dict =
            D("one", 1,
              "two", 2,
              "greet", "hello").Expand();

        Assert.Equal(
            JsonConvert.SerializeObject(expanded),
            JsonConvert.SerializeObject(dict));
    }

    [Fact]
    public void CanDeepExpandDict()
    {
        var expanded =
            E("{", "name", E("{", 
                       "first", E("const", "John"),
                       "last", E("const", "Doe"), "}"),
                    "greet", E("const", "hello"), "}");

        var dict =
            D("name", D(
                "first", "John",
                "last", "Doe"),
              "greet", "hello").Expand();

        Assert.Equal(
            JsonConvert.SerializeObject(expanded),
            JsonConvert.SerializeObject(dict));
    }

    [Fact]
    public void CanTranslateDict()
    {
        var dict =
            D("a", E("const", 1),
              "b", E("const", 2),
              "greet", E("const", "hello"));

        var exp = TranslateDict(dict);

        Assert.IsAssignableFrom<MemberInitExpression>(exp);
    }
}
