using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QuarentenarioWeb.Data;
using QuarentenarioWeb.Models;

namespace QuarentenarioWeb.Pages.Patogenos
{
    public class IndexModel : PageModel
    {
        private readonly QuarentenarioWeb.Data.QuarentenarioContext _context;

        public IndexModel(QuarentenarioWeb.Data.QuarentenarioContext context)
        {
            _context = context;
        }

        public IList<Patogeno> Patogeno { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Patogeno = await _context.Patogenos
                .Include(p => p.IdTipoControleNavigation)
                .Include(p => p.IdTipoPatogenoNavigation).ToListAsync();
        }
    }
}
