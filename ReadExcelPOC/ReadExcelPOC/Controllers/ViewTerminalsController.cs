using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ReadExcelPOC.Models;

namespace ReadExcelPOC.Controllers
{
    public class ViewTerminalsController : Controller
    {
        private readonly DBContext _context;

        public ViewTerminalsController(DBContext context)
        {
            _context = context;
        }

        // GET: ViewTerminals
        public async Task<IActionResult> Index(int criteria, string value)
        {
            if (String.IsNullOrEmpty(value))
            {
                return View(await _context.Terminal.ToListAsync());
            }
            else
            {
                if (criteria == 0)
                {
                    return View(await _context.Terminal.Where(t => t.PortRKST == value).ToListAsync());
                }
                else
                {
                    return View(await _context.Terminal.Where(t => t.PortGEOID == value).ToListAsync());
                }
            }
        }


        // GET: ViewTerminals
        public async Task<IActionResult> Search(int criteria,string value)
        {
            if (criteria == 0)
            {
                return View(await _context.Terminal.Where(t=>t.TerminalGEOID == value).ToListAsync());
            }
            else
            {
                return View(await _context.Terminal.Where(t => t.PortGEOID == value).ToListAsync());
            }
        }

        // GET: ViewTerminals/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var terminal = await _context.Terminal
                .FirstOrDefaultAsync(m => m.Id == id);
            if (terminal == null)
            {
                return NotFound();
            }

            return View(terminal);
        }

        // GET: ViewTerminals/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ViewTerminals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,TerminalGEOID,SubArea,TerminalRKSTCode,PortRKST,PortGEOID,TerminalName,PortName,ExtendField1,ExtendField2,ExtendField3,ExtendField4,ExtendField5,ExtendField6,ExtendField7,ExtendField8")] Terminal terminal)
        {
            if (ModelState.IsValid)
            {
                terminal.Id = Guid.NewGuid();
                _context.Add(terminal);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(terminal);
        }

        // GET: ViewTerminals/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var terminal = await _context.Terminal.FindAsync(id);
            if (terminal == null)
            {
                return NotFound();
            }
            return View(terminal);
        }

        // POST: ViewTerminals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("Id,TerminalGEOID,SubArea,TerminalRKSTCode,PortRKST,PortGEOID,TerminalName,PortName,ExtendField1,ExtendField2,ExtendField3,ExtendField4,ExtendField5,ExtendField6,ExtendField7,ExtendField8")] Terminal terminal)
        {
            if (id != terminal.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(terminal);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TerminalExists(terminal.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(terminal);
        }

        // GET: ViewTerminals/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var terminal = await _context.Terminal
                .FirstOrDefaultAsync(m => m.Id == id);
            if (terminal == null)
            {
                return NotFound();
            }

            return View(terminal);
        }

        // POST: ViewTerminals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var terminal = await _context.Terminal.FindAsync(id);
            _context.Terminal.Remove(terminal);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TerminalExists(Guid id)
        {
            return _context.Terminal.Any(e => e.Id == id);
        }
    }
}
