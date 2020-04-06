using ConsoleConfigurator.Extensions;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;


namespace ConsoleConfigurator.InitialData
{
    public class AppSettingsRoot
    {
        private IConfigurationRoot _configuration;
        public AppSettingsRoot(string[] args)
        {
            _configuration = new ConfigurationBuilder()
                               .SetBasePath(Directory.GetCurrentDirectory())
                               .AddJsonFile($"appsettings.json")
                               .AddCommandLine(args)
                               .Build();
            _configuration.FixOverridenArrays();
        }

        public string GetBasePath()
        {
            string basePath = _configuration["configurator:basePath"];
            if (basePath == null)
                throw new Exception("Base path is not defined");
            return basePath;
        }

        public string GetBackupPath()
        {
            string backupPath = _configuration["configurator:backupPath"];
            return backupPath;
        }

        public string[] GetFoldersToProcess()
        {
            string[] foldersToProcess = _configuration.GetSection("configurator:foldersToProcess")
                .Get<string[]>()?.Where(x => !string.IsNullOrEmpty(x)).ToArray();  //configurator:foldersToProcess:1 :2 etc
            return foldersToProcess;
        }

        public string[] GetFilesToProcess()
        {
            string[] filesToProcess = _configuration.GetSection("configurator:filesToProcess").Get<string[]>().Where(x => !string.IsNullOrEmpty(x)).ToArray();

            if (filesToProcess == null)
                throw new Exception("Files to process is not defined");
            return filesToProcess;
        }

        public Dictionary<string, string> GetEnviroment(string envName)
        {
            if (envName == null)
                throw new ArgumentNullException("Environment name cannot be null!");

            Dictionary<string, string> environment = _configuration
                .GetSection("environments:" + envName)
                .GetChildren()
                .ToDictionary(x => x.Key, x => x.Value);
            if (environment.Count() > 0)
                return environment;
            else
                throw new Exception("Environment is not found!");
        }

        public string GetRollback() 
        {
            string rollback = _configuration["rollback"];
            return rollback;
        }

        public string GetEnvName() 
        {
            string envName = _configuration["apply"];
            if (envName == null)
                throw new Exception("Required environment name is not defined!");
            return envName;
        }

    }
}