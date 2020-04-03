using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleConfigurator.InitialData
{
    public interface ISettingsRepository
    {
        bool HasBackup { get; }
        bool IsRollback { get; }

        IEnumerable<string> GetPaths();

        Dictionary<string, string> GetEnviroment();


        string GetBackupConfigPath();

        string GetBackupFilePath();

        string FindBackupPath(string path);
    }
}
