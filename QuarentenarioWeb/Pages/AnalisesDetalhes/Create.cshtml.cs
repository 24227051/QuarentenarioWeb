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
    public class CreateModel : PageModel
    {
        private readonly QuarentenarioWeb.Data.QuarentenarioContext _context;

        public CreateModel(QuarentenarioWeb.Data.QuarentenarioContext context)
        {
            _context = context;
        }

        public string? AnaliseDescricao { get; set; }

        public IList<Patogeno> Patogenos { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Load the analysis to get the associated material
            var analise = await _context.Analises
                .Include(a => a.IdMaterialNavigation)
                .FirstOrDefaultAsync(a => a.Id == id.Value);

            if (analise == null)
            {
                return NotFound();
            }

            AnaliseDescricao = analise.Descricao;

            // Initialize the bound model so the hidden field has the analysis id
            AnaliseDetalhe = new AnaliseDetalhe { IdAnalise = analise.Id };

            // Populate patogeno select list filtered by material associated to this analysis
            var materialId = analise.IdMaterial;
            Patogenos = await _context.Patogenos
                .Where(p => p.IdMaterials.Any(m => m.Id == materialId))
                .ToListAsync();

            //ViewData["IdPatogeno"] = new SelectList(patogenos, "Id", "Nome");
            PopularControles();

            return Page();
        }

        private void PopularControles()
        {
            ViewData["IdPatogeno"] = new SelectList(Patogenos, "Id", "Nome");
        }

        [BindProperty]
        public AnaliseDetalhe AnaliseDetalhe { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                PopularControles();
                return Page();
            }

            _context.AnaliseDetalhes.Add(AnaliseDetalhe);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index", new { id = AnaliseDetalhe.IdAnalise });
        }
    }
}
