using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Documents;
using Microsoft.Win32;

namespace dotnetCampus.YmlTool
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            InitializeViewModel();
        }

        private CheckWindowViewModel _viewModel;

        private void InitializeViewModel()
        {
            _viewModel = (CheckWindowViewModel) Resources["ViewModel"];
        }

        private void ChooseFile(object sender, RoutedEventArgs e)
        {
            InputTextBox.Focus(); //todo:通过失去焦点，使界面发送命令,要改
            var openFileDialog = new OpenFileDialog { Filter = "YML文件|*.yml;*.yaml" };

            var result = openFileDialog.ShowDialog();

            if (result == true)
            {
                var name = openFileDialog.FileName;
                _viewModel.YmlSource = name;
            }

            ChooseFileButton.Focus();
            Application.Current.MainWindow.Focus();
        }

        private void Hyperlink_OnClick(object sender, RoutedEventArgs e)
        {
            var link = sender as Hyperlink;
            try
            {
                Process.Start(new ProcessStartInfo(link.NavigateUri.AbsoluteUri));
            }
            catch (Exception exception)
            {
                
            }
        }
    }
}

namespace YmlTool
{

}