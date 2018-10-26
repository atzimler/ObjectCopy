using ATZ.ObjectCopy;
using JetBrains.Annotations;
using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace CalendarBlocks.Model.Tasks
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

        #region Public Properties
        [DataMember]
        public TaskCategory Category
        {
            get => _category;
            set => Set(ref _category, value);
        }

        [DataMember]
        public bool Completed
        {
            get => _completed;
            set => Set(ref _completed, value);
        }

        [DataMember]
        public DateTime? Day
        {
            get => _day;
            set => Set(ref _day, value);
        }

        [DataMember]
        public string Description
        {
            get => _description;
            set => Set(ref _description, value);
        }

        [DataMember]
        public Guid Id
        {
            get => _id;
            set => Set(ref _id, value);
        }

        [DataMember]
        public string Notes
        {
            get => _notes;
            set => Set(ref _notes, value);
        }

        [DataMember]
        public decimal Order
        {
            get => _order;
            set => Set(ref _order, value);
        }

        [NotNull]
        public ObservableCollection<TaskModel> SubTasks { get; } = new ObservableCollection<TaskModel>();
        #endregion

        #region Protected Methods
        protected override Guid GetId()
        {
            // TODO: On next major version of the App, when data is migrated instead just loaded, the IObjectGuidId from LifeAreaItemModel derived classes should be moved to LifeAreaItemModel.
            return Id;
        }
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

        [NotNull]
        public TaskModel CloneForRepetition(DateTime day)
        {
            var clone = new TaskModel();
            this.ObjectCopyTo(clone);

            clone.Day = day;
            clone.Id = Guid.NewGuid();

            return clone;
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
