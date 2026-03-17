using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using QuarentenarioWeb.Data;
using QuarentenarioWeb.Models;

namespace QuarentenarioWeb.Pages.Patogenos
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
        ViewData["IdTipoControle"] = new SelectList(_context.TipoControles, "Id", "Id");
        ViewData["IdTipoPatogeno"] = new SelectList(_context.TipoPatogenos, "Id", "Id");
            return Page();
        }

        [BindProperty]
        public Patogeno Patogeno { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Patogenos.Add(Patogeno);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
