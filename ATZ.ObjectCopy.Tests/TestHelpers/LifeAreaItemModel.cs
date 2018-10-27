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
        #endregion
    }
}