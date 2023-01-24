using System;
using System.Collections.Generic;
using TinyCsvParser.TypeConverter;

namespace TinyCsvParser.Collections
{
    public class QueueTypeConverter<T> : IArrayTypeConverter<Queue<T>>
    {
        private readonly ITypeConverterProvider _typeConverterProvider;

        public QueueTypeConverter(ITypeConverterProvider typeConverterProvider)
        {
            _typeConverterProvider = typeConverterProvider ?? throw new ArgumentNullException(nameof(typeConverterProvider));
        }

        public bool TryConvert(string[] value, out Queue<T> result)
        {
            result = new Queue<T>();
            
            var innerTypeConverter = _typeConverterProvider.ResolveCollection<T[]>();
            
            if (innerTypeConverter.TryConvert(value, out var values))
            {
                result = new Queue<T>(values);

                return true;
            }

            return false;
        }

        public Type TargetType { get; } = typeof(Queue<T>);
    }
}