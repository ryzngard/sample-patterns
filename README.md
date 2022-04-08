# Dev Intersection 2022 Notes

## Editorconfig

Added editorconfig to highlight how tooling can help enforce newer styles of csharp.

```ini
// Example: IDE0032: Use auto property
dotnet_diagnostic.IDE0032.severity = error
```

> Note: a lot of the editorconfig is generated defaults

## New Language Features That "Just Work"

* file scoped namespaces
* target type new
* Simplified property patterns `is { Prop1: { Prop2: value }}` becomes `is { Prop1.Prop2: value }`

## New Language Features That Have Support But Need Humans

* Nullability 
* pattern matching


## Benchmark.NET

[Benchmark.NET](https://benchmarkdotnet.org/) is a way to measure various aspects of a program through multiple iterations. Very useful for a way to determine impact of changes in a given scenario.

Since it only requires a method that can be run, anything that is unit testable is likely a good candidate here. 

<details>
<summary> Results from run </summary>

> BenchmarkDotNet=v0.12.1, OS=Windows 10.0.22000
> 11th Gen Intel Core i9-11950H 2.60GHz, 1 CPU, 16 logical and 8 physical > cores
> .NET Core SDK=6.0.300-preview.22178.8
>  [Host]     : .NET Core 6.0.3 (CoreCLR 6.0.322.12309, CoreFX 6.0.322.12309), X64 RyuJIT
>  DefaultJob : .NET Core 6.0.3 (CoreCLR 6.0.322.12309, CoreFX 6.0.322.12309), X64 RyuJIT

---------------------------------------------------------------------
| Method	| Mean	| Error |	StdDev |	Gen 0 |	Gen 1	| Gen 2 |	Allocated |
|-----------|-----------|------------|-----------|----|---|---|---------|
| Scenario1 |	2.073 s |	0.0296 s |	0.0277 s |	- |	- |	- |	3.48 MB |

</details>

## Records

```csharp
record Person(string FirstName, string LastName, string Address);
```

<details>
    <summary> Generated Code </summary>

```csharp
internal class Person : IEquatable<Person>
{
    private readonly string <FirstName>k__BackingField;
    private readonly string <LastName>k__BackingField;
    private readonly string <Address>k__BackingField;

    protected virtual Type EqualityContract
    {
        get
        {
            return typeof(Person);
        }
    }

    public string FirstName
    {
        get
        {
            return <FirstName>k__BackingField;
        }
        init
        {
            <FirstName>k__BackingField = value;
        }
    }

    public string LastName
    {
        get
        {
            return <LastName>k__BackingField;
        }
        init
        {
            <LastName>k__BackingField = value;
        }
    }

    public string Address
    {
        get
        {
            return <Address>k__BackingField;
        }
        init
        {
            <Address>k__BackingField = value;
        }
    }

    public Person(string FirstName, string LastName, string Address)
    {
        <FirstName>k__BackingField = FirstName;
        <LastName>k__BackingField = LastName;
        <Address>k__BackingField = Address;
        base..ctor();
    }

    public override string ToString()
    {
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append("Person");
        stringBuilder.Append(" { ");
        if (PrintMembers(stringBuilder))
        {
            stringBuilder.Append(" ");
        }
        stringBuilder.Append("}");
        return stringBuilder.ToString();
    }

    protected virtual bool PrintMembers(StringBuilder builder)
    {
        builder.Append("FirstName");
        builder.Append(" = ");
        builder.Append((object)FirstName);
        builder.Append(", ");
        builder.Append("LastName");
        builder.Append(" = ");
        builder.Append((object)LastName);
        builder.Append(", ");
        builder.Append("Address");
        builder.Append(" = ");
        builder.Append((object)Address);
        return true;
    }

    public static bool operator !=(Person left, Person right)
    {
        return !(left == right);
    }

    public static bool operator ==(Person left, Person right)
    {
        return (object)left == right || ((object)left != null && left.Equals(right));
    }

    public override int GetHashCode()
    {
        return ((EqualityComparer<Type>.Default.GetHashCode(EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(<FirstName>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(<LastName>k__BackingField)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(<Address>k__BackingField);
    }

    public override bool Equals(object obj)
    {
        return Equals(obj as Person);
    }

    public virtual bool Equals(Person other)
    {
        return (object)this == other || ((object)other != null && EqualityContract == other.EqualityContract && EqualityComparer<string>.Default.Equals(<FirstName>k__BackingField, other.<FirstName>k__BackingField) && EqualityComparer<string>.Default.Equals(<LastName>k__BackingField, other.<LastName>k__BackingField) && EqualityComparer<string>.Default.Equals(<Address>k__BackingField, other.<Address>k__BackingField));
    }

    public virtual Person <Clone>$()
    {
        return new Person(this);
    }

    protected Person(Person original)
    {
        <FirstName>k__BackingField = original.<FirstName>k__BackingField;
        <LastName>k__BackingField = original.<LastName>k__BackingField;
        <Address>k__BackingField = original.<Address>k__BackingField;
    }

    public void Deconstruct(out string FirstName, out string LastName, out string Address)
    {
        FirstName = this.FirstName;
        LastName = this.LastName;
        Address = this.Address;
    }
}
```
</details>

## "Withers": the `with` statement

Withers provide a way to do [nondestructive mutation](https://docs.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-9#nondestructive-mutation). It makes a copy of the item, and then modifies the values to be updated. The original item is left untouched, and the new item contains the modified values. 

Record classes and ALL structs support `with` (as of C# 10). 

### Note:

Since `with` modifies only the copy, certain truths are mantained if the value itself is the same. For example:
```csharp
using System;

var p1 = new Point { x = 1, y = 1};
var p2 = p1 with { };
Console.WriteLine(p1 == p2); // true

var p3 = p1 with { x = 1 };
Console.WriteLine(p1 == p3); // true

var p4 = p1 with { x = 2 };
Console.WriteLine(p1 == p4); // false

record struct Point(int x, int y);
````

## Immutability 

Immutable by default is well supported in c# now. That doesn't mean it's the only way, but it's the one I would recommend. 

Keep in mind this is subjective, but here's how we've seen benefits: 

* Explicit about mutations = less race conditions
* Most code is data contracts
* Caching in and out of process has been easier
* Object versioning + hashing provides a constant "are these two equal" 


Record structs/classes can use `with` to work well with immutability

```csharp
var andrew = new Person("Andrew", "Hall", "1313 Mockingbird Lane");
var andrewAfterMove = andrew with { Address = "1 Cemetary Lane"}; 
```

<details>
<summary> Generated Code </summary>

```csharp
Person person = new Person("Andrew", "Hall", "1313 Mockingbird Lane");
Person person2 = person.<Clone>$();
person2.Address = "1 Cemetary Lane";
Person person3 = person2;
```

</details>
