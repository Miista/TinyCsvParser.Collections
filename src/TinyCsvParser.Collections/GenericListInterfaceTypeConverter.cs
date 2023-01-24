using System;
using System.Collections.Generic;
using TinyCsvParser.TypeConverter;

namespace TinyCsvParser.Collections
{
    internal class GenericListInterfaceTypeConverter<T> : IArrayTypeConverter<IList<T>>
    {
        private readonly ITypeConverterProvider _typeConverterProvider;

        public GenericListInterfaceTypeConverter(ITypeConverterProvider typeConverterProvider)
        {
            _typeConverterProvider = typeConverterProvider ?? throw new ArgumentNullException(nameof(typeConverterProvider));
        }

        public bool TryConvert(string[] value, out IList<T> result)
        {
            result = new List<T>();

            var innerTypeConverter = _typeConverterProvider.ResolveCollection<T[]>();

            if (innerTypeConverter.TryConvert(value, out var values))
            {
                result = values as IList<T>;

                return true;
            }

            return false;
        }

        public Type TargetType { get; } = typeof(IList<T>);
    }
}