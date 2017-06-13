using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using DiffPlex;
using DiffPlex.DiffBuilder;
using DiffPlex.DiffBuilder.Model;
using JetBrains.Annotations;

namespace YmlTool
{
    public class CheckWindowViewModel:INotifyPropertyChanged
    {

        private List<string> _newlines;
        private string _ymlSource;
        private string _temp;

        private ObservableCollection<ErrorItem> _errors;
        private ObservableCollection<TextItem> _fullText;
        private ObservableCollection<TextItem> _diffText;
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

        public ObservableCollection<TextItem> DiffText
        {
            get => _diffText;
            set
            {
                _diffText = value;
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
            DiffText=new ObservableCollection<TextItem>();
            _temp = Path.GetTempPath();
           
            State=CheckWindowState.Compared;
           
            CheckFilesCommand = new DelegateCommand(CheckFilesExecute, CheckFilesCanExecute);
            CompareItemsCommand=new DelegateCommand(CompareItemsExecute, CompareItemsCanExecute);
            

            State = CheckWindowState.Ready;

           

        }

        private bool CompareItemsCanExecute()
        {
            return (State == CheckWindowState.Checked );
        }

        private void CompareItemsExecute()
        {
            State=CheckWindowState.Comparing;

            var filename = _temp + "\\" + Guid.NewGuid() + Path.GetExtension(YmlSource);
            using (var sw=new StreamWriter(filename))
            {
                foreach (var nl in _newlines)
                {
                    sw.WriteLine(nl);
                }
            }

            DiffFiles(YmlSource, filename);

            State = CheckWindowState.Compared;
        }

        private void DiffFiles(string oldfile, string newfile)
        {
            DiffText.Clear();

            var temp=new ObservableCollection<TextItem>();
            
            var d = new Differ();
            var builder = new InlineDiffBuilder(d);
            var osr=new StreamReader(oldfile);
            var nsr=new StreamReader(newfile);
            //注意大文本内存问题
            var result = builder.BuildDiffModel(osr.ReadToEnd(),nsr.ReadToEnd());
            
            int i = 1, j = 1;
            foreach (var line in result.Lines)
            {
                if (line.Type == ChangeType.Inserted)
                {
                    temp.Add(new TextItem(line.Text,-1,j++,true));
                }
                else if (line.Type == ChangeType.Deleted)
                {
                    temp.Add(new TextItem(line.Text, i++, -1, false));
                }
                else if (line.Type == ChangeType.Unchanged)
                {
                    temp.Add(new TextItem(line.Text));
                }

            }

            var r = from textItem in temp
                where textItem.IsAdd != null
                select temp.IndexOf(textItem);
            var lines=new List<int>();
            foreach (var ri in r)
            {
                lines.Add(ri-1);
                lines.Add(ri);
                lines.Add(ri + 1);
            }

            foreach (var line in lines.Distinct())
            {
                DiffText.Add(temp[line]);
            }

        }


        private bool CheckFilesCanExecute()
        {
            return  (State==CheckWindowState.Checked||State==CheckWindowState.Ready||State==CheckWindowState.Compared)
                &&File.Exists(YmlSource) && (new FileInfo(YmlSource).Extension == ".yml" ||
                new FileInfo(YmlSource).Extension == ".yaml"); ;
        }

        private void CheckFilesExecute()
        {
            
                State=CheckWindowState.Checking;
                var i = 1;
                foreach (var line in File.ReadAllLines(YmlSource))
                {
                    FullText.Add(new TextItem(line,i));
                    i++;
                } 
                
                Errors = YamlParser.Parse(YmlSource, out _newlines);
                State=CheckWindowState.Checked;
           
            
            
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
