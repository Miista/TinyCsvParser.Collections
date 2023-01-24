using System;
using System.Collections.Generic;
using System.Linq;
using TinyCsvParser;
using TinyCsvParser.Collections;
using TinyCsvParser.Mapping;
using TinyCsvParser.Ranges;
using TinyCsvParser.TypeConverter;

namespace Sandbox
{
    public class Program
    {
        public class Data
        {
            public class Mapping : CsvMapping<Data>
            {
                public Mapping(ITypeConverterProvider typeConverterProvider) : base(typeConverterProvider)
                {
                    MapProperty(new RangeDefinition(0, 3), x => x.ListOfInt);
                    MapProperty(new RangeDefinition(0, 3), x => x.HashSetOfInt);
                    MapProperty(new RangeDefinition(0, 3), x => x.SortedSetOfInt);
                    MapProperty(new RangeDefinition(0, 3), x => x.LinkedListOfInt);
                    MapProperty(new RangeDefinition(0, 3), x => x.EnumerableOfInt);
                    MapProperty(new RangeDefinition(0, 3), x => x.CollectionOfInt);
                    MapProperty(new RangeDefinition(0, 3), x => x.ReadOnlyCollectionOfInt);
                    MapProperty(new RangeDefinition(0, 3), x => x.ReadOnlyListOfInt);
                    MapProperty(new RangeDefinition(0, 3), x => x.SetOfInt);
                    MapProperty(new RangeDefinition(0, 3), x => x.ListInterfaceOfInt);
                }
            }
        
// ReSharper disable IdentifierTypo
// ReSharper disable UnusedAutoPropertyAccessor.Global
            public List<int> ListOfInt { get; set; }
            public HashSet<int> HashSetOfInt { get; set; }
            public SortedSet<int> SortedSetOfInt { get; set; }
            public LinkedList<int> LinkedListOfInt { get; set; }
            public IEnumerable<int> EnumerableOfInt { get; set; }
            public ICollection<int> CollectionOfInt { get; set; }
            public IReadOnlyCollection<int> ReadOnlyCollectionOfInt { get; set; }
            public IReadOnlyList<int> ReadOnlyListOfInt { get; set; }
            public ISet<int> SetOfInt { get; set; }
            public IList<int> ListInterfaceOfInt { get; set; }
// ReSharper enable IdentifierTypo
// ReSharper enable UnusedAutoPropertyAccessor.Global
        }
                
        public static void Main(string[] args)
        {
            var options = new CsvParserOptions(skipHeader: true, fieldsSeparator: ',');
            var typeConverterProvider = new TypeConverterProvider().AddCollections();
            var parser = new CsvParser<Data>(options, new Data.Mapping(typeConverterProvider));
            var readerOptions = new CsvReaderOptions(new[] { ";" });
            var results = parser.ReadFromString(readerOptions, $"Value1,Value2,Value3,Value4;0,1,2,3").ToList();
            
            Console.WriteLine($"Results: {results.Count}");
            foreach (var result in results)
            {
                if (result.IsValid) Console.WriteLine(result.Result.ToString());
                else Console.WriteLine($"Result is invalid: {result.Error}");
            }
        }
    }
}