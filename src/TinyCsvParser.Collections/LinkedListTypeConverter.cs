using System;
using System.Collections.Generic;
using TinyCsvParser.TypeConverter;

namespace TinyCsvParser.Collections
{
    public class LinkedListTypeConverter<T> : IArrayTypeConverter<LinkedList<T>>
    {
        private readonly ITypeConverterProvider _typeConverterProvider;

        public LinkedListTypeConverter(ITypeConverterProvider typeConverterProvider)
        {
            _typeConverterProvider = typeConverterProvider ?? throw new ArgumentNullException(nameof(typeConverterProvider));
        }

        public bool TryConvert(string[] value, out LinkedList<T> result)
        {
            result = new LinkedList<T>();

            var innerTypeConverter = _typeConverterProvider.ResolveCollection<T[]>();

            if (innerTypeConverter.TryConvert(value, out var values))
            {
                result = new LinkedList<T>(values);

                return true;
            }

            return false;
        }

        public Type TargetType { get; } = typeof(LinkedList<T>);
    }
}