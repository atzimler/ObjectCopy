using System;
using System.Runtime.Serialization;
using CalendarBlocks.Model;
using CalendarBlocks.Model.Tasks;

namespace ATZ.ObjectCopy.Tests.TestHelpers
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/CalendarBlocks")]
    public class TaskModel : LifeAreaItemModel
    {
        // TODO: Why do we have Title and Description on the TaskModel?

        #region Private Variables
        private TaskCategory _category = TaskCategory.Task;
        private bool _completed;
        private DateTime? _day;
        private string _description;
        private Guid _id;
        private string _notes;
        private decimal _order;
        #endregion

        #region Public Methods
        public static bool operator ==(TaskModel left, TaskModel right)
        {
            if (ReferenceEquals(left, null) || ReferenceEquals(right, null))
            {
                return ReferenceEquals(left, null) && ReferenceEquals(right, null);
            }

            return left._id == right._id;
        }

        public static bool operator !=(TaskModel left, TaskModel right)
        {
            return !(left == right);
        }

        public override bool Equals(object obj)
        {
            if (!(obj is TaskModel other))
            {
                return false;
            }

            return _id == other._id;
        }

        public override int GetHashCode()
        {
            return 0;
        }
        #endregion
    }
}
