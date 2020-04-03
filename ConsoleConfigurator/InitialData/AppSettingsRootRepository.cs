using ConsoleConfigurator.Helpers;
using ConsoleConfigurator.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleConfigurator.InitialData
{
    public class AppSettingsRootRepository:ISettingsRepository
    {
        private AppSettingsRoot _appSettings;
   
        public AppSettingsRootRepository(string[] args)
        {
            _appSettings = new AppSettingsRoot(args);
        }

        public bool HasBackup => !String.IsNullOrEmpty(GetBackupPath());

        public bool IsRollback => GetRollback() != null;
            
        public IEnumerable<string> GetPaths()
        {
            IEnumerable<string> folders = _appSettings.GetFoldersToProcess()??PathHelper.GetChildDirectories(GetBasePath());
            IEnumerable<string> files = _appSettings.GetFilesToProcess();

            foreach (var folder in folders)
            {
                foreach (var file in files)
                {
                    yield return GetBasePath() + '\\' + folder + '\\' + file;
                }
            }
        }

        public Dictionary<string, string> GetEnviroment()
        {
            string envName = _appSettings.GetEnvName();
            return _appSettings.GetEnviroment(envName);
        }

        public string GetBackupConfigPath()
        {
            return PathHelper.FindChildFilePath(GetGuidFolderPath(), "config.js");
        }

        public string GetBackupFilePath()
        {
            if (!IsRollback) return null;
            string backupFilePath = GetBackupConfigPath();
            return GetBasePath() + backupFilePath.StripPrefix(GetGuidFolderPath());
        }

        public string FindBackupPath(string path)
        {
            return GetBackupPath() + '\\' + Guid.NewGuid() + '\\' + path.StripPrefix(GetBasePath());
        }
                    
        private string GetBasePath()
        {
            return _appSettings.GetBasePath();
        }
        private string GetBackupPath() 
        { 
            return _appSettings.GetBackupPath();
        }
       
        private string GetRollback()
        {
            return _appSettings.GetRollback();
        }
        private string GetGuidFolderPath()  
        {
            if (!IsRollback) return null;
            return GetBackupPath() + '\\' + GetRollback();
        }

    }
}
