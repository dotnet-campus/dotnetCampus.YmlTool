using System.ComponentModel;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;

namespace YmlTool
{
    public class ErrorItem : INotifyPropertyChanged
    {
        public int Line
        {
            get => _line;
            set
            {
                _line = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 0001:没有key
        /// 0002:没有value
        /// 0003:tab缩进
        /// 0004:重复键
        /// 0005:缺少:
        /// </summary>
        public int ErrorCode
        {
            get => _errorCode;
            set
            {
                _errorCode = value;
                OnPropertyChanged();
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;
        private int _errorCode;
        private int _line;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}