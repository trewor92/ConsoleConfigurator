using System;
using Xunit;
using ConsoleConfigurator;
using System.Collections.Generic;

namespace ConsoleConfigurator.Tests
{
    public class FluentFunctionsReplacerTests
    {
        [Fact]
        public void Can_Replace_Fluent_Functions()
        {
            string oldParam1 = "old_param1";
            string oldParam2 = "old_param2";
            string newParameter = "new_parameter";
            string initial = $@"some precede text: env('1').default({oldParam1}).exec();
space or other separator env('2').default(parameter).func1().func2().exec(); enter separator
env('3').default({oldParam2}).func().exec();";
            FluentFunctionsReplacer fluentFunctionsReplacer = new FluentFunctionsReplacer(initial);
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("1", newParameter);
            dictionary.Add("3", newParameter);

            string result = fluentFunctionsReplacer.ReplaceFunctions(dictionary);

            Assert.Equal(initial.Replace(oldParam1, $"'{newParameter}'").Replace(oldParam2, $"'{newParameter}'"), result);
        }

        [Fact]
        public void Can_Save_Backup_Functions()
        {
            string oldParam1 = "old_param1";
            string oldParam2 = "old_param2";
            string newParameter = "new_parameter";
            string initial = $@"some precede text: env('1').default({oldParam1}).exec();
space or other separator env('2').default(parameter).func1().func2().exec(); enter separator
env('3').default({oldParam2}).func().exec();";
            FluentFunctionsReplacer fluentFunctionsReplacer = new FluentFunctionsReplacer(initial, true);
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("1", newParameter);
            dictionary.Add("3", newParameter);

            fluentFunctionsReplacer.ReplaceFunctions(dictionary);
            Dictionary<string,string> result = fluentFunctionsReplacer.Backup;
            Dictionary<string, string> expected = new Dictionary<string, string> { { "1", oldParam1 }, { "3", oldParam2 } };

            Assert.Equal(expected, result);
        }

        [Fact]
        public void Cannot_Recognize_Not_Correct_Functions_Order()
        {
            string oldParam1 = "old_param1";
            string newParameter = "new_parameter";
            string initial = $"some precede text: env('1').FuncBetweenEnvAndDefault().default({oldParam1}).exec()";
            FluentFunctionsReplacer fluentFunctionsReplacer = new FluentFunctionsReplacer(initial);
            Dictionary<string, string> dictionary = new Dictionary<string, string>();
            dictionary.Add("1", newParameter);

            string result = fluentFunctionsReplacer.ReplaceFunctions(dictionary);

            Assert.Equal(initial, result);
        }
    }
    
}
