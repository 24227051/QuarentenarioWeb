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

namespace QuarentenarioWeb.Pages.AnalisesDetalhes
{
    public class EditModel : PageModel
    {
        private readonly QuarentenarioWeb.Data.QuarentenarioContext _context;

        public EditModel(QuarentenarioWeb.Data.QuarentenarioContext context)
        {
            _context = context;
        }

        [BindProperty]
        public AnaliseDetalhe AnaliseDetalhe { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var analisedetalhe =  await _context.AnaliseDetalhes.FirstOrDefaultAsync(m => m.Id == id);
            if (analisedetalhe == null)
            {
                return NotFound();
            }
            AnaliseDetalhe = analisedetalhe;
           ViewData["IdAnalise"] = new SelectList(_context.Analises, "Id", "Id");
           ViewData["IdPatogeno"] = new SelectList(_context.Patogenos, "Id", "Id");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(AnaliseDetalhe).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnaliseDetalheExists(AnaliseDetalhe.Id))
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

        private bool AnaliseDetalheExists(int id)
        {
            return _context.AnaliseDetalhes.Any(e => e.Id == id);
        }
    }
}
