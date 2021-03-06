﻿using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace dotnetCampus.YmlTool
{
    public class YamlParser
    {
        /// <summary>
        /// 检测获取yaml的编写不规范，以及修复后的yaml树
        /// </summary>
        /// <param name="filename"></param>
        /// <param name="newlist"></param>
        /// <returns></returns>
        public static ObservableCollection<ErrorItem> Parse(string filename, out List<string> newlist)
        {
            var lines = File.ReadAllLines(filename);
            //如果包含TAB，可以考虑报异常
            newlist = new List<string>();

            var map = new Dictionary<string, string>();
            var errorlist = new ObservableCollection<ErrorItem>(); //错误项
            var currentNode = new YamlNode(new YamlData(), null);
            var headNode = currentNode;
            var currentSpan = -1;
            var i = 0; //行号
            foreach (var l in lines)
            {
                i++;
                var line = l;
                if (string.IsNullOrEmpty(line) || !line.Contains(":") || line[0] == '#')
                {
                    newlist.Add(line);
                    continue;
                }

                var comment = string.Empty;
                if (Regex.IsMatch(line, "^\\s*#"))
                {
                    //去掉注释
                    var commentTokens = line.Split('#');
                    line = commentTokens[0];
                    comment = "#" + commentTokens[1];
                    if (!line.Contains(":"))
                    {
                        errorlist.Add(new ErrorItem { ErrorCode = 5, Line = i });
                        newlist.Add(comment);
                        continue;
                    }
                }


                var match = Regex.Match(line, "([\\w.]*?):\\s*(.*)");
                var key = match.Groups[1].Value.Trim();
                if (key.Equals(string.Empty)) //没有键
                {
                    errorlist.Add(new ErrorItem { ErrorCode = 1, Line = i });
                }

                var value = match.Groups[2].Value.Trim();


                //开头缩进有tab
                if (Regex.IsMatch(line, "^\\s*\\t+\\s*"))
                {
                    errorlist.Add(new ErrorItem { ErrorCode = 3, Line = i });
                }

                //替换tab
                Regex.Replace(line, @"\t", "  ");


                var span = GetSpan(line);
                if (span <= currentSpan)
                {
                    while (span <= currentSpan)
                    {
                        currentNode = currentNode.Parent;
                        currentSpan = currentNode.Data.Span;
                    }
                }

                //同一层级统一缩进
                var newspan = currentNode.Children.Count > 0 ? currentNode.Children[0].Data.Span : span;
                var node = new YamlNode(new YamlData(key, newspan, key, value, i), currentNode);


                if (!string.IsNullOrEmpty(node.Parent.Data.Key))
                {
                    node.Data.Key = node.Parent.Data.Key + "." + key;
                }

                var dataValue = node.Data.Value;
                if (!string.IsNullOrEmpty(dataValue))
                {
                    //双引号内不转义
                    //if (Regex.IsMatch(dataValue, "\".*?\""))
                    //{
                    //    dataValue = dataValue.Replace("\"", "");
                    //    dataValue = dataValue.Replace("\\n", "\n");
                    //}
                    if (map.ContainsKey(node.Data.Key))
                    {
                        errorlist.Add(new ErrorItem { ErrorCode = 0004, Line = i });
                    }
                    else
                    {
                        map.Add(node.Data.Key, dataValue);
                    }
                }


                currentNode.Children.Add(node);
                if (currentSpan <= span)
                {
                    currentSpan = span;
                    currentNode = node;
                }

                var datakey = node.Data.Name;
                if (errorlist.Count > 0 && errorlist.Last().Line == i)
                {
                    if (errorlist.Last().ErrorCode == 1) //没有键
                    {
                        if (comment != string.Empty)
                        {
                            newlist.Add(comment);
                        }

                        continue;
                    }


                    //if (errorlist.Last().ErrorCode == 2)//没有值
                    //{
                    //    dataValue = "value needed";
                    //}

                    if (errorlist.Last().ErrorCode == 4) //重复key
                    {
                        datakey = "duplicate_" + datakey;
                    }
                }

                var newline = new string(' ', node.Data.Span) + $"{datakey}: {dataValue} {comment}";
                newlist.Add(newline);
            }

            //遍历获取无值的叶节点
            currentNode = headNode;
            var stack = new Stack<YamlNode>();
            stack.Push(currentNode);
            while (stack.Count > 0)
            {
                currentNode = stack.Pop();
                if (currentNode.Children.Count > 0)
                {
                    foreach (var c in currentNode.Children)
                    {
                        stack.Push(c);
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(currentNode.Data.Value))
                    {
                        errorlist.Add(new ErrorItem { ErrorCode = 2, Line = currentNode.Data.Line });
                    }
                }
            }

            return errorlist;
        }

        private static int GetSpan(string line)
        {
            var spaceNum = 0;
            for (var i = 0; i < line.Length; i++)
            {
                if (line[i] == ' ')
                {
                    spaceNum++;
                }
                else
                {
                    break;
                }
            }

            return spaceNum;
        }
    }

    /// <summary>
    ///     Yaml Tree node class
    /// </summary>
    [DebuggerDisplay("Key={Data.Key},Value={Data.Value}")]
    public class YamlNode
    {
        public YamlNode(YamlData data, YamlNode parent)
        {
            Data = data;
            Parent = parent;
            Children = new List<YamlNode>();
        }

        public YamlNode Parent { get; }
        public YamlData Data { get; }
        public List<YamlNode> Children { get; }
    }

    /// <summary>
    ///     Yaml Tree Node data class
    /// </summary>
    [DebuggerDisplay("Key={Key},Value={Value}")]
    public class YamlData
    {
        public YamlData()
        {
            Span = -1;
        }

        public YamlData(string key, int span, string name, string value, int line)
        {
            Key = key;
            Span = span;
            Name = name;
            Value = value;
            Line = line;
        }

        public string Key { get; set; }
        public int Span { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }
        public int Line { get; set; }
    }
}