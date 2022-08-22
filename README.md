# csharp-macros

Exploring expressions as data and macros in c#

## Macros

```csharp
Macro("switch", (exp, _args) => exp
    .Skip(2)
    .Partition(size: 2)
    .Where(pair => pair.Count() == 2)
    .Select(pair => new {
        test = pair.First(),
        result = pair.Last() })
    .Aggregate(exp.Nth<Exp>(-1), (acc, curr) =>
        E("if", E("eq", curr.test, exp.Nth<Exp>(1)),
            curr.result,
            acc)));
```

```csharp
var getVimMovement =
    E("fn",
        E("command", typeof(string)),
        E("switch", E("param", "command"),
            E("const", "h"), E("const", "move-left"),
            E("const", "j"), E("const", "move-down"),
            E("const", "k"), E("const", "move-up"),
            E("const", "l"), E("const", "move-left"),
            E("const", "unknown")))
    .Compile(contract: (string comand) => default(string));

getVimMovement("h"); // => "move-left"
getVimMovement("k"); // => "move-up"
``

## E(...)

All the expression tree is built as a uniform data structure. Components of an expression are of arbitrary types, depend on each specific expression type. The complete expression tree is manimulable data, so you can easely expand expression.

```
E: (object: T[]) -> Exp
```

The expression building process is:

Exp -> [expand -> translate -> compile] -> Func<...>

## Functions

Composed of exp-type: "fn", first expression for arguments with even number of components for pairs: ("arg-name", arg-type) and a body expression of any type.

```
func: E("fn", args, body);

args: E("arg1-name", arg1-type,
        "arg2-name", arg2-type,
        "arg3-name", arg3-type, ...);

body: E("exp-type", exp-params...)
```

## Examples

To build this function:

```csharp
Func<Person, HumanName> getName = (Person person) => person.Name;
```

```csharp
var getName = E("fn",
    E("person", person.GetType()),
    E("get", "Name",
        E("param", "person"))).Compile();

getName(person); // => { "Given": [ "Glen", "Ruben" ], "Family": "Rodriguez"}
```

### Exp data structure

```json
["fn",
    ["person", person.GetType()],
    ["get", "Name",
        ["param", "person"]]]
```

### Expanded

```json
["fn",
    ["param", "person", person.GetType()],
    ["get", "Name",
        ["param", "person", person.GetType()]]]
```

## Supported expressions

| exp-type | desc                          | expandable | example                                     |
| -------- | ----------------------------- | :--------: | ------------------------------------------- |
| "fn"     | lambda exp                    |     X      | ("fn", Exp, Exp)                            |
| "get"    | member access                 |            | ("get", "PropName", Exp)                    |
| "cast"   | convert                       |            | ("cast", Type, Exp)                         |
| "param"  | fn arg ref                    |     X      | ("param", "param-name")                     |
| "const"  | declare constant              |            | ("const", 5)                                |
| "sum"    | add numbers                   |     X      | ("sum", 1, 2, 3, 4, 5)                      |
| "sub"    | subtract numbers              |     X      | ("sub", 15, 1, 2, 3, 4)                     |
| "mod"    | modulo                        |     X      | ("mod", 5, 2)                               |
| "{"      | dictionary                    |     X      | ("{", "greet", "hello", "a", 1, "}")        |
| "or"     | boolean or                    |            | ("or", E("const", false), E("const", true)) |
| "and"    | boolean and                   |            | ("and", E("const", true), E("const", true)) |
| "not"    | boolean not                   |            | ("not", E("const", false))                  |
| "eq"     | boolean equal                 |            | ("eq", E("const", 2), E("const", 2))        |
| "neq"    | boolean not equal             |     X      | ("neq", E("const", 2), E("const", 3))       |
| "gt"     | boolean greater than          |            | ("gt", E("const", 5), E("const", 3))        |
| "lt"     | boolean less than             |            | ("lt", E("const", 5), E("const", 10))       |
| "geq"    | boolean greater than or equal |     X      | ("geq", E("const", 5), E("const", 5))       |
| "leq"    | boolean less than or equal    |     X      | ("leq", E("const", 5), E("const", 5))       |
