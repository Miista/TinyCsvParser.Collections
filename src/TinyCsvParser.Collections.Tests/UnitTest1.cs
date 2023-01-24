using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using TinyCsvParser.Mapping;
using TinyCsvParser.Ranges;
using TinyCsvParser.TypeConverter;
using Xunit;

// ReSharper disable file IdentifierTypo
// ReSharper disable file UnusedAutoPropertyAccessor.Global

namespace TinyCsvParser.Collections.Tests
{
    public class UnitTest1
    {
        public class Data
        {
            public class Mapping : CsvMapping<Data>
            {
                public Mapping(ITypeConverterProvider typeConverterProvider) : base(typeConverterProvider)
                {
                    MapProperty(new RangeDefinition(0, 1), x => x.Ints);
                    MapProperty(new RangeDefinition(2, 3), x => x.IntSet);
                    MapProperty(new RangeDefinition(0, 3), x => x.Links);
                    MapProperty(new RangeDefinition(0, 3), x => x.All);
                    MapProperty(new RangeDefinition(0, 3), x => x.AllCollection);
                }
            }
        
#pragma warning disable CS8618
            public List<int> Ints { get; set; }
            public HashSet<int> IntSet { get; set; }
            public LinkedList<int> Links { get; set; }
            public IEnumerable<int> All { get; set; }
            public ICollection<int> AllCollection { get; set; }
#pragma warning restore CS8618
        }

        [Fact]
        public void Test()
        {
            // Arrange
            var (parser, readerOptions) = CreateParser();

            // Act
            var results = parser.ReadFromString(readerOptions, $"Value1,Value2;0,1").ToList();
        
            // Assert
            results.Should().NotBeNullOrEmpty();
            results.First().Result.Ints.Should().BeEquivalentTo(new List<int>{0, 1}, because: "that was the values passed");
        }
        
        [Fact]
        public void Test2()
        {
            // Arrange
            var (parser, readerOptions) = CreateParser();

            // Act
            var results = parser.ReadFromString(readerOptions, $"Value1,Value2,Value3,Value4;0,1,2,3").ToList();
        
            // Assert
            results.Should().NotBeNullOrEmpty();
            results.First().Result.IntSet.Should().BeEquivalentTo(new HashSet<int>{2,3}, because: "that was the values passed");
        }
        
        [Fact]
        public void Test3()
        {
            // Arrange
            var (parser, readerOptions) = CreateParser();

            // Act
            var results = parser.ReadFromString(readerOptions, $"Value1,Value2,Value3,Value4;0,1,2,3").ToList();
        
            // Assert
            results.Should().NotBeNullOrEmpty();
            results.First().Result.Links.Should().BeEquivalentTo(new LinkedList<int>(new[] { 0, 1, 2, 3 }), because: "that was the values passed");
        }
        
        [Fact]
        public void Can_create_IEnumerable()
        {
            // Arrange
            var (parser, readerOptions) = CreateParser();

            // Act
            var results = parser.ReadFromString(readerOptions, $"Value1,Value2,Value3,Value4;0,1,2,3").ToList();
        
            // Assert
            results.Should().NotBeNullOrEmpty();
            results.First().Result.All.Should().BeEquivalentTo(new[] { 0, 1, 2, 3 }, because: "that was the values passed");
        }
        
        [Fact]
        public void Can_create_ICollection()
        {
            // Arrange
            var (parser, readerOptions) = CreateParser();

            // Act
            var results = parser.ReadFromString(readerOptions, $"Value1,Value2,Value3,Value4;0,1,2,3").ToList();
        
            // Assert
            results.Should().NotBeNullOrEmpty();
            results.First().Result.AllCollection.Should().BeEquivalentTo(new[] { 0, 1, 2, 3 }, because: "that was the values passed");
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
                .Add(new CollectionTypeConverter<int>(typeConverterProvider))
                ;
            var parser = new CsvParser<Data>(options, new Data.Mapping(typeConverterProvider));
            var readerOptions = new CsvReaderOptions(new[] { ";" });

            return (Parser: parser, ReaderOptions: readerOptions);
        }
    }
}