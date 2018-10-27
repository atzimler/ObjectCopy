using ATZ.TimeCalculations;
using ATZ.TimeCalculations.Abstractions;
using System;
using System.Runtime.Serialization;

namespace CalendarBlocks.Model.TimeBlocks
{
    [DataContract(Namespace = "http://schemas.datacontract.org/2004/07/CalendarBlocks.Model.TimeBlocks")]
    public class TimeBlockModel : LifeAreaItemModel
    {
        #region Private Variables
        private TimeIntervalProxy _actual;
        private bool _actualChanged;
        // TODO: We should have a possibility to delete the whole series.
        private DateTime? _deletedOn;
        private string _eventIdentifier;
        private DateTime? _fromActualTime;
        private DateTime _fromPlannedTime;
        private Guid _id;
        private IntervalName _lastSynced = IntervalName.None;
        private string _notes;
        private bool _notesChanged;
        private DateTime? _order;
        private ClosedTimeIntervalProxy _plan;
        private bool _planChanged;
        private DateTime? _toActualTime;
        private DateTime _toPlannedTime;
        #endregion

        #region Public Properties

        public TimeInterval Actual
        {
            get => _actual;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                _actual.From = value.From;
                _actual.To = value.To;
            }
        }

        [DataMember]
        public bool ActualChanged
        {
            get => _actualChanged;
            set => Set(ref _actualChanged, value);
        }

        [DataMember]
        public DateTime? DeletedOn
        {
            get => _deletedOn;
            set => Set(ref _deletedOn, value);
        }

        [DataMember]
        public string EventIdentifier
        {
            get => _eventIdentifier;
            set => Set(ref _eventIdentifier, value);
        }

        [DataMember]
        public DateTime? FromActualTime
        {
            get => _fromActualTime;
            set => Set(ref _fromActualTime, value);
        }

        [DataMember]
        public DateTime FromPlannedTime
        {
            get => _fromPlannedTime;
            set => Set(ref _fromPlannedTime, value);
        }

        [DataMember]
        public Guid Id
        {
            get => _id;
            set => Set(ref _id, value);
        }

        [DataMember]
        public IntervalName LastSynced
        {
            get => _lastSynced;
            set => Set(ref _lastSynced, value);
        }

        [DataMember]
        public string Notes
        {
            get => _notes;
            set => Set(ref _notes, value);
        }

        [DataMember]
        public bool NotesChanged
        {
            get => _notesChanged;
            set => Set(ref _notesChanged, value);
        }

        [DataMember]
        public DateTime? Order
        {
            get => _order;
            set => Set(ref _order, value);
        }

        public IClosedTimeInterval Plan
        {
            get => _plan;
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException(nameof(value));
                }

                _plan.From = value.From;
                _plan.To = value.To;
            }
        }

        [DataMember]
        public bool PlanChanged
        {
            get => _planChanged;
            set => Set(ref _planChanged, value);
        }

        [DataMember]
        public DateTime? ToActualTime
        {
            get => _toActualTime;
            set => Set(ref _toActualTime, value);
        }

        [DataMember]
        public DateTime ToPlannedTime
        {
            get => _toPlannedTime;
            set => Set(ref _toPlannedTime, value);
        }
        #endregion

        #region Constructors
        public TimeBlockModel()
        {
            InitializeIntervalProxies();
        }

        private void InitializeIntervalProxies()
        {
            _actual = new TimeIntervalProxy(
                () => FromActualTime, ndt => FromActualTime = ndt,
                () => ToActualTime, ndt => ToActualTime = ndt);

            _plan = new ClosedTimeIntervalProxy(
                () => FromPlannedTime, dt => FromPlannedTime = dt,
                () => ToPlannedTime, dt => ToPlannedTime = dt);
        }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            InitializeIntervalProxies();
        }
        #endregion

        #region Protected Methods
        protected virtual Guid GetId()
        {
            // TODO: On next major version of the App, when data is migrated instead just loaded, the IObjectGuidId from LifeAreaItemModel derived classes should be moved to LifeAreaItemModel.
            return Id;
        }
        #endregion
    }
}
