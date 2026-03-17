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
            ViewData["IdCliente"] = new SelectList(_context.Clientes, "Id", "Nome");
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

            _context.Analises.Add(Analise);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
