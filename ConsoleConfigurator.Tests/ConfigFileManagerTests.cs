using ConsoleConfigurator.InitialData;
using Moq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ConsoleConfigurator.Tests
{
    public class ConfigFileManagerTests
    {
        [Fact]
        public async Task Can_Replace_Parameters()
        {
            string initialFilePath = "./Resources/testConfigInitial.js";
            string expectedFilePath = "./Resources/testConfigExpected.js";
            Dictionary<string, string> env = new Dictionary<string, string>() { { "MAX_ATTRIBUTE_COUNT", "5000"}, { "SERVICE_NAME", "NEW_TEST_NAME" }, { "AZURE_SERVICEBUS_HOST", "TEST_HOST" } };
            Mock<ISettingsRepository> mockRepo = new Mock<ISettingsRepository>();
            mockRepo.Setup(x => x.GetPaths()).Returns(new[] { initialFilePath});
            mockRepo.Setup(x => x.IsRollback).Returns(false);
            mockRepo.Setup(x => x.HasBackup).Returns(false);
            mockRepo.Setup(x => x.GetEnviroment()).Returns(env);
            ConfigFileManager manager = new ConfigFileManager(mockRepo.Object);

            string result = await manager.RunAsync();            
            string expected = File.ReadAllText(expectedFilePath);

            Assert.Equal(expected, result);
        }
    }
}
