using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CsharpMacros.UnitTests;

public record Person(
    HumanName Name,
    DateTime DoB,
    IEnumerable<Address> Addresses);

public record HumanName(
    IEnumerable<string> Given,
    string Family);

public record Address(
    IEnumerable<string> Lines,
    string PostalCode,
    Period Validity);

public record Period(
    DateTime? Start = null,
    DateTime? End = null);
