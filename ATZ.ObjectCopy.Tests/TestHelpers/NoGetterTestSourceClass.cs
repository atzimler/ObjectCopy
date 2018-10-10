namespace ATZ.ObjectCopy.Tests.TestHelpers
{
    public class NoGetterTestSourceClass
    {
        private int _p3;

        public int P3
        {
            set => _p3 = value;
        }

        public NoGetterTestSourceClass()
        {
            _p3 = 13;
        }

    }
}
