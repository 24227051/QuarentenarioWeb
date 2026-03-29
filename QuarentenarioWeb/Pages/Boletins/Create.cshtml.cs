using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuarentenarioWeb.Data;
using QuarentenarioWeb.Models;

namespace QuarentenarioWeb.Pages.Boletins
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
        public Boletim Boletim { get; set; } = default!;

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
                .Where(m => m.Id == Boletim.IdMaterial)
                .Select(m => m.IdPatogenos)
                .FirstOrDefault()?.ToList() ?? new List<Patogeno>();


            ICollection<Analise> analises = new List<Analise>();

            // Para cada Patogeno associado ao Material, cria um novo Analise e adiciona à coleção de Analises da Boletim
            foreach (Patogeno patogeno in idPatogenos)
            {
                Analise analise = new Analise
                {
                    IdPatogeno = patogeno.Id,
                    Descricao = "Análise: " + patogeno.Nome,
                    DataInicio = DateTime.Now,
                    Finalizada = false,
                    Positivo = false
                };
                analises.Add(analise);
            }

            Boletim.Analises = analises;
            _context.Boletims.Add(Boletim);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
