using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QuarentenarioWeb.Data;
using QuarentenarioWeb.Models;

namespace QuarentenarioWeb.Pages.Anexos
{
    public class IndexModel : PageModel
    {
        private readonly QuarentenarioWeb.Data.QuarentenarioContext _context;

        public IndexModel(QuarentenarioWeb.Data.QuarentenarioContext context)
        {
            _context = context;
        }

        public IList<Anexo> Anexo { get;set; } = default!;

        public int? IdAnalise { get; set; }

        public int? IdAnaliseDetalhe { get; set; }

        public async Task<IActionResult> OnGetAsync(int? idAnalise, int? idAnaliseDetalhe)
        {
            if (idAnalise == null  && idAnaliseDetalhe == null)
            {
                return NotFound();
            }

            IdAnalise = idAnalise;
            IdAnaliseDetalhe = idAnaliseDetalhe;

            // Filtra os anexos com base no ID da análise ou no ID do detalhe da análise, se fornecidos
            Anexo = await _context.Anexos
                .Include(a => a.IdAnaliseDetalheNavigation)
                .Include(a => a.IdAnaliseNavigation)
                .Where(a => (idAnalise != null && a.IdAnalise == idAnalise) || (idAnaliseDetalhe != null && a.IdAnaliseDetalhe == idAnaliseDetalhe))
                .ToListAsync();

            if (Anexo == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
