using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using JetBrains.Annotations;

namespace YmlTool
{
    public class TextItem :INotifyPropertyChanged
    {
        public TextItem( string text ,int preline=-1, int postline=-1, bool? isadd = null)
        {
            Text = text;
            Preline = preline;
            Postline = postline;
            IsAdd = isadd;
        }

        private int _preline;
        private int _postline;
        private string _text;
        private bool? _isAdd;

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

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
