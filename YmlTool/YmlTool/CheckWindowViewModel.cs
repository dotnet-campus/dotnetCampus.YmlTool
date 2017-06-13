using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using JetBrains.Annotations;

namespace YmlTool
{
    public class CheckWindowViewModel:INotifyPropertyChanged
    {
        private double _progress;

        private string _ymlSource;
        private string _targetDir;
        private Dictionary<string, HashSet<string>> _allLangItems;
        private Dictionary<string, HashSet<string>> _selectedLangItems;
        private CheckWindowState _state;
        private bool _isShowDynamicItems;

        public DelegateCommand CheckFilesCommand { get; }
        public DelegateCommand CompareItemsCommand { get; }

        /// <summary>
        /// 进度条
        /// </summary>
        public double Progress
        {
            get => _progress;
            set
            {
                _progress = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 显示语言源状态
        /// </summary>
        public bool IsShowDynamicItems
        {
            get => _isShowDynamicItems;
            set
            {
                _isShowDynamicItems = value;
                OnPropertyChanged();
            }
        }



        /// <summary>
        /// 语言源路径
        /// </summary>
        public string YmlSource
        {
            get =>_ymlSource;
            set
            {
                _ymlSource = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// 语言项路径
        /// </summary>
        public string TargetDir
        {
            get => _targetDir;
            set
            {
                _targetDir = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// 是否递归
        /// </summary>
        public bool IsRecursion { get; set; } = true;

        /// <summary>
        /// 全部语言项
        /// </summary>
        public Dictionary<string, HashSet<string>> AllLangItems
        {
            get => _allLangItems;
            private set
            {
                _allLangItems = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// 筛选的语言项
        /// </summary>
        public Dictionary<string, HashSet<string>> SelectedLangItems
        {
            get => _selectedLangItems;
            private set
            {
                _selectedLangItems = value;
                OnPropertyChanged();
            }
        }
        /// <summary>
        /// 程序状态
        /// </summary>
        public CheckWindowState State
        {
            get => _state;
            private set
            {
                _state = value;
                OnPropertyChanged();
            }
        }
        
        public CheckWindowViewModel()
        {
            State = CheckWindowState.Initialing;

            _progress = 0;

            SelectedLangItems = new Dictionary<string, HashSet<string>>();
            
            IsShowDynamicItems = true;
            CheckFilesCommand = new DelegateCommand(CheckFilesExecute, CheckFilesCanExecute);
            CompareItemsCommand=new DelegateCommand(CompareItemsExecute, CompareItemsCanExecute);
            

            State = CheckWindowState.Ready;

           

        }

        private bool CompareItemsCanExecute()
        {
            return (State == CheckWindowState.Checked ||State==CheckWindowState.Compared)&&
                (File.Exists(YmlSource)|| Directory.Exists(YmlSource)) ;
        }

        private void CompareItemsExecute()
        {
            State=CheckWindowState.Comparing;

            CompareFilesAsync();



        }
        /// <summary>
        /// 比对文件异步方法
        /// </summary>
        private async void  CompareFilesAsync()
        {
            
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        private bool CheckFilesCanExecute()
        {
            return  (State==CheckWindowState.Checked||State==CheckWindowState.Ready||State==CheckWindowState.Compared) 
                &&(File.Exists(TargetDir) || Directory.Exists(TargetDir));
        }

        private void CheckFilesExecute()
        {
            
            
            
        }

        private async void SearchFilesAsync()
        {
            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
