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

        public string? BoletimDescricao { get; set; }

        public IList<Patogeno> Patogenos { get; set; } = default!;

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

            // Load the analysis to get the associated material
            var boletim = await _context.Boletims
                .Include(a => a.IdMaterialNavigation)
                .FirstOrDefaultAsync(a => a.Id == analise.IdBoletim);

            if (boletim == null)
            {
                return NotFound();
            }

            BoletimDescricao = boletim.Descricao;

            var materialId = boletim.IdMaterial;
            Patogenos = await _context.Patogenos
                .Where(p => p.IdMaterials.Any(m => m.Id == materialId))
                .ToListAsync();

            PopularControles();
            return Page();
        }

        private void PopularControles()
        {
            //ViewData["IdBoletim"] = new SelectList(_context.Boletins, "Id", "Descricao");
            ViewData["IdPatogeno"] = new SelectList(Patogenos, "Id", "Nome");
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
                if (!AnaliseDetalheExists(Analise.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            // Redireciona para a página de detalhes da análise após a edição passando o ID da análise
            return RedirectToPage("./Index", new { id = Analise.IdBoletim });
        }

        private bool AnaliseDetalheExists(int id)
        {
            return _context.Analises.Any(e => e.Id == id);
        }
    }
}
