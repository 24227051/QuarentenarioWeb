using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QuarentenarioWeb.Data;
using QuarentenarioWeb.Models;

namespace QuarentenarioWeb.Pages.Analises
{
    public class IndexModel : PageModel
    {
        private readonly QuarentenarioWeb.Data.QuarentenarioContext _context;

        public IndexModel(QuarentenarioWeb.Data.QuarentenarioContext context)
        {
            _context = context;
        }

        public IList<Analise> Analise { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Analise = await _context.Analises
                .Include(a => a.IdPaisNavigation)
                .Include(a => a.IdMaterialNavigation).ToListAsync();
        }
    }
}
