using System;
using System.Collections.Generic;
using TinyCsvParser.TypeConverter;

namespace TinyCsvParser.Collections
{
    public class GenericCollectionInterfaceTypeConverter<T> : IArrayTypeConverter<ICollection<T>>
    {
        private readonly ITypeConverterProvider _typeConverterProvider;

        public GenericCollectionInterfaceTypeConverter(ITypeConverterProvider typeConverterProvider)
        {
            _typeConverterProvider = typeConverterProvider ?? throw new ArgumentNullException(nameof(typeConverterProvider));
        }

        public bool TryConvert(string[] value, out ICollection<T> result)
        {
            result = new List<T>();

            var innerTypeConverter = _typeConverterProvider.ResolveCollection<T[]>();

            if (innerTypeConverter.TryConvert(value, out var values))
            {
                result = values as ICollection<T>;

                return true;
            }

            return false;
        }

        public Type TargetType { get; } = typeof(ICollection<T>);
    }
}