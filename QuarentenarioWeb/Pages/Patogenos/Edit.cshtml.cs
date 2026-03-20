using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QuarentenarioWeb.Data;
using QuarentenarioWeb.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QuarentenarioWeb.Pages.Patogenos
{
    public class EditModel : PageModel
    {
        private readonly QuarentenarioWeb.Data.QuarentenarioContext _context;

        public EditModel(QuarentenarioWeb.Data.QuarentenarioContext context)
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
            if (patogeno == null)
            {
                return NotFound();
            }
            Patogeno = patogeno;
            PopularControles();
            return Page();
        }

        private void PopularControles()
        {
            ViewData["IdTipoControle"] = new SelectList(_context.TipoControles, "Id", "Nome");
            ViewData["IdTipoPatogeno"] = new SelectList(_context.TipoPatogenos, "Id", "Nome");
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                PopularControles();
                return Page();
            }

            _context.Attach(Patogeno).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
            {
                // 2601/2627 = violação de índice único (tratamento para condição de corrida)
                ModelState.AddModelError(string.Empty, "Não foi possível salvar: Já existe um patógeno com esse nome e tipos selecionados.");
                PopularControles();
                return Page();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PatogenoExists(Patogeno.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool PatogenoExists(int id)
        {
            return _context.Patogenos.Any(e => e.Id == id);
        }
    }
}
