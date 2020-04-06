using ConsoleConfigurator.Extensions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ConsoleConfigurator
{
    public class FluentFunctionsReplacer
    {
        bool _writeBackup;
        string _text;

        public Dictionary<string, string> Backup { get; private set; }
        public FluentFunctionsReplacer(string initial, bool writeBackup=false)
        {
            _text = initial;
            _writeBackup = writeBackup;
            if (writeBackup) Backup = new Dictionary<string, string>();
        }

        private string ReplaceFunction(string initial, KeyValuePair<string, string> pair)
        {
            Regex regex = new Regex($@"(env\('{pair.Key}'\)).default\((?<defName>[^)]+)\).+exec\(\)");
            return regex.Replace(initial, match=>
            {
                if (_writeBackup && match.Success)
                    AddToBackup(pair.Key, match.Groups["defName"].Value);
                return match.Value.Replace(match.Groups["defName"].Value, pair.Value.AddQuotesIfNotInt());
            });
        }

        private void AddToBackup(string key, string value)
        {
            Backup.Add(key, value);
        }

        public string ReplaceFunctions(Dictionary<string, string> dictionary)
        {
            string result = _text;
            foreach (var keyValuePair in dictionary)
            {
                result = ReplaceFunction(result, keyValuePair);
            }
            return result;
        }
    }
}
