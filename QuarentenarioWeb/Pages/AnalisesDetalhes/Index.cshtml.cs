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
    public class IndexModel : PageModel
    {
        private readonly QuarentenarioWeb.Data.QuarentenarioContext _context;

        public IndexModel(QuarentenarioWeb.Data.QuarentenarioContext context)
        {
            _context = context;
        }

        public IList<AnaliseDetalhe> AnaliseDetalhe { get;set; } = default!;
        public int? IdAnalise { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            IdAnalise = id;

            // Carrega os detalhes da análise, incluindo as informações do patógeno e da análise
            // de acordo com o ID da análise fornecido
            AnaliseDetalhe = await _context.AnaliseDetalhes
                .Where(a => a.IdAnalise == id)
                .Include(a => a.IdAnaliseNavigation)
                .Include(a => a.IdPatogenoNavigation).ToListAsync();

            if (AnaliseDetalhe == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
