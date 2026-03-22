using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QuarentenarioWeb.Data;
using QuarentenarioWeb.Models;

namespace QuarentenarioWeb.Pages.TiposPatogeno
{
    public class DeleteModel : PageModel
    {
        private readonly QuarentenarioWeb.Data.QuarentenarioContext _context;

        public DeleteModel(QuarentenarioWeb.Data.QuarentenarioContext context)
        {
            _context = context;
        }

        [BindProperty]
        public TipoPatogeno TipoPatogeno { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipopatogeno = await _context.TipoPatogenos.FirstOrDefaultAsync(m => m.Id == id);

            if (tipopatogeno is not null)
            {
                TipoPatogeno = tipopatogeno;

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

            var tipopatogeno = await _context.TipoPatogenos.FindAsync(id);
            if (tipopatogeno != null)
            {
                TipoPatogeno = tipopatogeno;
                _context.TipoPatogenos.Remove(TipoPatogeno);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    ModelState.Remove($"{nameof(TipoPatogeno)}.{nameof(TipoPatogeno.Nome)}");
                    // Log the exception (ex) as needed
                    ModelState.AddModelError(string.Empty, "Não foi possível excluir o tipo de patógeno. Ele pode estar associado a outros registros.");
                    return Page();
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
