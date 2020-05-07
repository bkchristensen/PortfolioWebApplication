using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortfolioWebApplication.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortfolioWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScriptsApiController : ControllerBase
    {
        private readonly ScriptContext _context;
        PythonManager _pythonManager = null;

        public ScriptsApiController(ScriptContext context)
        {
            _context = context;
            _pythonManager = new PythonManager(context);
        }

        // GET: api/ScriptsApi
        [HttpGet]
        //[Route("~/api/[controller]/GetScripts")]
        public async Task<ActionResult<IEnumerable<Script>>> GetScriptItems()
        {
            return await _context.ScriptItems.ToListAsync();
        }

        [HttpGet]
        [Route("~/api/GetScripts")]
        public async Task<ActionResult<IEnumerable<Script>>> GetScripts()
        {
            return await GetScriptItems();
        }

        // GET: api/ScriptsApi/5
        [HttpGet("{id}")]
        //[Route("~/api/[controller]/GetScript/{id}")]
        public async Task<ActionResult<Script>> GetScript(long id)
        {
            var script = await _context.ScriptItems.FindAsync(id);

            if (script == null)
            {
                return NotFound();
            }

            return script;
        }

        [Route("~/api/GetScript/{id}")]
        public async Task<ActionResult<Script>> GetScript2(long id)
        {
            return await GetScript(id);
        }


        // PUT: api/ScriptsApi/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutScript(long id, Script script)
        {
            if (id != script.Id)
            {
                return BadRequest();
            }

            _context.Entry(script).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ScriptExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ScriptsApi
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Script>> PostScript(Script script)
        {
            _context.ScriptItems.Add(script);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetScript", new { id = script.Id }, script);
        }

        // DELETE: api/ScriptsApi/5
        [HttpDelete("{id}")]
        //[Route("~/api/[controller]/DeleteScript/{id}")]
        public async Task<ActionResult<Script>> DeleteScript(long id)
        {
            var script = await _context.ScriptItems.FindAsync(id);
            if (script == null)
            {
                return NotFound();
            }

            _context.ScriptItems.Remove(script);
            await _context.SaveChangesAsync();

            return script;
        }

        [Route("~/api/DeleteScript/{id}")]
        public async Task<ActionResult<Script>> DeleteScript2(long id)
        {
            return await DeleteScript(id);
        }

        //[Route("~/api/ExecuteScript/{className}/{methodName}")]
        //public ActionResult<ScriptResult> ExecuteScript(string className, string methodName)
        //{
        //    Script foundScript = GetScript(className, methodName);

        //    if (foundScript == null)
        //        return new ScriptResult("Script Not Found", null);
        //    else if (foundScript.Type == "Python")
        //        return _pythonManager.ExecuteMethod(className, methodName);

        //    return new ScriptResult("Script Type Not Supported", null);
        //}

        [Route("~/api/ExecuteScript/{className}/{methodName}/{parameters?}")]
        public ActionResult<ScriptResult> ExecuteScript(string className, string methodName, [FromQuery(Name ="param")]string[] parameters)
        {
            Script foundScript = GetScript(className, methodName);
            if (foundScript == null)
                return new ScriptResult("Script Not Found", null);
            else if (foundScript.Type == "Python")
                return _pythonManager.ExecuteMethod(className, methodName, parameters);

            return new ScriptResult("Script Type Not Supported", null);
        }

        [Route("~/api/ExecuteScriptJson")]
        [HttpPost]
        public ActionResult<ScriptResult> ExecuteScriptJson([FromBody]ScriptRequest request)
        {
            Script foundScript = GetScript(request.Class, request.Name);
            if (foundScript == null)
                return new ScriptResult("Script Not Found", null);
            else if (foundScript.Type == "Python")
                return _pythonManager.ExecuteMethod(request.Class, request.Name, request.Parameters.ToArray());

            return new ScriptResult("Script Type Not Supported", null);
        }


        //[Route("~/api/ExecuteScript/{className}/{methodName}/{param1}/param2")]
        //public ActionResult<ScriptResult> ExecuteScript2(string className, string methodName, [FromRoute]dynamic param1, [FromRoute]dynamic param2)
        //{
        //    Script foundScript = GetScript(className, methodName);

        //    if (foundScript == null)
        //        return new ScriptResult("Script Not Found", null);
        //    else if (foundScript.Type == "Python")
        //        return _pythonManager.ExecuteMethod(className, methodName, param1);

        //    return new ScriptResult("Script Type Not Supported", null);
        //}

        /// <summary>
        /// Get a script with a class and method name
        /// </summary>
        /// <param name="className"></param>
        /// <param name="methodName"></param>
        /// <returns></returns>
        Script GetScript(string className, string methodName)
        {
            Script foundScript = null;
            foreach (Script script in _context.ScriptItems)
            {
                if ((script.Class == className) && (script.Name == methodName))
                {
                    foundScript = script;
                    break;
                }
            }
            return foundScript;
        }

        private bool ScriptExists(long id)
        {
            return _context.ScriptItems.Any(e => e.Id == id);
        }
    }
}
