using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        private YamlNode _headNode;
        private string _ymlSource;

        private ObservableCollection<ErrorItem> _errors;
        private ObservableCollection<TextItem> _fullText;
        private CheckWindowState _state;
        private bool _isShowDynamicItems;

        public DelegateCommand CheckFilesCommand { get; }
        public DelegateCommand CompareItemsCommand { get; }

        /// <summary>
        /// 错误列表    
        /// </summary>
        public ObservableCollection<ErrorItem> Errors
        {
            get => _errors;
            set
            {
                _errors = value;
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


        public ObservableCollection<TextItem> FullText
        {
            get => _fullText;
            set
            {
                _fullText = value;
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

           
            FullText=new ObservableCollection<TextItem>();
           
            
           
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

        
        private bool CheckFilesCanExecute()
        {
            return  (State==CheckWindowState.Checked||State==CheckWindowState.Ready||State==CheckWindowState.Compared) ;
        }

        private void CheckFilesExecute()
        {
            if (File.Exists(YmlSource)&&
                (new FileInfo(YmlSource).Extension ==".yml"||
                 new FileInfo(YmlSource).Extension == ".yaml" ))
            {
                State=CheckWindowState.Checking;
                var i = 1;
                foreach (var line in File.ReadAllLines(YmlSource))
                {
                    FullText.Add(new TextItem(line,i));
                    i++;
                } 
                
                Errors = YamlParser.Parse(YmlSource, out _headNode);
                State=CheckWindowState.Checked;
            }
            
            
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
