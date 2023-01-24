using System;
using System.Collections.Generic;
using TinyCsvParser.TypeConverter;

namespace TinyCsvParser.Collections
{
    internal class StackTypeConverter<T> : IArrayTypeConverter<Stack<T>>
    {
        private readonly ITypeConverterProvider _typeConverterProvider;

        public StackTypeConverter(ITypeConverterProvider typeConverterProvider)
        {
            _typeConverterProvider = typeConverterProvider ?? throw new ArgumentNullException(nameof(typeConverterProvider));
        }

        public bool TryConvert(string[] value, out Stack<T> result)
        {
            result = new Stack<T>();
            
            var innerTypeConverter = _typeConverterProvider.ResolveCollection<T[]>();
            
            if (innerTypeConverter.TryConvert(value, out var values))
            {
                result = new Stack<T>(values);

                return true;
            }

            return false;
        }

        public Type TargetType { get; } = typeof(Stack<T>);
    }
}