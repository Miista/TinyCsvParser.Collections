using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using AutoFixture;
using FluentAssertions;
using TinyCsvParser.Mapping;
using TinyCsvParser.Ranges;
using TinyCsvParser.TypeConverter;
using Xunit;

namespace TinyCsvParser.Collections.Tests
{
    public class CollectionTests
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
                    // MapProperty(new RangeDefinition(0, 3), x => x.Collection);
                    // MapProperty(new RangeDefinition(0, 3), x => x.List);
                    // MapProperty(new RangeDefinition(0, 3), x => x.Enumerable);
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
            // public ICollection Collection { get; set; }
            // public IList List { get; set; }
            // public IEnumerable Enumerable { get; set; }
// ReSharper enable IdentifierTypo
// ReSharper enable UnusedAutoPropertyAccessor.Global
        }

        [Theory]
        [MemberData(nameof(Supports_Data))]
        public void Supports<TCollection>(
            string csvData,
            Func<Data, TCollection> extractor,
            TCollection expectedCollection
        )
        {
            // Arrange
            var (parser, readerOptions) = CreateParser();

            // Act
            var results = parser.ReadFromString(readerOptions, csvData).ToList();

            // Assert
            results.Should().NotBeNullOrEmpty();

            var collection = extractor(results.First().Result);
            collection.Should().BeEquivalentTo(expectedCollection, because: "that was the values passed");
        }

        // ReSharper disable once InconsistentNaming
        public static IEnumerable<object[]> Supports_Data
        {
            get
            {
                var fixture = new Fixture();

                var ints = fixture.CreateMany<int>().ToList();
                var set = ints.ToHashSet();
                var sortedSet = new SortedSet<int>(set);
                var linkedList = new LinkedList<int>(ints);

                yield return TestCase(ints, data => data.ListOfInt);
                yield return TestCase(set, data => data.HashSetOfInt);
                yield return TestCase(sortedSet, data => data.SortedSetOfInt);
                yield return TestCase(linkedList, data => data.LinkedListOfInt);
                yield return TestCase(ints.AsEnumerable(), data => data.EnumerableOfInt);
                yield return TestCase(ints, data => data.CollectionOfInt);
                yield return TestCase(ints, data => data.ReadOnlyCollectionOfInt);
                yield return TestCase(ints, data => data.ReadOnlyListOfInt);
                yield return TestCase(set, data => data.SetOfInt);
                yield return TestCase(ints, data => data.ListInterfaceOfInt);
                // yield return TestCase(ints, data => data.Collection);
                // yield return TestCase(ints, data => data.List);
                // yield return TestCase(ints, data => data.Enumerable);

                object[] TestCase<T, TCollection>(IEnumerable<T> items, Func<Data, TCollection> extractor)
                {
                    var csvData = $"Value1;{string.Join(',', items)}";

                    return new object[] { csvData, extractor, items };
                }
            }
        }
        
        private static (ICsvParser<Data> Parser, CsvReaderOptions ReaderOptions) CreateParser()
        {
            var options = new CsvParserOptions(skipHeader: true, fieldsSeparator: ',');
            var typeConverterProvider = new TypeConverterProvider(); 
            typeConverterProvider = typeConverterProvider
                .Add(new ListTypeConverter<int>(typeConverterProvider))
                .Add(new HashSetTypeConverter<int>(typeConverterProvider))
                .Add(new LinkedListTypeConverter<int>(typeConverterProvider))
                .Add(new EnumerableTypeConverter<int>(typeConverterProvider))
                .Add(new GenericCollectionInterfaceTypeConverter<int>(typeConverterProvider))
                .Add(new ReadOnlyCollectionTypeConverter<int>(typeConverterProvider))
                .Add(new ReadOnlyListTypeConverter<int>(typeConverterProvider))
                .Add(new GenericListInterfaceTypeConverter<int>(typeConverterProvider))
                .Add(new SetTypeConverter<int>(typeConverterProvider))
                // .Add(new UntypedCollectionInterfaceTypeConverter<int>(typeConverterProvider))
                // .Add(new UntypedListInterfaceTypeConverter<int>(typeConverterProvider))
                // .Add(new UntypedEnumerableInterfaceTypeConverter<int>(typeConverterProvider))
                .Add(new SortedSetTypeConverter<int>(typeConverterProvider))
                ;
            var parser = new CsvParser<Data>(options, new Data.Mapping(typeConverterProvider));
            var readerOptions = new CsvReaderOptions(new[] { ";" });

            return (Parser: parser, ReaderOptions: readerOptions);
        }
    }
}