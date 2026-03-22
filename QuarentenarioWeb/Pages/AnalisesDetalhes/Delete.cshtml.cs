using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QuarentenarioWeb.Data;
using QuarentenarioWeb.Models;

namespace QuarentenarioWeb.Pages.AnalisesDetalhes
{
    public class DeleteModel : PageModel
    {
        private readonly QuarentenarioWeb.Data.QuarentenarioContext _context;

        public DeleteModel(QuarentenarioWeb.Data.QuarentenarioContext context)
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

            var analisedetalhe = await _context.AnaliseDetalhes
                .Include(p => p.IdPatogenoNavigation)
                .Include(p => p.IdAnaliseNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (analisedetalhe is not null)
            {
                AnaliseDetalhe = analisedetalhe;

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

            var analisedetalhe = await _context.AnaliseDetalhes
                .Include(p => p.IdPatogenoNavigation)
                .Include(p => p.IdAnaliseNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (analisedetalhe != null)
            {
                AnaliseDetalhe = analisedetalhe;
                _context.AnaliseDetalhes.Remove(AnaliseDetalhe);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    ModelState.Remove($"{nameof(AnaliseDetalhe)}.{nameof(AnaliseDetalhe.Descricao)}");
                    // Log the exception (ex) as needed
                    ModelState.AddModelError(string.Empty, "Não foi possível excluir o controle. Ele pode estar relacionado a outros dados.");
                    return Page();
                }
            }

            return RedirectToPage("./Index", new { id = AnaliseDetalhe.IdAnalise });
        }
    }
}
