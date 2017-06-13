using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Markup;

namespace YmlTool
{
   
    public class YamlParser
    {
        /// <summary>
        /// </summary>
        /// <param name="lines">yaml document as string lines</param>
        /// <param name="prefix">the prefix of key</param>
        /// <returns>Key and values, key as concat keys (parentName.childName)</returns>
        public static Dictionary<string, string> Parse(string[] lines, out Dictionary<string, int> lineNum, string prefix = null)
        {
            //如果包含TAB，可以考虑报异常
            lineNum=new Dictionary<string, int>();

            var map = new Dictionary<string, string>();
            var currentNode = new YamlNode(new YamlData(), null);
            int currentSpan = -1;
            var i = 0;
            foreach (var l in lines)
            {
                i++;
                var line = l;
                if (string.IsNullOrEmpty(line) || !line.Contains(":") || line[0] == '#') continue;

                if (Regex.IsMatch(line, "^\\s*#"))
                {
                    //去掉注释
                    var commentTokens = line.Split('#');
                    line = commentTokens[0];
                    if (!line.Contains(":"))
                    {
                        continue;
                    }
                }

                var match = Regex.Match(line, "(.*?):\\s*(.*)");
                var key = match.Groups[1].Value.Trim();
                var value = match.Groups[2].Value.Trim();

                var span = GetSpan(line);
                if (span <= currentSpan)
                {
                    while (span <= currentSpan)
                    {
                        currentNode = currentNode.Parent;
                        currentSpan = currentNode.Data.Span;
                    }
                }

                var node = new YamlNode(new YamlData(key, span, key, value), currentNode);

                if (!string.IsNullOrEmpty(node.Parent.Data.Key))
                {
                    node.Data.Key = node.Parent.Data.Key + "." + key;
                }

                var dataValue = node.Data.Value;
                if (!string.IsNullOrEmpty(dataValue))
                {
                    //双引号内不转义
                    if (Regex.IsMatch(dataValue, "\".*?\""))
                    {
                        dataValue = dataValue.Replace("\"", "");
                        dataValue = dataValue.Replace("\\n", "\n");
                    }
                    map.Add(prefix + node.Data.Key, dataValue );
                    lineNum.Add(prefix + node.Data.Key,i);
                }

                currentNode.Children.Add(node);
                if (currentSpan <= span)
                {
                    currentSpan = span;
                    currentNode = node;
                }
                
            }
            return map;
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
        public YamlNode Parent { get; }
        public YamlData Data { get; }
        public List<YamlNode> Children { get; }

        public YamlNode(YamlData data, YamlNode parent)
        {
            Data = data;
            Parent = parent;
            Children = new List<YamlNode>();
        }
    }

    /// <summary>
    ///     Yaml Tree Node data class
    /// </summary>
    [DebuggerDisplay("Key={Key},Value={Value}")]
    public class YamlData
    {
        public string Key { get; set; }
        public int Span { get; set; }
        public string Name { get; set; }
        public string Value { get; set; }

        public YamlData()
        {
            Span = -1;
        }

        public YamlData(string key, int span, string name, string value)
        {
            Key = key;
            Span = span;
            Name = name;
            Value = value;
        }
    }


}
