using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuarentenarioWeb.Data;
using QuarentenarioWeb.Models;

namespace QuarentenarioWeb.Pages.Analises
{
    public class CreateModel : PageModel
    {
        private readonly QuarentenarioWeb.Data.QuarentenarioContext _context;

        public CreateModel(QuarentenarioWeb.Data.QuarentenarioContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            PopularControles();
            return Page();
        }

        private void PopularControles()
        {
            ViewData["IdPais"] = new SelectList(_context.Pais, "Id", "Nome");
            ViewData["IdMaterial"] = new SelectList(_context.Materials, "Id", "Nome");
        }

        [BindProperty]
        public Analise Analise { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                PopularControles();
                return Page();
            }

            // Retorna o ICollection<Patogeno> IdPatogenos associados ao Material selecionado na analise
            IList<Patogeno> idPatogenos = _context.Materials
                .Where(m => m.Id == Analise.IdMaterial)
                .Select(m => m.IdPatogenos)
                .FirstOrDefault()?.ToList() ?? new List<Patogeno>();


            ICollection<AnaliseDetalhe> analiseDetalhes = new List<AnaliseDetalhe>();

            // Para cada Patogeno associado ao Material, cria um novo AnaliseDetalhe e adiciona à coleção de AnaliseDetalhes da Analise
            foreach (Patogeno patogeno in idPatogenos)
            {
                AnaliseDetalhe analiseDetalhe = new AnaliseDetalhe
                {
                    IdPatogeno = patogeno.Id,
                    Descricao = "Análise: " + patogeno.Nome,
                    DataInicio = DateTime.Now,
                    Finalizada = false,
                    Positivo = false
                };
                analiseDetalhes.Add(analiseDetalhe);
            }

            Analise.AnaliseDetalhes = analiseDetalhes;
            _context.Analises.Add(Analise);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
