using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static CsharpMacros.ExpModule;
using static CsharpMacros.MembersModule;

namespace CsharpMacros.UnitTests;

public class MembersTests
{
    private readonly Person person = new
    (
        Name: new(
            Given: new[] { "Glen", "Ruben" },
            Family: "Rodriguez"),
        DoB: new DateTime(2011, 10, 3),
        Addresses: new[] {
            new Address (
                Lines: new[] { "184 #42301, esq 423" },
                PostalCode: "18100",
                Validity: new Period( Start: new DateTime(2019, 04, 17) ) ) });

    [Fact]
    public void CanAccessProp()
    {
        var getName =
            E("fn",
                E("person", typeof(Person)),
                E("get", "Name",
                    E("param", "person")))
            .Compile(contract: (Person person) => default(HumanName));

        var name = getName(person);

        Assert.Equal("Rodriguez", name.Family);
    }
}
