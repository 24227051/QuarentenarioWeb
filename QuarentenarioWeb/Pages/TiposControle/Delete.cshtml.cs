using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QuarentenarioWeb.Data;
using QuarentenarioWeb.Models;

namespace QuarentenarioWeb.Pages.TiposControle
{
    public class DeleteModel : PageModel
    {
        private readonly QuarentenarioWeb.Data.QuarentenarioContext _context;

        public DeleteModel(QuarentenarioWeb.Data.QuarentenarioContext context)
        {
            _context = context;
        }

        [BindProperty]
        public TipoControle TipoControle { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipocontrole = await _context.TipoControles.FirstOrDefaultAsync(m => m.Id == id);

            if (tipocontrole is not null)
            {
                TipoControle = tipocontrole;

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

            var tipocontrole = await _context.TipoControles.FindAsync(id);
            if (tipocontrole != null)
            {
                TipoControle = tipocontrole;
                _context.TipoControles.Remove(TipoControle);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    ModelState.Remove($"{nameof(TipoControle)}.{nameof(TipoControle.Nome)}");
                    // Log the exception (ex) as needed
                    ModelState.AddModelError(string.Empty, "Não foi possível excluir o tipo de controle. Ele pode estar associado a outros registros.");
                    return Page();
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
