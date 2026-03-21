using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QuarentenarioWeb.Models;

namespace QuarentenarioWeb.Pages.MateriaisPatogenos
{
    public class DeleteModel : PageModel
    {
        private readonly QuarentenarioWeb.Data.QuarentenarioContext _context;

        public DeleteModel(QuarentenarioWeb.Data.QuarentenarioContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Patogeno Patogeno { get; set; } = default!;

        [BindProperty]
        public Material Material { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? idPatogeno, int? idMaterial)
        {
            if (idPatogeno == null || idMaterial == null)
            {
                return NotFound();
            }

            var material = await _context.Materials.FirstOrDefaultAsync(m => m.Id == idMaterial);
            var patogeno = await _context.Patogenos.Where(p => p.Id == idPatogeno).FirstOrDefaultAsync();
            if (material is not null && patogeno is not null)
            {
                Material = material;
                Patogeno = patogeno;
                return Page();
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var idMaterial = Material?.Id;
            var idPatogeno = Patogeno?.Id;

            if (idMaterial == null || idPatogeno == null)
                return NotFound();

            var patogeno = await _context.Materials
                .Include(m => m.IdPatogenos)
                .Where(m => m.Id == idMaterial)
                .SelectMany(m => m.IdPatogenos.Where(p => p.Id == idPatogeno))
                .FirstOrDefaultAsync();

            if (patogeno is not null)
            {     
                _context.Materials
                    .Include(m => m.IdPatogenos).FirstOrDefault(m => m.Id == idMaterial)?.IdPatogenos.Remove(patogeno);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index", new { id = idMaterial });
        }
    }
}
