using CsharpDataOriented;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using static CsharpMacros.Exp;
using static CsharpMacros.Macros;

namespace CsharpMacros;

public class Dict
{
    static Dict()
    {
        TranslateMulti.DefMethod("{", TranslateDict);

        ExpandMulti.DefMethod("{", (arg) => ExpandDict(arg.exp, arg.args));
    }

    public static Exp D(params object[] exp)
        => E(new[] { "{" }.Concat(exp).Append("}").ToArray());

    public static Exp ExpandDict(Exp exp, IEnumerable<Exp> args)
    {
        return D(GetDictBody(exp)
            .Select(prop => new { key = prop.First(), value = prop.Last() })
            .SelectMany(prop => new[] { prop.key, prop.value is Exp exp ? exp.Expand(args) : E("const", prop.value) })
            .ToArray());
    } 

    public static Expression TranslateDict(Exp dict)
    {
        var pairs = GetProps(dict);
        var dictType = BuildDictType(pairs);
        var meta = BuildMeta(pairs);
        var ctor = dictType.GetConstructor(Array.Empty<Type>());

        var newExp = Expression.New(ctor);
        var bindings = BuildBindings(pairs, dictType, meta);

        var exp = Expression.MemberInit(newExp, bindings);

        return exp;
    }

    private static IEnumerable<MemberBinding> BuildBindings(IEnumerable<TypeDictProp> props, Type dictType, string meta)
    {
        var bindings = props
            .Append(new TypeDictProp(Key: "meta", Value: Expression.Constant(meta), Prop: "Metadata"))
            .Select(prop => Expression.Bind(
                member: dictType.GetProperty(prop.Prop, BindingFlags.Public | BindingFlags.Instance),
                expression: prop.Value))
            .ToList();

        return bindings;
    }

    private static IEnumerable<TypeDictProp> GetProps(Exp dict)
    {
        var props = GetDictBody(dict)
            .Select((part, i) => new TypeDictProp(
                Key: part.Nth<string>(0),
                Value: part.Nth<Exp>(-1).Translate(),
                Prop: $"Prop{i + 1}"))
            .ToList();

        return props;
    }

    private static IEnumerable<IEnumerable<object>> GetDictBody(Exp dict)
        => dict.Skip(1).Partition(2).Where(part => part.Count() == 2);

    private static Type BuildDictType(IEnumerable<TypeDictProp> pairs)
    {
        var pad = Enumerable
            .Range(0, 100)
            .Select((_) => typeof(object));

        var types = pairs
            .Select(pair => pair.Value.Type)
            .Concat(pad)
            .Take(100)
            .ToArray();

        var dictType = typeof(TypeDict<
,,,,,,,,,,,,,,,,,,,,,,,,,
,,,,,,,,,,,,,,,,,,,,,,,,,
,,,,,,,,,,,,,,,,,,,,,,,,,
,,,,,,,,,,,,,,,,,,,,,,,,>)
            .MakeGenericType(types);

        return dictType;
    }

    private static string BuildMeta(IEnumerable<TypeDictProp> pairs)
    {
        var meta = pairs.ToDictionary(
            pair => pair.Key,
            pair => pair.Prop);

        var jsonMeta = JsonConvert.SerializeObject(meta);

        return jsonMeta;
    }
}

record TypeDictProp(
    string Key,
    Expression Value,
    string Prop);

public class TypeDict<
    T1, T2, T3, T4, T5,
    T6, T7, T8, T9, T10,
    T11, T12, T13, T14, T15,
    T16, T17, T18, T19, T20,
    T21, T22, T23, T24, T25,
    T26, T27, T28, T29, T30,
    T31, T32, T33, T34, T35,
    T36, T37, T38, T39, T40,
    T41, T42, T43, T44, T45,
    T46, T47, T48, T49, T50,
    T51, T52, T53, T54, T55,
    T56, T57, T58, T59, T60,
    T61, T62, T63, T64, T65,
    T66, T67, T68, T69, T70,
    T71, T72, T73, T74, T75,
    T76, T77, T78, T79, T80,
    T81, T82, T83, T84, T85,
    T86, T87, T88, T89, T90,
    T91, T92, T93, T94, T95,
    T96, T97, T98, T99, T100>
{
    public string Metadata { get; set; }
    public T1 Prop1 { get; set; }
    public T2 Prop2 { get; set; }
    public T3 Prop3 { get; set; }
    public T4 Prop4 { get; set; }
    public T5 Prop5 { get; set; }
    public T6 Prop6 { get; set; }
    public T7 Prop7 { get; set; }
    public T8 Prop8 { get; set; }
    public T9 Prop9 { get; set; }
    public T10 Prop10 { get; set; }
    public T11 Prop11 { get; set; }
    public T12 Prop12 { get; set; }
    public T13 Prop13 { get; set; }
    public T14 Prop14 { get; set; }
    public T15 Prop15 { get; set; }
    public T16 Prop16 { get; set; }
    public T17 Prop17 { get; set; }
    public T18 Prop18 { get; set; }
    public T19 Prop19 { get; set; }
    public T20 Prop20 { get; set; }
    public T21 Prop21 { get; set; }
    public T22 Prop22 { get; set; }
    public T23 Prop23 { get; set; }
    public T24 Prop24 { get; set; }
    public T25 Prop25 { get; set; }
    public T26 Prop26 { get; set; }
    public T27 Prop27 { get; set; }
    public T28 Prop28 { get; set; }
    public T29 Prop29 { get; set; }
    public T30 Prop30 { get; set; }
    public T31 Prop31 { get; set; }
    public T32 Prop32 { get; set; }
    public T33 Prop33 { get; set; }
    public T34 Prop34 { get; set; }
    public T35 Prop35 { get; set; }
    public T36 Prop36 { get; set; }
    public T37 Prop37 { get; set; }
    public T38 Prop38 { get; set; }
    public T39 Prop39 { get; set; }
    public T40 Prop40 { get; set; }
    public T41 Prop41 { get; set; }
    public T42 Prop42 { get; set; }
    public T43 Prop43 { get; set; }
    public T44 Prop44 { get; set; }
    public T45 Prop45 { get; set; }
    public T46 Prop46 { get; set; }
    public T47 Prop47 { get; set; }
    public T48 Prop48 { get; set; }
    public T49 Prop49 { get; set; }
    public T51 Prop51 { get; set; }
    public T52 Prop52 { get; set; }
    public T53 Prop53 { get; set; }
    public T54 Prop54 { get; set; }
    public T55 Prop55 { get; set; }
    public T56 Prop56 { get; set; }
    public T57 Prop57 { get; set; }
    public T58 Prop58 { get; set; }
    public T59 Prop59 { get; set; }
    public T60 Prop60 { get; set; }
    public T61 Prop61 { get; set; }
    public T62 Prop62 { get; set; }
    public T63 Prop63 { get; set; }
    public T64 Prop64 { get; set; }
    public T65 Prop65 { get; set; }
    public T66 Prop66 { get; set; }
    public T67 Prop67 { get; set; }
    public T68 Prop68 { get; set; }
    public T69 Prop69 { get; set; }
    public T70 Prop70 { get; set; }
    public T71 Prop71 { get; set; }
    public T72 Prop72 { get; set; }
    public T73 Prop73 { get; set; }
    public T74 Prop74 { get; set; }
    public T75 Prop75 { get; set; }
    public T76 Prop76 { get; set; }
    public T77 Prop77 { get; set; }
    public T78 Prop78 { get; set; }
    public T79 Prop79 { get; set; }
    public T80 Prop80 { get; set; }
    public T81 Prop81 { get; set; }
    public T82 Prop82 { get; set; }
    public T83 Prop83 { get; set; }
    public T84 Prop84 { get; set; }
    public T85 Prop85 { get; set; }
    public T86 Prop86 { get; set; }
    public T87 Prop87 { get; set; }
    public T88 Prop88 { get; set; }
    public T89 Prop89 { get; set; }
    public T90 Prop90 { get; set; }
    public T91 Prop91 { get; set; }
    public T92 Prop92 { get; set; }
    public T93 Prop93 { get; set; }
    public T94 Prop94 { get; set; }
    public T95 Prop95 { get; set; }
    public T96 Prop96 { get; set; }
    public T97 Prop97 { get; set; }
    public T98 Prop98 { get; set; }
    public T99 Prop99 { get; set; }
    public T100 Prop100 { get; set; }
}
