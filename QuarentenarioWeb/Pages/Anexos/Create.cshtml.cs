using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuarentenarioWeb.Data;
using QuarentenarioWeb.Models;

namespace QuarentenarioWeb.Pages.Anexos
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
        ViewData["IdAnaliseDetalhe"] = new SelectList(_context.AnaliseDetalhes, "Id", "Id");
        ViewData["IdAnalise"] = new SelectList(_context.Analises, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public Anexo Anexo { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Anexos.Add(Anexo);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
