[MIT License]: https://opensource.org/licenses/MIT

# TinyCsvParser.Collections

Adding support for parsing collections in System.Collections.Generic.

Supports **.NET Core** (.NET Standard 2+)

## Installation

```
PM> Install-Package TinyCsvParser.Collections
```

## Support

This extension adds support for the following types (in alphabetical order):
* `HashSet<T>`
* `ICollection<T>`
* `IEnumerable<T>`
* `IList<T>`
* `IReadOnlyCollection<T>`
* `IReadOnlyList<T>`
* `ISet<T>`
* `LinkedList<T>`
* `List<T>`
* `SortedSet<T>`
* `Stack<T>`
* `Queue<T>`

## Usage

The only thing you need to keep in mind when using this extension
is that your mapping class must have a constructor taking in an instance of `ITypeConverterProvider`
and passing it on to its base constructor. See example below.

```csharp
// Entity
private class Data
{
    public List<int> Ints { get; set; }
}

// Mapping
private class CsvDataMapping : CsvMapping<Data>
{
    // Need to take in ITypeConverterProvider
    public CsvDataMapping(ITypeConverterProvider typeConverterProvider) : base(typeConverterProvider)
    {
        MapProperty(new RangeDefinition(0, 2), x => x.Ints);
    }
}

// Parsing
var options = new CsvParserOptions(skipHeader: false, fieldsSeparator: ',');
var typeConverterProvider = new TypeConverterProvider().AddCollections(); // <-- This line
var parser = new CsvParser<Data>(options, new CsvDataMapping(typeConverterProvider));
var readerOptions = new CsvReaderOptions(new[] { ";" });
var result = parser.ReadFromString(readerOptions, $"0,1,2").ToList();

Console.WriteLine(string.Join(',', result[0].Result.Ints)); // Prints 0,1,2
```

## License

The library is released under terms of the [MIT License].