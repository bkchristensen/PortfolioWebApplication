using Microsoft.Scripting.Hosting;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PortfolioWebApplication.Models
{
    [Table("Script")]
    public class Script
    {
        //        var var1, var2 = ...
        //ScriptEngine engine = Python.CreateEngine();
        //        ScriptScope scope = engine.CreateScope();
        //        engine.ExecuteFile(@"C:\test.py", scope);
        //dynamic testFunction = scope.GetVariable("test_func");
        //        var result = testFunction(var1, var2);
        //        Python code:

        //def test_func(var1, var2):
        //    ...do something...

        // https://stackoverflow.com/questions/9569270/custom-method-names-in-asp-net-web-api

        #region Fields & Properties

        public int Id { get; set; }  // Primary Key
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = "Python";
        public string Class { get; set; } = string.Empty;
        public string CodeReferences { get; set; } = string.Empty;

        string _strCode = string.Empty;
        [Display(Description = "Method or Function code. Specify full method text.")]
        [DataType(DataType.MultilineText)]
        public string Code
        {
            get { return _strCode; }
            set
            {
                _strCode = value;
            }
        }

        ScriptSource ScriptSource { get; set; }

        #endregion
    }
}
