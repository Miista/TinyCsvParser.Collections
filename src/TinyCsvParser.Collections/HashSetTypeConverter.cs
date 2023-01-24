using System;
using System.Collections.Generic;
using TinyCsvParser.TypeConverter;

namespace TinyCsvParser.Collections
{
    internal class HashSetTypeConverter<T> : IArrayTypeConverter<HashSet<T>>
    {
        private readonly ITypeConverterProvider _typeConverterProvider;

        public HashSetTypeConverter(ITypeConverterProvider typeConverterProvider)
        {
            _typeConverterProvider = typeConverterProvider ?? throw new ArgumentNullException(nameof(typeConverterProvider));
        }

        public bool TryConvert(string[] value, out HashSet<T> result)
        {
            result = new HashSet<T>();

            var innerTypeConverter = _typeConverterProvider.ResolveCollection<T[]>();

            if (innerTypeConverter.TryConvert(value, out var values))
            {
                result = new HashSet<T>(values);

                return true;
            }

            return false;
        }

        public Type TargetType { get; } = typeof(HashSet<T>);
    }
}