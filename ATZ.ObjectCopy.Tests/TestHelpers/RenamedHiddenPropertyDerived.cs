namespace ATZ.ObjectCopy.Tests.TestHelpers
{
    public class RenamedHiddenPropertyDerived : RenamedHiddenPropertyBase
    {
        private string Description
        {
            get => Title;
            set => Title = value;
        }
    }
}