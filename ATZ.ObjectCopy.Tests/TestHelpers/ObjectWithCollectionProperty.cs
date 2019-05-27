using System.Collections.Generic;

namespace ATZ.ObjectCopy.Tests.TestHelpers
{
    public class ObjectWithCollectionProperty<T>
    {
        public List<T> Property { get; set; } = new List<T>();
    }
}