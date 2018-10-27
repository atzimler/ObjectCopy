using System;
using System.Runtime.Serialization;
using ATZ.ObservableObjects;

namespace CalendarBlocks.Model
{
    [DataContract]
    public class LifeAreaItemModel
    {
        private Guid? _lifeAreaIdentifier;

        [DataMember]
        public Guid? LifeAreaIdentifier
        {
            get => _lifeAreaIdentifier;
            set => _lifeAreaIdentifier = value;
        }
    }
}