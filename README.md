# Dev Intersection 2022 Notes

## Editorconfig

Added editorconfig to highlight how tooling can help enforce newer styles of csharp.

```ini
// Example: IDE0032: Use auto property
dotnet_diagnostic.IDE0032.severity = error
```

> Note: a lot of the editorconfig is generated defaults

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