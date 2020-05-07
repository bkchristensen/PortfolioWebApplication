using Microsoft.Scripting.Hosting;
using System;
using System.Collections.Generic;

namespace PortfolioWebApplication.Models
{
    public class PythonManager
    {
        public PythonManager(ScriptContext context)
        {
            _scriptContext = context;
            _pythonClasses = new Dictionary<string, PythonScriptList>();
            BuildScriptList();
        }

        static PythonManager()
        {
        }

        #region Fields & Properties

        static readonly string DefaultClass = "Default";
        ScriptContext _scriptContext;

        IDictionary<string, PythonScriptList> _pythonClasses = null;

        #endregion

        /// <summary>
        /// Build the script lists
        /// </summary>
        void BuildScriptList()
        {
            // Clear the classes to start from a clean slate
            _pythonClasses.Clear();

            foreach (Script script in _scriptContext.ScriptItems)
                AddPythonScript(script);

            Rebuild();
        }
        public bool AddPythonScript(Script script, bool rebuild = false)
        {
            bool isValid = IsValidMethod(script.Code);
            if (isValid == true)
            {
                string strClass = !string.IsNullOrEmpty(script.Class) ? script.Class : DefaultClass;
                if (_pythonClasses.ContainsKey(strClass) == false)
                    _pythonClasses[strClass] = new PythonScriptList(strClass);
                _pythonClasses[strClass].Add(script);
                if (rebuild == true)
                    _pythonClasses[strClass].Rebuild();
            }
            return isValid;
        }

        public bool RemovePythonScript(Script script, bool rebuild = false)
        {
            bool bRemoved = false;
            string strClass = !string.IsNullOrEmpty(script.Class) ? script.Class : DefaultClass;
            if (_pythonClasses.ContainsKey(strClass))
            {
                _pythonClasses[strClass].Remove(script);
                if (rebuild == true)
                    _pythonClasses[strClass].Rebuild();
                bRemoved = true;
            }
            return bRemoved;
        }

        public ScriptResult ExecuteMethod(string strClass, string strMethod, params dynamic[] methodParams)
        {
            ScriptResult scriptResult = null;
            if (_pythonClasses.ContainsKey(strClass))
                scriptResult = _pythonClasses[strClass].ExecuteMethod(strMethod, methodParams);
            return scriptResult;
        }

        static bool IsValidMethod(string strCode)
        {
            return strCode.StartsWith("def");
        }
        //if (!string.IsNullOrEmpty(_strCode))
        //             ScriptSource = PythonEngine.CreateScriptSourceFromString(_strCode, Microsoft.Scripting.SourceCodeKind.AutoDetect);

        public bool Rebuild()
        {
            bool rebuilt = true;

            foreach (KeyValuePair<string, PythonScriptList> scriptPair in _pythonClasses)
            {
                rebuilt = scriptPair.Value.Rebuild() && rebuilt;
            }
            return rebuilt;
        }
        public class PythonScriptList : List<Script> {
            public PythonScriptList(string strClass)
            {
                _scriptEngine = IronPython.Hosting.Python.CreateEngine();
                Class = strClass;
            }

            #region Fields & Properties

            public string Class { get; set;  }
            ScriptEngine _scriptEngine = null;
            ScriptSource _scriptSource = null;
            ScriptScope _scriptScope = null;
            CompiledCode _compiledCode = null;
            object _scriptClass = null;
            #endregion

            public ScriptResult ExecuteMethod(string strMethod, params dynamic[] methodParams)
            {
                ScriptResult result = null;
                try
                {
                    dynamic response = _scriptEngine.Operations.InvokeMember(_scriptClass, strMethod, methodParams);
                    return new ScriptResult(string.Format("Executed {0}", strMethod), response);
                }
                catch (Exception e)
                {
                    result = new ScriptResult(string.Format("Python exception - " + e.Message), "No result");
                }
                return result;
            }

           public bool Rebuild()
            {
                _scriptScope = _scriptEngine.CreateScope();

                string strScript = string.Empty;
                // First process the references
                foreach (Script script in this)
                {
                    if (!string.IsNullOrEmpty(script.CodeReferences))
                    {
                        string[] references = script.CodeReferences.Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
                        foreach (string strReference in references)
                            strScript += string.Format("import {0}{1}", strReference, Environment.NewLine);
                    }
                }
                // Add the class definition
                strScript += string.Format("class {0}:{1}", Class, Environment.NewLine);
                // Now add the method definitions
                foreach (Script script in this)
                {
                    string[] codeLines = script.Code.Split(new string[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                    foreach (string codeLine in codeLines)
                    {
                        string lclCodeLine = codeLine;
                        if (lclCodeLine.StartsWith("def"))
                        {
                            int index = lclCodeLine.IndexOf("()");
                            if (index > 0)
                                lclCodeLine = codeLine.Insert(index + 1, "self");
                            else
                            {
                                index = codeLine.IndexOf("(");
                                lclCodeLine = codeLine.Insert(index + 1, "self,");
                            }
                            strScript += string.Format("\t{0}{1}", lclCodeLine, Environment.NewLine);
                        }
                        else
                            strScript += string.Format("\t\t{0}{1}", codeLine, Environment.NewLine);
                    }
                }

                bool rebuilt = false;
                if (!string.IsNullOrEmpty(strScript))
                {
                    _scriptSource = _scriptEngine.CreateScriptSourceFromString(strScript, Microsoft.Scripting.SourceCodeKind.AutoDetect);
                    if (_scriptSource != null)
                    {
                        try
                        {
                            _compiledCode = _scriptSource.Compile();
                            _compiledCode.Execute(_scriptScope);
                            _scriptClass = _scriptEngine.Operations.Invoke(_scriptScope.GetVariable(Class));
                            rebuilt = true;
                        }
                        catch (Exception)
                        { }
                    }
                }

                return rebuilt;
            }
        }
    }
}
