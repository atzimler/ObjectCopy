using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ATZ.ObjectCopy
{
    /// <summary>
    /// Extension class for adding ObjectCopyTo extension method to the type: object.
    /// </summary>
    public static class ObjectCopyExtensions
    {
        private static void CollectionClear(object collection)
        {
            var methodInfo = collection.GetType().GetMethod(nameof(ICollection<int>.Clear));
            methodInfo?.Invoke(collection, null);
        }

        private static IEnumerator CollectionGetEnumerator(object collection)
        {
            var methodInfo = collection.GetType().GetMethod(nameof(ICollection<int>.GetEnumerator));
            return (IEnumerator)methodInfo?.Invoke(collection, null);
        }
        
        private static bool CollectionTypeIsAssignable(Type[] sourceItemTypes, Type[] targetItemTypes)
            // Currently we don't support multiple type parameters and
            // we only support generic collections (old System.Collections.ICollection has no Clear() method).
               => HasOneGenericParameterAndSourceItemTypeIsAssignableToTargetItemType(sourceItemTypes, targetItemTypes);

        private static void CopyCollectionProperty(
            object source, object target,
            [NotNull] PropertyInfo sourceProperty, [NotNull] PropertyInfo targetProperty)
        {
            var sourceCollection = GetCollection(source, sourceProperty);
            var targetCollection = GetCollection(target, targetProperty);
            if (sourceCollection == null || targetCollection == null)
            {
                return;
            }

            CollectionClear(targetCollection);
            var enumerator = CollectionGetEnumerator(sourceCollection);
            var addMethodInfo = targetCollection.GetType().GetMethod(nameof(ICollection<int>.Add));
            if (addMethodInfo == null)
            {
                return;
            }
            
            while (enumerator.MoveNext())
            {
                addMethodInfo.Invoke(targetCollection, new[] { enumerator.Current });
            }
        }

        private static void CopyObjectProperty([NotNull] object source, [NotNull] object target, string propertyName)
        {
            var sourceProperty = source.GetType().GetRuntimeProperty(propertyName);
            var targetProperty = target.GetType().GetRuntimeProperty(propertyName);
            if (IsCollection(sourceProperty.PropertyType))
            {
                CopyCollectionProperty(source, target, sourceProperty, targetProperty);
                return;
            }

            CopyObjectProperty(source, target, sourceProperty, targetProperty);
        }

        private static void CopyObjectProperty(
            object source, object target,
            [NotNull] PropertyInfo sourceProperty, [NotNull] PropertyInfo targetProperty)
        {
            var get = sourceProperty.GetMethod;
            if (get == null)
            {
                return;
            }

            var set = targetProperty.SetMethod;
            set?.Invoke(target, new[] { get.Invoke(source, null) });
        }

        private static object GetCollection(object obj, [NotNull] PropertyInfo propertyInfo)
        {
            var get = propertyInfo.GetMethod;
            return get == null ? null : get.Invoke(obj, null);
        }

        private static bool HasOneGenericParameterAndSourceItemTypeIsAssignableToTargetItemType(Type[] sourceItemTypes, Type[] targetItemTypes)
            => sourceItemTypes.Length == 1 && targetItemTypes.Length == 1 && targetItemTypes[0].IsAssignableFrom(sourceItemTypes[0]);

        private static bool IsCollection(Type type) => typeof(ICollection).IsAssignableFrom(type);
        
        [NotNull]
        [ItemNotNull]
        private static IEnumerable<string> ListPropertyNamesForCopy(Type type)
        {
            return type
                .GetRuntimeProperties()
                .Where(p => p != null && p.DeclaringType != null)
                .Where(p => p.DeclaringType.IsAssignableFrom(type))
                .Select(p => p.Name);
        }

        private static bool PropertyIsCompatibleOnTypes(Type sourceType, Type targetType, string propertyName)
        {
            var sourceProperty = sourceType.GetRuntimeProperty(propertyName);
            var targetProperty = targetType.GetRuntimeProperty(propertyName);

            if (sourceProperty == null || targetProperty == null)
            {
                return false;
            }

            return PropertyTypeIsAssignable(sourceProperty.PropertyType.GetTypeInfo(), targetProperty.PropertyType.GetTypeInfo());
        }

        private static bool PropertyTypeIsAssignable(TypeInfo sourcePropertyType, TypeInfo targetPropertyType)
        {
            if (IsCollection(sourcePropertyType) && IsCollection(targetPropertyType))
            {
                return CollectionTypeIsAssignable(
                    sourcePropertyType.GetGenericArguments(), 
                    targetPropertyType.GetGenericArguments());
            }
            
            return targetPropertyType.IsAssignableFrom(sourcePropertyType);
        }

        /// <summary>
        /// Copy source object to target object with specified parameter.
        /// </summary>
        /// <remarks>
        /// The copy operation from the source to the target works as following:
        /// <list type="number">
        /// <item><description>
        /// Enumerate public properties of the source object at the source type level.
        /// </description></item>
        /// <item><description>
        /// Enumerate public properties of the target object at the target type level.
        /// </description></item>
        /// <item><description>
        /// Intersect the source property list with target property list and if specified with the onlyProperties list.
        /// </description></item>
        /// <item><description>
        /// Limit the properties to where on the source it has a getter and on the target has a setter and the types of
        /// the properties are compatible.
        /// </description></item>
        /// <item><description>
        /// Copy the resulting list of properties by getting the value through the getter from the source and setting
        /// it through the setter on the target.
        /// </description></item>
        /// </list>
        /// </remarks>
        /// <param name="source">The object to copy data from.</param>
        /// <param name="target">The object to copy data to.</param>
        /// <param name="atSourceTypeLevel">
        /// The type of the source to enumerate properties. If not specified it is the actual type of the source object.
        /// This can be used to limit the source type to base class of the source.
        /// </param>
        /// <param name="atTargetTypeLevel">
        /// The type of the target to enumerate properties. If not specified it is the actual type of the target object.
        /// This can be used to limit the target type to base class of the target.
        /// </param>
        /// <param name="onlyProperties">List of properties to limit the copy operations to.</param>
        public static void ObjectCopyTo([NotNull] this object source, [NotNull] object target, Type atSourceTypeLevel = null, Type atTargetTypeLevel = null, IEnumerable<string> onlyProperties = null)
        {
            if (atSourceTypeLevel == null)
            {
                atSourceTypeLevel = source.GetType();
            }
            if (atTargetTypeLevel == null)
            {
                atTargetTypeLevel = target.GetType();
            }

            var sourceProperties = ListPropertyNamesForCopy(atSourceTypeLevel);
            var targetProperties = ListPropertyNamesForCopy(atTargetTypeLevel);

            var propertyNamesToVerify = sourceProperties.Intersect(targetProperties).ToList();
            if (onlyProperties != null)
            {
                var onlyListOfProperties = onlyProperties.ToList();
                propertyNamesToVerify = propertyNamesToVerify.Intersect(onlyListOfProperties).ToList();

            }
            var propertyNamesToCopy = propertyNamesToVerify
                .Where(p => PropertyIsCompatibleOnTypes(atSourceTypeLevel, atTargetTypeLevel, p))
                .ToList();
            propertyNamesToCopy.ForEach(p => CopyObjectProperty(source, target, p));
        }
    }
}
