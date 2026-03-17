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

namespace QuarentenarioWeb.Pages.Anexos
{
    public class EditModel : PageModel
    {
        private readonly QuarentenarioWeb.Data.QuarentenarioContext _context;

        public EditModel(QuarentenarioWeb.Data.QuarentenarioContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Anexo Anexo { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var anexo =  await _context.Anexos.FirstOrDefaultAsync(m => m.Id == id);
            if (anexo == null)
            {
                return NotFound();
            }
            Anexo = anexo;
           ViewData["IdAnaliseDetalhe"] = new SelectList(_context.AnaliseDetalhes, "Id", "Id");
           ViewData["IdAnalise"] = new SelectList(_context.Analises, "Id", "Id");
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

            _context.Attach(Anexo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnexoExists(Anexo.Id))
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

        private bool AnexoExists(int id)
        {
            return _context.Anexos.Any(e => e.Id == id);
        }
    }
}
