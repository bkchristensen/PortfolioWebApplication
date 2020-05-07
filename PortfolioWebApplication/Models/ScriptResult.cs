using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortfolioWebApplication.Models
{
    public class ScriptResult
    {
        public ScriptResult(string description, object result)
        {
            Description = description;
            Result = result;
        }

        #region Fields & Properties 

        public string Description { get; set; }
        public object Result { get; set;  }

        #endregion
    }
}
