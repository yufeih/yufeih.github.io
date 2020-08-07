---
layout: post
title: Use C# 8 nullable reference type in docfx 3
---

[Docfx 2](https://github.com/dotnet/docfx) has long been bothered by this [billion dollar mistake](https://en.wikipedia.org/wiki/Tony_Hoare). There are crashes caused by [`NullReferenceException`](https://github.com/dotnet/docfx/search?q=nullreferenceexception&type=Issues) every now and then. In the early days of [docfx 3](https://github.com/dotnet/docfx/tree/v3), when we were discussing the engineering guidelines, C# team is already prototyping the idea of [nullable reference types](https://docs.microsoft.com/en-us/dotnet/csharp/nullable-references) (or _non-nullable reference types_). Known that `NullReferenceException` is a big pain and that there is likely a language feature to prevent it, I sketched a [strategy](#the-null-strategy) on `null` handling for docfx 3.

But it was until recently that we finally moved to .NET Core 3. Among all the goodies of .NET Core 3, the most exciting feature to me is _nullable reference types_. I've been experimenting with nullable reference types in docfx 3. Now the conversion is complete, the whole project is now _"null safe"_, it's time to review [the null strategy](#the-null-strategy) and see what worked and the caveats.

## The null strategy

Docfx 3 is design to be a tool rather than a library from the beginning, all we ship is an executable. This avoids a bunch of problems like binary compatibility, API design, etc. It also makes _null handling_ more convenient.

> ⚠️ The null strategy described here is specific to docfx, some of the principles may not apply to other projects.

### Prefer non-nullable types

Whenever possible, use non-nullable types to save unnecessary null checks. Provide a default value for data models:

```csharp
class Blog
{
  public string[] Tags = Array.Empty<string>();
}
```

### Replace argument null checks with nullable reference type

In absence of _nullable reference types_, we use `null` default value to indicate that an argument may be null:

```csharp
object ParseJson(string json, string sourcePath = null)
```

Now with _nullable reference types_, if a type can potentially be null, add nullable reference type modifier `?`.

```csharp
object ParseJson(string json, string? sourcePath = null);
```

### Configure JSON deserialization to ignore nulls 

With _nullable reference types_, you can mark a property type as _non-nullable_ but still get `null` when the object is deserialized from JSON. The compiler does not check runtime variable assignment.

Most JSON libraries provide an option to ignore `null` assignment to your strongly typed classes. Like [NullValueHandling.Ignore](https://www.newtonsoft.com/json/help/html/NullValueHandlingIgnore.htm) in `Json.Net` or [JsonSerializerOptions.IgnoreNullValues Property](https://docs.microsoft.com/en-us/dotnet/api/system.text.json.jsonserializeroptions.ignorenullvalues?view=netcore-3.1) in `System.Text.Json`.

This works for JSON scalars, but what about _arrays_ and _dictionaries_?

#### Remove nulls in arrays and report a user warning

A user could write `["1","2",null,"3"]` and bypass null check if the property type is `string[]`. This is considered user input error in docfx, we simply remove all `null` in arrays and report a warning before deserialization.

#### Mark dictionary value type as nullable

A user could also write `{"a": null}` and by pass null check if the object type is `Dictionary<string, string>`. We could use the same strategy as arrays by removing all `null` entries, but docfx requires some `null` values to be preserved, so using `Dictionary<string, string?>` as the data type is our current choice here.

### Use immutable object model and constructors

Immutable object model has lots of other advantages, it also plays surprisingly well with _nullable reference types_. During the conversion, I had to convert some initialization only types to immutable types with constructors:

```csharp
class Item
{
  public string Name { get; set; } // warning CS8618: Non-nullable property 'Name' is uninitialized. Consider declaring the property as nullable
}
```

is changed to

```csharp
class Item
{
  public string Name { get; }

  public Item(string name) => Name = name;
}
```

### Mark value type fields as nullable

Docfx uses value types a lot to improve performance. However, a non-nullable property in a value type can still be null if the value type is created using the default constructor:

```csharp
struct SourcePosition
{
  public string FileName;
  public int Line;

  public SourcePosition(string fileName, int line)
  {
    FileName = fileName;
    Line = line;
  }
}

SourcePosition position = default; // position.FileName is null and there is no compiler warning.
SourcePosition[] positions = new SourcePosition[2]; // positions[0].FileName is null and there is no compiler warning.
```

It's best to mark the above code as:

```csharp
struct SourcePosition
{
  public int Line;
  public string? FilePath;
}
```

## Conclusion

With the above strategy, if the whole project enables nullable check, the compiler can detect places that potentially throws `NullReferenceException`, and the codebase is _null safe_. Next time, I'll talk about the caveats and false positives of _nullable reference types_ in C# 8 and how to workaround them.
