using CommandLine;
using ConsoleConfigurator.InitialData;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace ConsoleConfigurator
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine(
                "Please enter required:\n"+
                "environment name, for example:         envName=value,\n" +
                "or rollback, for example:              rollback=value.\n\n"+
                "You can optional override:\n" +
                "base path, for example:                configurator:basePath=value,\n"+
                "backup path, for example:              configurator:backupPath=value,\n"+
                "folders to process, for example:       configurator:foldersToProcess:1=value,\n"+
                "files to process, for example:         configurator:filesToProcess:1=value,\n"+
                "folders to process, for example:       configurator:foldersToProcess:1=value.\n"+
                "******************************************************************************");
            try
            {
                ConfigFileManager configFileManager = new ConfigFileManager(new AppSettingsRootRepository(args));
                configFileManager.FileNotFoundEvent += FileNotFound;
                await configFileManager.RunAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception message: " + ex.Message);
            }
        }

        private static void FileNotFound(object sender, FileNotFoundEventArgs e)
        {
            Console.WriteLine($"{e.Path} is not found");
        }

    }
}
