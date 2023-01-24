using System;
using System.Collections;
using System.Collections.Generic;
using TinyCsvParser.TypeConverter;

namespace TinyCsvParser.Collections
{
    // public class UntypedListInterfaceTypeConverter<T> : IArrayTypeConverter<IList>
    // {
    //     private readonly ITypeConverterProvider _typeConverterProvider;
    //
    //     public UntypedListInterfaceTypeConverter(ITypeConverterProvider typeConverterProvider)
    //     {
    //         _typeConverterProvider = typeConverterProvider ?? throw new ArgumentNullException(nameof(typeConverterProvider));
    //     }
    //
    //     public bool TryConvert(string[] value, out IList result)
    //     {
    //         result = new ArrayList();
    //
    //         var innerTypeConverter = _typeConverterProvider.ResolveCollection<T[]>();
    //
    //         if (innerTypeConverter.TryConvert(value, out var values))
    //         {
    //             result = values as IList;
    //
    //             return true;
    //         }
    //
    //         return false;
    //     }
    //
    //     public Type TargetType { get; } = typeof(IList);
    // }
}