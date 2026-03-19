using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QuarentenarioWeb.Data;
using QuarentenarioWeb.Models;

namespace QuarentenarioWeb.Pages.Analises
{
    public class DeleteModel : PageModel
    {
        private readonly QuarentenarioWeb.Data.QuarentenarioContext _context;

        public DeleteModel(QuarentenarioWeb.Data.QuarentenarioContext context)
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

            if (analise is not null)
            {
                Analise = analise;

                return Page();
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var analise = await _context.Analises.FindAsync(id);
            if (analise != null)
            {
                Analise = analise;
                _context.Analises.Remove(Analise);

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    ModelState.Remove($"{nameof(Analise)}.{nameof(Analise.Descricao)}");
                    // Fornece feedback simples na página caso algo falhe
                    ModelState.AddModelError(string.Empty, "Não foi possível excluir a análise: " + (ex.InnerException?.Message ?? ex.Message));
                    Analise = analise;
                    return Page();
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
