using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuarentenarioWeb.Data;
using QuarentenarioWeb.Models;

namespace QuarentenarioWeb.Pages.Analises
{
    public class EditModel : PageModel
    {
        private readonly QuarentenarioWeb.Data.QuarentenarioContext _context;

        public EditModel(QuarentenarioWeb.Data.QuarentenarioContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Analise Analise { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var analise = await _context.Analises.FirstOrDefaultAsync(m => m.Id == id);
            if (analise == null)
            {
                return NotFound();
            }
            Analise = analise;
            PopularControles();
            return Page();
        }

        private void PopularControles()
        {
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "Id", "Nome");
            ViewData["IdMaterial"] = new SelectList(_context.Materials, "Id", "Nome");
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                PopularControles();
                return Page();
            }

            _context.Attach(Analise).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnaliseExists(Analise.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool AnaliseExists(int id)
        {
            return _context.Analises.Any(e => e.Id == id);
        }
    }
}
