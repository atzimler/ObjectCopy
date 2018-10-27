using System;
using System.Runtime.Serialization;
using ATZ.ObservableObjects;

namespace CalendarBlocks.Model
{
    [DataContract]
    public class LifeAreaItemModel : ObservableObject
    {
        private Guid? _lifeAreaIdentifier;

        [DataMember]
        public Guid? LifeAreaIdentifier
        {
            get => _lifeAreaIdentifier;
            set => Set(ref _lifeAreaIdentifier, value);
        }
    }
}