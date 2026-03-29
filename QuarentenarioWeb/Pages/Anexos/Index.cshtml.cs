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

        [BindProperty]
        public int? IdBoletim { get; set; }

        [BindProperty]
        public int? IdAnaliseDetalhe { get; set; }

        public async Task<IActionResult> OnGetAsync(int? idBoletim, int? idAnaliseDetalhe)
        {
            if (idBoletim == null  && idAnaliseDetalhe == null)
            {
                return NotFound();
            }

            IdBoletim = idBoletim;
            IdAnaliseDetalhe = idAnaliseDetalhe;

            // Filtra os anexos com base no ID da análise ou no ID do detalhe da análise, se fornecidos
            if (IdBoletim != null)
            {
                Anexo = await _context.Anexos
                                .Include(a => a.IdAnaliseDetalheNavigation)
                                .Include(a => a.IdBoletimNavigation)
                                .Where(a => a.IdBoletim == IdBoletim)
                                .ToListAsync();
            }

            if (IdAnaliseDetalhe != null)
            {
                Anexo = await _context.Anexos
                                .Include(a => a.IdAnaliseDetalheNavigation)
                                .Include(a => a.IdBoletimNavigation)
                                .Where(a => a.IdAnaliseDetalhe == IdAnaliseDetalhe)
                                .ToListAsync();
            }




            if (Anexo == null)
            {
                return NotFound();
            }

            return Page();
        }
    }
}
