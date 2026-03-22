using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QuarentenarioWeb.Data;
using QuarentenarioWeb.Models;

namespace QuarentenarioWeb.Pages.Materiais
{
    public class DeleteModel : PageModel
    {
        private readonly QuarentenarioWeb.Data.QuarentenarioContext _context;

        public DeleteModel(QuarentenarioWeb.Data.QuarentenarioContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Material Material { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var material = await _context.Materials.FirstOrDefaultAsync(m => m.Id == id);

            if (material is not null)
            {
                Material = material;

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

            var material = await _context.Materials.FindAsync(id);
            if (material != null)
            {
                Material = material;
                _context.Materials.Remove(Material);
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateException ex)
                {
                    ModelState.Remove($"{nameof(Material)}.{nameof(Material.Nome)}");
                    // Log the exception (ex) as needed
                    ModelState.AddModelError(string.Empty, "Não foi possível excluir o material. Ele pode estar associado a outros registros.");
                    return Page();
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
