using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QuarentenarioWeb.Data;
using QuarentenarioWeb.Models;

namespace QuarentenarioWeb.Pages.Anexos
{
    public class DeleteModel : PageModel
    {
        private readonly QuarentenarioWeb.Data.QuarentenarioContext _context;

        public DeleteModel(QuarentenarioWeb.Data.QuarentenarioContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Anexo Anexo { get; set; } = default!;

        [BindProperty]
        public int? IdAnalise { get; set; }

        [BindProperty]
        public int? IdAnaliseDetalhe { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var anexo = await _context.Anexos
                .Include(p => p.IdAnaliseNavigation)
                .Include(p => p.IdAnaliseDetalheNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (anexo == null)
            {
                return NotFound();
            }

            IdAnalise = anexo.IdAnalise;
            IdAnaliseDetalhe = anexo.IdAnaliseDetalhe;

            Anexo = anexo;

            if (IdAnalise == null)
            {
                IdAnalise = Anexo.IdAnaliseDetalheNavigation!.IdAnalise;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var anexo = await _context.Anexos
                .Include(p => p.IdAnaliseNavigation)
                .Include(p => p.IdAnaliseDetalheNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (anexo != null)
            {
                Anexo = anexo;
                _context.Anexos.Remove(Anexo);
                await _context.SaveChangesAsync();
            }

            if (Anexo.IdAnalise == null)
            {
                Anexo.IdAnalise = Anexo.IdAnaliseDetalheNavigation!.IdAnalise;
            }

            return RedirectToPage("./Index", new { idAnalise = Anexo.IdAnalise, idAnaliseDetalhe = Anexo.IdAnaliseDetalhe });
        }
    }
}
