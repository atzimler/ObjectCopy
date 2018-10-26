using System.Runtime.Serialization;
using ATZ.ObservableObjects;
using Colors;

namespace CalendarBlocks.Model
{
    [DataContract]
    public class BlockModel : ObservableObject
    {
        #region Private Variables
        private Color _backgroundColor = Color.Transparent;
        private Color _color = Color.White;
        private string _title;
        private bool _titleChanged;

        [DataMember]
        private double A { get; set; }

        [DataMember]
        private double R { get; set; }

        [DataMember]
        private double G { get; set; }

        [DataMember]
        private double B { get; set; }
        #endregion

        #region Public Properties
        public Color BackgroundColor
        {
            get => _backgroundColor;
            set => Set(ref _backgroundColor, value);
        } 

        public Color Color
        {
            get => _color;
            set => Set(ref _color, value);
        }

        [DataMember]
        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }

        [DataMember]
        public bool TitleChanged
        {
            get => _titleChanged;
            set => Set(ref _titleChanged, value);
        }
        #endregion

        public void Refresh()
        {
            OnPropertyChanged(string.Empty);
        }

        [OnSerializing]
        // ReSharper disable once UnusedMember.Local => Used by the DataContractSerializer
        // ReSharper disable once UnusedParameter.Local => Required signature for DataContractSerializer hook.
        private void OnSerializing(StreamingContext context)
        {
            A = _color.A;
            R = _color.R;
            G = _color.G;
            B = _color.B;
        }

        [OnDeserialized]
        // ReSharper disable once UnusedMember.Local => Used by the DataContractSerializer
        // ReSharper disable once UnusedParameter.Local => Required signature for DataContractSerializer hook.
        private void OnDeserialized(StreamingContext context)
        {
            _color = Color.FromRgba(R, G, B, A);
        }
    }
}
