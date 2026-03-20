using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QuarentenarioWeb.Models;

namespace QuarentenarioWeb.Pages.MateriaisPatogenos
{
    public class IndexModel : PageModel
    {
        private readonly QuarentenarioWeb.Data.QuarentenarioContext _context;

        public IndexModel(QuarentenarioWeb.Data.QuarentenarioContext context)
        {
            _context = context;
        }

        public IList<Patogeno> Patogenos { get; set; } = default!;
        public int? IdMaterial { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            IdMaterial = id;

            // Carrega os patógenos associados ao material de acordo com o ID do material fornecido
            Patogenos = await _context.Patogenos
                .Where(p => p.IdMaterials.Any(m => m.Id == IdMaterial))
                .Include(p => p.IdTipoControleNavigation)
                .Include(p => p.IdTipoPatogenoNavigation)
                .ToListAsync(); ;

            if (Patogenos == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
