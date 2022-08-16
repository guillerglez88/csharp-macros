# csharp-macros

Exploring expressions as data and macros in c#

## E(...)

All the expression tree is built as a uniform data structure.

```
E: (object: T[]) -> Exp
```

## Functions

Composed of exp-type: "fn", first expression for arguments with even number of component for pairs: ("arg-name", arg-type) and a body expression of any type.

```
func: E("fn", args, body);

args: E("arg1-name", arg1-type,
		"arg2-name", arg2-type,
		"arg3-name", arg3-type, ...);

body: E("exp-type", exp-params...)
```

## Exambles:

```csharp
Func<Person, HumanName>> getName = (Person person) => person.Name;
```

```csharp
var getName = E("fn",
	E("person", person.GetType()),
	E("get", "Name",
		E("param", "person"))).Compile();

getName(person); // => { "Given": [ "Glen", "Ruben" ], "Family": "Rodriguez"}
```

## Supported expressions

| exp-type | desc          | expandable | example |
| -------- | ------------- | :--------: | ------- |
| "fn"     | lambda exp    |     X      |         |
| "get"    | member access |            |         |
| "cast"   | convert       |            |         |
| "param"  | fn arg ref    |     X      |         |
