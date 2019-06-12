namespace ATZ.ObjectCopy.Tests.TestHelpers
{
    public class RenamedHiddenPropertyDerived : RenamedHiddenPropertyBase
    {
        // ReSharper disable UnusedMember.Local => Used by the ObjectCopy via reflection, test only code, no other references.
        private string Description
            // ReSharper restore UnusedMember.Local
        {
            get => Title;
            set => Title = value;
        }
    }
}