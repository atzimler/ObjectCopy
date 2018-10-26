using CalendarBlocks.Model.Repetitions;
using System;
using System.Runtime.Serialization;

namespace CalendarBlocks.Model
{
    [DataContract]
    public class LifeAreaItemModel : BlockModel
    {
        #region Private Variables
        private Guid? _lifeAreaIdentifier;
        private RepetitionModel _repetition;
        private Guid? _repetitionId;
        #endregion

        #region Public Properties
        [DataMember]
        public Guid? LifeAreaIdentifier
        {
            get => _lifeAreaIdentifier;
            set => Set(ref _lifeAreaIdentifier, value);
        }

        public RepetitionModel Repetition
        {
            get => _repetition;
            set
            {
                Set(ref _repetitionId, value?.Id);
                Set(ref _repetition, value);

                if (_repetition?.OriginalInstanceId == Guid.Empty)
                {
                    _repetition.OriginalInstanceId = GetId();
                }
            }
        }

        [DataMember]
        public Guid? RepetitionId
        {
            // TODO: This could be an Observable object construct if used more than once.
            get => _repetition?.Id ?? _repetitionId;
            set
            {
                Set(ref _repetitionId, value);
                if (value == null)
                {
                    Set(ref _repetition, null);
                    return;
                }

                if (_repetition == null)
                {
                    return;
                }

                _repetition.Id = value.Value;
            }
        }
        #endregion

        #region Protected Methods
        protected virtual Guid GetId()
        {
            // TODO: On next major version of the App, when data is migrated instead just loaded, the IObjectGuidId from LifeAreaItemModel derived classes should be moved to LifeAreaItemModel.
            throw new NotImplementedException();
        }
        #endregion

        #region Public Methods
        public void AssociateWithLifeArea(ILifeArea lifeArea)
        {
            LifeAreaIdentifier = lifeArea?.Id;
            Color = lifeArea?.Color ?? Colors.Color.Gray;
        }
        #endregion
    }
}