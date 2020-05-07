using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PortfolioWebApplication.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

/// <summary>
/// From tutorial at https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api?view=aspnetcore-3.1&tabs=visual-studio
/// </summary>
namespace PortfolioWebApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PurchasesController : ControllerBase
    {
        private readonly PurchaseContext _context;

        public PurchasesController(PurchaseContext context)
        {
            _context = context;
        }

        // GET: api/Purchases
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Purchase>>> GetPurchaseItems()
        {
            return await _context.PurchaseItems.ToListAsync();
        }

        // GET: api/Purchases/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Purchase>> GetPurchase(long id)
        {
            var purchase = await _context.PurchaseItems.FindAsync(id);

            if (purchase == null)
            {
                return NotFound();
            }

            return purchase;
        }

        // PUT: api/Purchases/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPurchase(long id, Purchase purchase)
        {
            if (id != purchase.ID)
            {
                return BadRequest();
            }

            _context.Entry(purchase).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PurchaseExists(id))
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

        // POST: api/Purchases
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<Purchase>> PostPurchase(Purchase purchase)
        {
            _context.PurchaseItems.Add(purchase);
            await _context.SaveChangesAsync();

            //return CreatedAtAction("GetPurchase", new { id = purchase.ID }, purchase);
            return CreatedAtAction(nameof(GetPurchase), new { id = purchase.ID }, purchase);
        }

        // DELETE: api/Purchases/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Purchase>> DeletePurchase(long id)
        {
            var purchase = await _context.PurchaseItems.FindAsync(id);
            if (purchase == null)
            {
                return NotFound();
            }

            _context.PurchaseItems.Remove(purchase);
            await _context.SaveChangesAsync();

            return purchase;
        }

        private bool PurchaseExists(long id)
        {
            return _context.PurchaseItems.Any(e => e.ID == id);
        }
    }
}
