using System;
using System.Collections.Generic;
using TinyCsvParser.TypeConverter;

namespace TinyCsvParser.Collections
{
    public class ReadOnlyListTypeConverter<T> : IArrayTypeConverter<IReadOnlyList<T>>
    {
        private readonly ITypeConverterProvider _typeConverterProvider;

        public ReadOnlyListTypeConverter(ITypeConverterProvider typeConverterProvider)
        {
            _typeConverterProvider = typeConverterProvider ?? throw new ArgumentNullException(nameof(typeConverterProvider));
        }

        public bool TryConvert(string[] value, out IReadOnlyList<T> result)
        {
            result = new List<T>();

            var innerTypeConverter = _typeConverterProvider.ResolveCollection<T[]>();

            if (innerTypeConverter.TryConvert(value, out var values))
            {
                result = values as IReadOnlyList<T>;

                return true;
            }

            return false;
        }

        public Type TargetType { get; } = typeof(IReadOnlyList<T>);
    }
}