# csharp-macros

Exploring expressions as data and macros in c#

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

| exp-type | desc          | expandable | example |
| -------- | ------------- | :--------: | ------- |
| "fn"     | lambda exp    |     X      |         |
| "get"    | member access |            |         |
| "cast"   | convert       |            |         |
| "param"  | fn arg ref    |     X      |         |
