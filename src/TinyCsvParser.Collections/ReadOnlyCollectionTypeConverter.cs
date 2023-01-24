using System;
using System.Collections.Generic;
using TinyCsvParser.TypeConverter;

namespace TinyCsvParser.Collections
{
    public class ReadOnlyCollectionTypeConverter<T> : IArrayTypeConverter<IReadOnlyCollection<T>>
    {
        private readonly ITypeConverterProvider _typeConverterProvider;

        public ReadOnlyCollectionTypeConverter(ITypeConverterProvider typeConverterProvider)
        {
            _typeConverterProvider = typeConverterProvider ?? throw new ArgumentNullException(nameof(typeConverterProvider));
        }

        public bool TryConvert(string[] value, out IReadOnlyCollection<T> result)
        {
            result = new List<T>();

            var innerTypeConverter = _typeConverterProvider.ResolveCollection<T[]>();

            if (innerTypeConverter.TryConvert(value, out var values))
            {
                result = values as IReadOnlyCollection<T>;

                return true;
            }

            return false;
        }

        public Type TargetType { get; } = typeof(IReadOnlyCollection<T>);
    }
}