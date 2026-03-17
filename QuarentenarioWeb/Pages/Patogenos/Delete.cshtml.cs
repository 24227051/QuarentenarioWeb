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
    public class DeleteModel : PageModel
    {
        private readonly QuarentenarioWeb.Data.QuarentenarioContext _context;

        public DeleteModel(QuarentenarioWeb.Data.QuarentenarioContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Patogeno Patogeno { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patogeno = await _context.Patogenos.FirstOrDefaultAsync(m => m.Id == id);

            if (patogeno is not null)
            {
                Patogeno = patogeno;

                return Page();
            }

            return NotFound();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var patogeno = await _context.Patogenos.FindAsync(id);
            if (patogeno != null)
            {
                Patogeno = patogeno;
                _context.Patogenos.Remove(Patogeno);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
