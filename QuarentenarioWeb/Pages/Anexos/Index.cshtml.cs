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

        public async Task OnGetAsync()
        {
            Anexo = await _context.Anexos
                .Include(a => a.IdAnaliseDetalheNavigation)
                .Include(a => a.IdAnaliseNavigation).ToListAsync();
        }
    }
}
