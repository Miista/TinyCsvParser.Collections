using System;
using System.Collections.Generic;
using System.Linq;
using TinyCsvParser.TypeConverter;

namespace TinyCsvParser.Collections
{
    internal class EnumerableTypeConverter<T> : IArrayTypeConverter<IEnumerable<T>>
    {
        private readonly ITypeConverterProvider _typeConverterProvider;

        public EnumerableTypeConverter(ITypeConverterProvider typeConverterProvider)
        {
            _typeConverterProvider = typeConverterProvider ?? throw new ArgumentNullException(nameof(typeConverterProvider));
        }

        public bool TryConvert(string[] value, out IEnumerable<T> result)
        {
            result = new List<T>();

            var innerTypeConverter = _typeConverterProvider.ResolveCollection<T[]>();

            if (innerTypeConverter.TryConvert(value, out var values))
            {
                result = values.AsEnumerable();

                return true;
            }

            return false;
        }

        public Type TargetType { get; } = typeof(IEnumerable<T>);
    }
}