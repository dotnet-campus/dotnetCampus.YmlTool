using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;

namespace YmlTool
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private CheckWindowViewModel _viewModel;
        public MainWindow()
        {
            InitializeComponent();

            InitializeViewModel();
        }

        private void InitializeViewModel()
        {
            _viewModel = (CheckWindowViewModel)Resources["ViewModel"];

        }
        private void ChooseFile(object sender, RoutedEventArgs e)
        {
            InputTextBox.Focus();//todo:通过失去焦点，使界面发送命令,要改
            var openFileDialog = new OpenFileDialog() { Filter = "YML文件|*.yml;*.yaml" };

            var result = openFileDialog.ShowDialog();

            if (result == true)
            {
                var name = openFileDialog.FileName;
                _viewModel.YmlSource = name;
            }
            ChooseFileBtn.Focus();
            Application.Current.MainWindow.Focus();
        }
    }
}
