using System;
using System.Collections;
using System.Collections.Generic;
using TinyCsvParser.TypeConverter;

namespace TinyCsvParser.Collections
{
    // public class UntypedEnumerableInterfaceTypeConverter<T> : IArrayTypeConverter<IEnumerable>
    // {
    //     private readonly ITypeConverterProvider _typeConverterProvider;
    //
    //     public UntypedEnumerableInterfaceTypeConverter(ITypeConverterProvider typeConverterProvider)
    //     {
    //         _typeConverterProvider = typeConverterProvider ?? throw new ArgumentNullException(nameof(typeConverterProvider));
    //     }
    //
    //     public bool TryConvert(string[] value, out IEnumerable result)
    //     {
    //         result = new List<T>();
    //
    //         var innerTypeConverter = _typeConverterProvider.ResolveCollection<T[]>();
    //
    //         if (innerTypeConverter.TryConvert(value, out var values))
    //         {
    //             result = values as IEnumerable;
    //
    //             return true;
    //         }
    //
    //         return false;
    //     }
    //
    //     public Type TargetType { get; } = typeof(IEnumerable);
    // }
}