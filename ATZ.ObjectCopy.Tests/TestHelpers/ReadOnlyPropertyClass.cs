namespace ATZ.ObjectCopy.Tests.TestHelpers
{
    public class ReadOnlyPropertyClass
    {
        public int Ro { get; }

        public ReadOnlyPropertyClass(int value)
        {
            Ro = value;
        }
    }
}