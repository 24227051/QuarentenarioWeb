using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QuarentenarioWeb.Data;
using QuarentenarioWeb.Models;

namespace QuarentenarioWeb.Pages.Paises
{
    public class DeleteModel : PageModel
    {
        private readonly QuarentenarioWeb.Data.QuarentenarioContext _context;

        public DeleteModel(QuarentenarioWeb.Data.QuarentenarioContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Pai Pais { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pais = await _context.Pais.FirstOrDefaultAsync(m => m.Id == id);

            if (pais is not null)
            {
                Pais = pais;

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

            var pais = await _context.Pais.FindAsync(id);
            if (pais != null)
            {
                Pais = pais;
                _context.Pais.Remove(Pais);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    ModelState.Remove($"{nameof(Pais)}.{nameof(Pais.Nome)}");
                    // Log the exception (ex) as needed
                    ModelState.AddModelError(string.Empty, "Não foi possível excluir o pais. Ele pode estar associado a outras entidades.");
                    return Page();
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
