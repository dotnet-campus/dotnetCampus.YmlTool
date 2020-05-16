using System;
using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using YmlTool.Properties;

namespace YmlTool
{
    public class MinusConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var v = (int) value;
            if (v == -1)
            {
                return " ";
            }

            return v.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var v = (((ListViewItem) value)?.DataContext as TextItem)?.IsAdd;
            if (v.HasValue)
            {
                return v.Value ? Brushes.GreenYellow : Brushes.Crimson;
            }

            return Brushes.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class IsAddConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var v = (bool?) value;
            if (v.HasValue)
            {
                return v.Value ? "+" : "-";
            }

            return " ";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class ErrorBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var line = (((ListViewItem) value)?.DataContext as TextItem)?.Preline;
            var errorlines = ((CheckWindowViewModel) parameter)?.Errors;

            foreach (var el in errorlines)
            {
                if (el.Line == line)
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
            {
                return null;
            }

            var msgKey = $"errorMsg{(int) value:D4}";

            return Resources.ResourceManager.GetString(msgKey);
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
            {
                return null;
            }

            var state = ((CheckWindowState) value).ToString();

            return Resources.ResourceManager.GetString(state);
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