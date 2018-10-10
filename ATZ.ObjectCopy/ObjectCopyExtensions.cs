using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ATZ.ObjectCopy
{
    public static class ObjectCopyExtensions
    {
        private static void CopyObjectProperty([NotNull] object source, [NotNull] object target, string propertyName)
        {
            var sourceProperty = source.GetType().GetRuntimeProperty(propertyName);
            var targetProperty = target.GetType().GetRuntimeProperty(propertyName);

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

        [NotNull]
        [ItemNotNull]
        private static IEnumerable<string> ListPropertyNamesForCopy(Type type)
        {
            return type.GetRuntimeProperties().Where(p => p != null).Where(p => p.DeclaringType == type).Select(p => p.Name);
        }

        private static bool PropertyIsCompatibleOnTypes(Type sourceType, Type targetType, string propertyName)
        {
            var sourceProperty = sourceType.GetRuntimeProperty(propertyName);
            var targetProperty = targetType.GetRuntimeProperty(propertyName);

            return targetProperty.PropertyType.GetTypeInfo().IsAssignableFrom(sourceProperty.PropertyType.GetTypeInfo());
        }

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

            var propertyNamesToCopy = sourceProperties.Intersect(targetProperties).ToList();
            if (onlyProperties != null)
            {
                var onlyListOfProperties = onlyProperties.ToList();
                var propertyNamesToVerify = onlyListOfProperties.Where(p => !propertyNamesToCopy.Contains(p)).ToList();
                propertyNamesToCopy = propertyNamesToCopy.Intersect(onlyListOfProperties).ToList();

                propertyNamesToVerify.ForEach(p =>
                {
                    if (PropertyIsCompatibleOnTypes(atSourceTypeLevel, atTargetTypeLevel, p))
                        propertyNamesToCopy.Add(p);
                });
            }
            propertyNamesToCopy.ForEach(p => CopyObjectProperty(source, target, p));
        }
    }
}
