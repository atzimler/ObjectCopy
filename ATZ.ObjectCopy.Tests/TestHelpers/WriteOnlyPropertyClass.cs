namespace ATZ.ObjectCopy.Tests.TestHelpers
{
    public class WriteOnlyPropertyClass
    {
        private int _value;

        // ReSharper disable once UnusedMember.Global => used for testing.
        public int P1
        {
            set => _value = value;
        }

        // ReSharper disable once ConvertToAutoPropertyWithPrivateSetter => used for testing.
        public int Read => _value;
    }
}