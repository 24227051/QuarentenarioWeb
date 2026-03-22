using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QuarentenarioWeb.Data;
using QuarentenarioWeb.Models;

namespace QuarentenarioWeb.Pages.AnalisesDetalhes
{
    public class DetailsModel : PageModel
    {
        private readonly QuarentenarioWeb.Data.QuarentenarioContext _context;

        public DetailsModel(QuarentenarioWeb.Data.QuarentenarioContext context)
        {
            _context = context;
        }

        public AnaliseDetalhe AnaliseDetalhe { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var analisedetalhe = await _context.AnaliseDetalhes
                .Include(p => p.IdPatogenoNavigation)
                .Include(p => p.IdAnaliseNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (analisedetalhe is not null)
            {
                AnaliseDetalhe = analisedetalhe;

                return Page();
            }

            return NotFound();
        }
    }
}
