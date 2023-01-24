using System;
using System.Collections.Generic;
using TinyCsvParser.TypeConverter;

namespace TinyCsvParser.Collections
{
    public class SortedSetTypeConverter<T> : IArrayTypeConverter<SortedSet<T>>
    {
        private readonly ITypeConverterProvider _typeConverterProvider;

        public SortedSetTypeConverter(ITypeConverterProvider typeConverterProvider)
        {
            _typeConverterProvider = typeConverterProvider ?? throw new ArgumentNullException(nameof(typeConverterProvider));
        }

        public bool TryConvert(string[] value, out SortedSet<T> result)
        {
            result = new SortedSet<T>();
            
            var innerTypeConverter = _typeConverterProvider.ResolveCollection<T[]>();
            
            if (innerTypeConverter.TryConvert(value, out var values))
            {
                result = new SortedSet<T>(values);

                return true;
            }

            return false;
        }

        public Type TargetType { get; } = typeof(SortedSet<T>);
    }
}