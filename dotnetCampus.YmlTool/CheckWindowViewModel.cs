using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using DiffPlex;
using DiffPlex.DiffBuilder;
using DiffPlex.DiffBuilder.Model;
using JetBrains.Annotations;
using Microsoft.Win32;

namespace dotnetCampus.YmlTool
{
    public class CheckWindowViewModel : INotifyPropertyChanged
    {
        public CheckWindowViewModel()
        {
            State = CheckWindowState.Initialing;


            FullText = new ObservableCollection<TextItem>();
            DiffText = new ObservableCollection<TextItem>();


            CheckFilesCommand = new DelegateCommand(CheckFilesExecute, CheckFilesCanExecute);
            CompareItemsCommand = new DelegateCommand(CompareItemsExecute, CompareItemsCanExecute);
            SaveCommand = new DelegateCommand(SaveExecute, SaveCanExecute);

            State = CheckWindowState.Ready;
        }

        public DelegateCommand CheckFilesCommand { get; }
        public DelegateCommand CompareItemsCommand { get; }
        public DelegateCommand SaveCommand { get; }


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
            get => _ymlSource;
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

        public event PropertyChangedEventHandler PropertyChanged;
        private ObservableCollection<TextItem> _diffText;

        private ObservableCollection<ErrorItem> _errors;
        private ObservableCollection<TextItem> _fullText;
        private bool _isShowDynamicItems;

        private List<string> _newlines;
        private CheckWindowState _state;
        private string _tempfile;
        private string _ymlSource;

        private void SaveExecute()
        {
            var saveFileDialog = new SaveFileDialog
            {
                Filter = "YML文件|*.yml",
                AddExtension = true,
                CheckPathExists = true,
                OverwritePrompt = true,
                DefaultExt = "yml"
            };


            var result = saveFileDialog.ShowDialog();

            if (result == true)
            {
                var filename = saveFileDialog.FileName;
                using (var sw = new StreamWriter(filename))
                {
                    using (var sr = new StreamReader(_tempfile))
                    {
                        while (!sr.EndOfStream)
                        {
                            var s = sr.ReadLine();
                            sw.WriteLine(s);
                        }
                    }
                }
            }

            Application.Current.MainWindow.Focus();
        }

        private bool SaveCanExecute()
        {
            return State == CheckWindowState.Compared && File.Exists(_tempfile);
        }

        private bool CompareItemsCanExecute()
        {
            return (State == CheckWindowState.Checked);
        }

        private void CompareItemsExecute()
        {
            State = CheckWindowState.Comparing;

            _tempfile = Path.GetTempPath() + "\\" + Guid.NewGuid() + Path.GetExtension(YmlSource);

            using (var sw = new StreamWriter(_tempfile))
            {
                foreach (var nl in _newlines)
                {
                    sw.WriteLine(nl);
                }
            }

            DiffFiles(YmlSource, _tempfile);

            State = CheckWindowState.Compared;
        }

        private void DiffFiles(string oldfile, string newfile)
        {
            DiffText.Clear();

            var temp = new ObservableCollection<TextItem>();

            var d = new Differ();
            var builder = new InlineDiffBuilder(d);
            var osr = new StreamReader(oldfile);
            var nsr = new StreamReader(newfile);
            //注意大文本内存问题
            var result = builder.BuildDiffModel(osr.ReadToEnd(), nsr.ReadToEnd());

            int i = 1, j = 1;
            foreach (var line in result.Lines)
            {
                if (line.Type == ChangeType.Inserted)
                {
                    temp.Add(new TextItem(line.Text, -1, j++, true));
                }
                else if (line.Type == ChangeType.Deleted)
                {
                    temp.Add(new TextItem(line.Text, i++, -1, false));
                }
                else if (line.Type == ChangeType.Unchanged)
                {
                    temp.Add(new TextItem(line.Text, i++, j++));
                }
            }

            var r = from textItem in temp
                where textItem.IsAdd != null
                select temp.IndexOf(textItem);
            var lines = new List<int>();
            foreach (var ri in r)
            {
                if (ri > 0)
                {
                    lines.Add(ri - 1);
                    lines.Add(ri);
                    lines.Add(ri + 1);
                }
                else
                {
                    lines.Add(ri);
                    lines.Add(ri + 1);
                }
            }

            foreach (var line in lines.Distinct())
            {
                DiffText.Add(temp[line - 1]);
            }
        }


        private bool CheckFilesCanExecute()
        {
            return (State == CheckWindowState.Checked || State == CheckWindowState.Ready ||
                    State == CheckWindowState.Compared)
                   && File.Exists(YmlSource) && (new FileInfo(YmlSource).Extension == ".yml" ||
                                                 new FileInfo(YmlSource).Extension == ".yaml");
            ;
        }

        private void CheckFilesExecute()
        {
            State = CheckWindowState.Checking;
            var i = 1;
            foreach (var line in File.ReadAllLines(YmlSource))
            {
                FullText.Add(new TextItem(line, i));
                i++;
            }

            Errors = YamlParser.Parse(YmlSource, out _newlines);
            State = CheckWindowState.Checked;
        }

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}