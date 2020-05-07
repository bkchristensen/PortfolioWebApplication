using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortfolioWebApplication.Models
{
    public class ScriptRequest
    {
        #region Fields & Properties

        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = "Python";
        public string Class { get; set; } = string.Empty;
        public List<string> Parameters { get; set; } = new List<string>();

        #endregion
    }
}
