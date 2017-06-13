using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace YmlTool
{

    public class ErrorBackgroudConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var line =(((ListViewItem) value)?.DataContext as TextItem)?.Preline;
            var errorlines = ((CheckWindowViewModel) parameter)?.Errors;
            
            foreach (var el in errorlines)
            {
                if (el.Line==line)
                {
                    return Brushes.Crimson;
                }
            }
            return Brushes.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ErrorMsgConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            var msgKey = $"errorMsg{(int) value:D4}"; 

            return Properties.Resources.ResourceManager.GetString(msgKey);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    /// <summary>
    /// 程序状态枚举和字符串的转换
    /// </summary>
    public class CheckWindowStateToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return null;
            var state= ((CheckWindowState)value).ToString();

            return Properties.Resources.ResourceManager.GetString(state);
        }
        /// <summary>
        /// 不使用
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
