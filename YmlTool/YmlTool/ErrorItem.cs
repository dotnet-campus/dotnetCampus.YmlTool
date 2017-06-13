
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using JetBrains.Annotations;

namespace YmlTool
{
    public class ErrorItem:INotifyPropertyChanged
    {
        private int _line;
        private int _errorCode;

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

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}