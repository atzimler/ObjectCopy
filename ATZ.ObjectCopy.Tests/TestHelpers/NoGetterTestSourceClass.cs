namespace ATZ.ObjectCopy.Tests.TestHelpers
{
    public class NoGetterTestSourceClass
    {
        // ReSharper disable NotAccessedField.Local => Used through reflection.
        private int _p3;
        // ReSharper restore NotAccessedField.Local

        // ReSharper disable UnusedMember.Global => Used through reflection.
        public int P3
            // ReSharper restore UnusedMember.Global
        {
            set => _p3 = value;
        }

        public NoGetterTestSourceClass()
        {
            _p3 = 13;
        }

    }
}
