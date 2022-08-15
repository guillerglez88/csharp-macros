# csharp-macros
Exploring expressions as data and macros in c#

```csharp
Expression<Func<object, object?>> getValue = (object obj) => (object)(((Person)obj).Name);

var getValue = Expression
    .Lambda<Func<object, object?>>(Expression
        .Convert(Expression
            .MakeMemberAccess(Expression
                .Convert(Expression
                    .Parameter(typeof(object), "obj"), 
                    prop.DeclaringType), 
                prop), 
            typeof(object)), Expression
        .Parameter(typeof(object), "obj"));
```

```json
["fn", 
    ["obj", typeof(object)],
    ["cast", typeof(object), 
        ["get", "Name",
            ["cast", typeof(Person), 
                ["param", "obj"]]]]]
```

```csharp
var getValue = 
    seq("fn", 
        seq("obj", typeof(object)),
        seq("cast", typeof(object), 
            seq("get", "Name", 
                seq("cast", typeof(Person),
                    seq("param", "obj" )))))
```

