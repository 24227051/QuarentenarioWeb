using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QuarentenarioWeb.Models;

namespace QuarentenarioWeb.Pages.MateriaisPatogenos
{
    public class DeleteModel : PageModel
    {
        public async Task<IActionResult> OnGetAsync(int? idPatogeno, int? idMaterial)
        {
            if (idPatogeno == null || idMaterial == null)
            {
                return NotFound();
            }

            //var analisedetalhe = await _context.AnaliseDetalhes.FirstOrDefaultAsync(m => m.Id == id);

            //if (analisedetalhe is not null)
            //{
            //    AnaliseDetalhe = analisedetalhe;

            //    return Page();
            //}

            return NotFound();
        }
    }
}
