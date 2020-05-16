using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace YmlTool
{
    public class TextItem : INotifyPropertyChanged
    {
        public TextItem(string text, int preline = -1, int postline = -1, bool? isadd = null)
        {
            Text = text;
            Preline = preline;
            Postline = postline;
            IsAdd = isadd;
        }

        public int Preline
        {
            get => _preline;
            set
            {
                _preline = value;
                OnPropertyChanged();
            }
        }

        public int Postline
        {
            get => _postline;
            set
            {
                _postline = value;
                OnPropertyChanged();
            }
        }

        public string Text
        {
            get => _text;
            set
            {
                _text = value;
                OnPropertyChanged();
            }
        }

        public bool? IsAdd
        {
            get => _isAdd;
            set
            {
                _isAdd = value;
                OnPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private bool? _isAdd;
        private int _postline;

        private int _preline;
        private string _text;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}