using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QuarentenarioWeb.Data;
using QuarentenarioWeb.Models;

namespace QuarentenarioWeb.Pages.TiposControle
{
    public class DetailsModel : PageModel
    {
        private readonly QuarentenarioWeb.Data.QuarentenarioContext _context;

        public DetailsModel(QuarentenarioWeb.Data.QuarentenarioContext context)
        {
            _context = context;
        }

        public TipoControle TipoControle { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var tipocontrole = await _context.TipoControles.FirstOrDefaultAsync(m => m.Id == id);

            if (tipocontrole is not null)
            {
                TipoControle = tipocontrole;

                return Page();
            }

            return NotFound();
        }
    }
}
