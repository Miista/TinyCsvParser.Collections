using System;
using System.Collections;
using System.Collections.Generic;
using TinyCsvParser.TypeConverter;

namespace TinyCsvParser.Collections
{
    // public class UntypedCollectionInterfaceTypeConverter<T> : IArrayTypeConverter<ICollection>
    // {
    //     private readonly ITypeConverterProvider _typeConverterProvider;
    //
    //     public UntypedCollectionInterfaceTypeConverter(ITypeConverterProvider typeConverterProvider)
    //     {
    //         _typeConverterProvider = typeConverterProvider ?? throw new ArgumentNullException(nameof(typeConverterProvider));
    //     }
    //
    //     public bool TryConvert(string[] value, out ICollection result)
    //     {
    //         result = new List<T>();
    //
    //         var innerTypeConverter = _typeConverterProvider.ResolveCollection<T[]>();
    //
    //         if (innerTypeConverter.TryConvert(value, out var values))
    //         {
    //             result = values as ICollection;
    //
    //             return true;
    //         }
    //
    //         return false;
    //     }
    //
    //     public Type TargetType { get; } = typeof(ICollection);
    // }
}