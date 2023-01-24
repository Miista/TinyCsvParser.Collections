using System;
using System.Collections.Generic;
using TinyCsvParser.TypeConverter;

namespace TinyCsvParser.Collections
{
    internal class SetTypeConverter<T> : IArrayTypeConverter<ISet<T>>
    {
        private readonly ITypeConverterProvider _typeConverterProvider;

        public SetTypeConverter(ITypeConverterProvider typeConverterProvider)
        {
            _typeConverterProvider = typeConverterProvider ?? throw new ArgumentNullException(nameof(typeConverterProvider));
        }

        public bool TryConvert(string[] value, out ISet<T> result)
        {
            result = new HashSet<T>();

            var innerTypeConverter = _typeConverterProvider.ResolveCollection<HashSet<T>>();

            if (innerTypeConverter.TryConvert(value, out var values))
            {
                result = values as ISet<T>;

                return true;
            }

            return false;
        }

        public Type TargetType { get; } = typeof(ISet<T>);
    }
}