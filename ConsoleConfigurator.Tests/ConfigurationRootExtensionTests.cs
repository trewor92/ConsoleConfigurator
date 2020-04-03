using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Xunit;
using ConsoleConfigurator.Extensions;

namespace ConsoleConfigurator.Tests
{
    public class ConfigurationRootExtensionTests
    {
        [Fact]
        public void Can_Delete_Previous_Data()
        {
            IConfigurationBuilder configuration0 = new ConfigurationBuilder();
            Dictionary<string, string> dictionary1 = new Dictionary<string, string>();
            dictionary1.Add("array:1", "1");
            dictionary1.Add("array:2", "2");
            dictionary1.Add("array:3", "3");
            Dictionary<string, string> dictionary2 = new Dictionary<string, string>();
            dictionary2.Add("array:1", "5");
            dictionary2.Add("array:2", "6");
            DictionaryConfigurationSource dictionaryConfigurationSource1 = new DictionaryConfigurationSource(dictionary1);
            DictionaryConfigurationSource dictionaryConfigurationSource2 = new DictionaryConfigurationSource(dictionary2);
            IConfigurationRoot root = configuration0.Add(dictionaryConfigurationSource1).Add(dictionaryConfigurationSource2).Build();
            root.FixOverridenArrays();

            string[] results =  root.GetSection("array").Get<string[]>();

            Assert.DoesNotContain("1", results);
            Assert.DoesNotContain("2", results);
            Assert.DoesNotContain("3",results);
        }
    }

    public class DictionaryConfigurationProvider : ConfigurationProvider
    {
        Dictionary<string, string> _dictionary;
        public DictionaryConfigurationProvider(Dictionary<string,string> dictionary)
        {
            _dictionary = dictionary;
        }
        public override void Load()
        {
            Data = _dictionary;
        }
    }

    public class DictionaryConfigurationSource : IConfigurationSource
    {
        Dictionary<string, string> _dictionary;
        public DictionaryConfigurationSource(Dictionary<string, string> dictionary)
        {
            _dictionary = dictionary;
        }
        public IConfigurationProvider Build(IConfigurationBuilder builder)
        {
            return new DictionaryConfigurationProvider(_dictionary);
        }
    }
}
