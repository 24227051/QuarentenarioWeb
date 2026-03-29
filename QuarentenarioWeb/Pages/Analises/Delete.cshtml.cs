using System;
using System.Collections.Generic;
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

            var analisedetalhe = await _context.Analises
                .Include(p => p.IdPatogenoNavigation)
                .Include(p => p.IdBoletimNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (analisedetalhe is not null)
            {
                Analise = analisedetalhe;

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

            var analisedetalhe = await _context.Analises
                .Include(p => p.IdPatogenoNavigation)
                .Include(p => p.IdBoletimNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (analisedetalhe != null)
            {
                Analise = analisedetalhe;
                _context.Analises.Remove(Analise);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    ModelState.Remove($"{nameof(Analise)}.{nameof(Analise.Descricao)}");
                    // Log the exception (ex) as needed
                    ModelState.AddModelError(string.Empty, "Não foi possível excluir o controle. Ele pode estar relacionado a outros dados.");
                    return Page();
                }
            }

            return RedirectToPage("./Index", new { id = Analise.IdBoletim });
        }
    }
}
