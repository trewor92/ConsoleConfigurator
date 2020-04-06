using ConsoleConfigurator.Helpers;
using ConsoleConfigurator.InitialData;
using ConsoleConfigurator.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleConfigurator
{
    public class FileNotFoundEventArgs : EventArgs
    {
        public FileNotFoundEventArgs(string path)
        {
            Path = path;
        }
        public string Path { get; }
    }

    public class ConfigFileManager
    {
        private ISettingsRepository _repo;
        public event EventHandler<FileNotFoundEventArgs> FileNotFoundEvent;
        public ConfigFileManager(ISettingsRepository repo)
        {
            _repo = repo;
        }

        private async Task<string> ReplaceAllAsync()
        {
            bool createBackup = _repo.HasBackup;
            string result=null;
            foreach (var path in _repo.GetPaths())
            {
                if (ExistConfigFile(path))
                {
                    string currentFile = await ReadFromFileAsync(path);
                    FluentFunctionsReplacer fluentFunctionsReplacer = new FluentFunctionsReplacer(currentFile, createBackup);
                    result = fluentFunctionsReplacer.ReplaceFunctions(_repo.GetEnviroment());
                    await SaveToFileAsync(path, result);
                    if (createBackup)
                    {
                        string json = JsonHelper.ToJson(fluentFunctionsReplacer.Backup);
                        await SaveToFileAsync(_repo.FindBackupPath(path), json); 
                    }
                }
            }
            return result;
        }

        private async Task<string> RollbackAsync()
        {
            string path = _repo.GetBackupFilePath();
            string jsonBackup = await ReadFromFileAsync(_repo.GetBackupConfigPath());
            Dictionary<string,string> backupDict = JsonHelper.ToDictionary(jsonBackup);
            string currentFile = await ReadFromFileAsync(path);
            FluentFunctionsReplacer fluentFunctionsReplacer = new FluentFunctionsReplacer(currentFile);
            string result = fluentFunctionsReplacer.ReplaceFunctions(backupDict);

            await SaveToFileAsync(path, result);
            return result;
        }

        public async Task<string> RunAsync()
        {
            string result;
            if (_repo.IsRollback) result =  await RollbackAsync();
            else result = await ReplaceAllAsync();
            return result;
        }

        private bool ExistConfigFile(string path)
        {
            bool result = File.Exists(path);
            if (!result && FileNotFoundEvent != null)
                FileNotFoundEvent(this, new FileNotFoundEventArgs(path));

            return result;
        }

        private async Task<string> ReadFromFileAsync(string path)
        {   
            using (StreamReader sr = new StreamReader(path))
            {
                return  await sr.ReadToEndAsync();
            }
        }

        private async Task SaveToFileAsync(string path, string text)
        {
            string dirName = Path.GetDirectoryName(path);
            if (!Directory.Exists(dirName)) Directory.CreateDirectory(Path.GetDirectoryName(path));

            using (StreamWriter sw = new StreamWriter(path))
            {
                await sw.WriteAsync(text);
            }
        }

    }
}
