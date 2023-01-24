using System;
using System.Collections.Generic;
using System.Linq;
using TinyCsvParser.TypeConverter;

namespace TinyCsvParser.Collections
{
    public class ListTypeConverter<T> : IArrayTypeConverter<List<T>>
    {
        private readonly ITypeConverterProvider _typeConverterProvider;

        public ListTypeConverter(ITypeConverterProvider typeConverterProvider)
        {
            _typeConverterProvider = typeConverterProvider ?? throw new ArgumentNullException(nameof(typeConverterProvider));
        }

        public bool TryConvert(string[] value, out List<T> result)
        {
            result = new List<T>();

            var innerTypeConverter = _typeConverterProvider.ResolveCollection<T[]>();

            if (innerTypeConverter.TryConvert(value, out var values))
            {
                result = values.ToList();

                return true;
            }

            return false;
        }

        public Type TargetType { get; } = typeof(List<T>);
    }
}