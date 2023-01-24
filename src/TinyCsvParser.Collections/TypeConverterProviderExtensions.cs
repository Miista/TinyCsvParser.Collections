using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using TinyCsvParser.Collections.Internal;
using TinyCsvParser.TypeConverter;

namespace TinyCsvParser.Collections
{
    public static class TypeConverterProviderExtensions
    {
        public static ITypeConverterProvider AddCollections(this TypeConverterProvider typeConverterProvider)
        {
            if (typeConverterProvider == null) throw new ArgumentNullException(nameof(typeConverterProvider));

            var registeredTypeConvertersField =
                typeof(TypeConverterProvider)
                    .GetField("typeConverters", BindingFlags.Instance | BindingFlags.NonPublic)
                ?? throw new InvalidOperationException($"Cannot get field 'typeConverters' from type {nameof(TypeConverterProvider)}");

            var registeredTypeConvertersDictionary =
                registeredTypeConvertersField
                    .GetValue(typeConverterProvider) as Dictionary<Type, ITypeConverter>
                ?? throw new InvalidOperationException($"Unexpected field type. Expected {nameof(Dictionary<Type, ITypeConverter>)}");

            // Copy all existing type converters
            var alreadyRegisteredTypes = new Type[registeredTypeConvertersDictionary.Keys.Count];
            registeredTypeConvertersDictionary.Keys.CopyTo(alreadyRegisteredTypes, 0);

            foreach (var type in alreadyRegisteredTypes)
            {
                AddConverters(type, typeConverterProvider);
            }

            return typeConverterProvider;
        }

        private static void AddConverters(Type concreteType, ITypeConverterProvider typeConverterProvider)
        {
            var collectionAndConverters = new[]
            {
                new TypePair(typeof(IEnumerable<>), typeof(EnumerableTypeConverter<>)),
                new TypePair(typeof(ICollection<>), typeof(GenericCollectionInterfaceTypeConverter<>)),
                new TypePair(typeof(IList<>), typeof(GenericListInterfaceTypeConverter<>)),
                new TypePair(typeof(HashSet<>), typeof(HashSetTypeConverter<>)),
                new TypePair(typeof(LinkedList<>), typeof(LinkedListTypeConverter<>)),
                new TypePair(typeof(List<>), typeof(ListTypeConverter<>)),
                new TypePair(typeof(Queue<>), typeof(QueueTypeConverter<>)),
                new TypePair(typeof(IReadOnlyCollection<>), typeof(ReadOnlyCollectionTypeConverter<>)),
                new TypePair(typeof(IReadOnlyList<>), typeof(ReadOnlyListTypeConverter<>)),
                new TypePair(typeof(ISet<>), typeof(SetTypeConverter<>)),
                new TypePair(typeof(SortedSet<>), typeof(SortedSetTypeConverter<>)),
                new TypePair(typeof(Stack<>), typeof(StackTypeConverter<>))
            };

            var convertersWithConcreteType = collectionAndConverters.Select(pair => pair.SpecializeTo(concreteType));

            foreach (var typePair in convertersWithConcreteType)
            {
                // 1. Create instance of the immutable collection converter
                var createMethod =
                    typeof(TypeConverterProviderExtensions)
                        .GetMethod(nameof(Create), BindingFlags.NonPublic | BindingFlags.Static)
                        ?.MakeGenericMethod(typePair.ConverterType, typePair.CollectionType)
                    ?? throw new InvalidOperationException($"Cannot make static generic method from '{nameof(Create)}");

                var optionalConverterInstance = createMethod.Invoke(null, new object[] { typeConverterProvider })
                                                ?? throw new InvalidOperationException(
                                                    $"Cannot make instance of {typePair.ConverterType} specialized to type '{typePair.ConcreteType}'"
                                                );

                // 2. Add the instance to the TypeConverterProvider
                var addMethod =
                    typeof(TypeConverterProviderExtensions)
                        .GetMethod(nameof(Add), BindingFlags.NonPublic | BindingFlags.Static)
                        ?.MakeGenericMethod(typePair.CollectionType)
                    ?? throw new InvalidOperationException($"Cannot make static generic method from '{nameof(Add)}");

                addMethod.Invoke(null, new[] { optionalConverterInstance, typeConverterProvider });
            }
        }

        private static IArrayTypeConverter<TCollection> Create<TConverter, TCollection>(ITypeConverterProvider typeConverterProvider)
        {
            var constructorInfo = GetConstructor(typeof(TConverter));
            var createdInstance = constructorInfo.Invoke(new object[] { typeConverterProvider });
            var arrayTypeConverter = createdInstance as IArrayTypeConverter<TCollection>;

            return arrayTypeConverter;
        }

        private static ConstructorInfo GetConstructor(Type converterType) =>
            converterType.GetConstructor(new[] { typeof(ITypeConverterProvider) })
            ?? throw new InvalidOperationException(
                $"Type {converterType} does not have a public constructor which takes {nameof(ITypeConverterProvider)} as its sole parameter"
            );

        private static void Add<T>(IArrayTypeConverter<T> converter, TypeConverterProvider typeConverterProvider)
        {
            typeConverterProvider.Add(converter);
        }
    }
}