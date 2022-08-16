using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static CsharpMacros.ExpressionModule;

namespace CsharpMacros.UnitTests;

public class ExpressionTests
{
    private readonly object person = new
    {
        Name = new
        {
            Given = new[] { "Glen", "Ruben" },
            Family = "Rodriguez"
        },
        DoB = new DateTime(2011, 10, 3),
        Addresses = new[] {
            new {
                Lines = new[] { "184 #42301, esq 423" },
                PostalCode = "18100",
                Period = new { Start = new DateTime(2019, 04, 17) } } }
    };

    [Fact]
    public void CanBuildExp()
    {
        var getName = 
            E("fn",
                E("person", person.GetType()),
                E("get", "Name",
                    E("param", "person")))
            .Compile();

        dynamic name = getName(new[] { person });

        Assert.Equal("Rodriguez", name.Family);
    }
}
