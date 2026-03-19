using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
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

        public IActionResult OnGet()
        {
            PopularControles();
            return Page();
        }

        private void PopularControles()
        {
            ViewData["IdAnalise"] = new SelectList(_context.Analises, "Id", "Descricao");
            ViewData["IdPatogeno"] = new SelectList(_context.Patogenos, "Id", "Nome");
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
