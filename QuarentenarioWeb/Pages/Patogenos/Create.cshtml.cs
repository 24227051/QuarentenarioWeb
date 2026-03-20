using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using QuarentenarioWeb.Data;
using QuarentenarioWeb.Models;

namespace QuarentenarioWeb.Pages.Patogenos
{
    public class CreateModel : PageModel
    {
        private readonly QuarentenarioWeb.Data.QuarentenarioContext _context;

        public CreateModel(QuarentenarioWeb.Data.QuarentenarioContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            PopularControles();
            return Page();
        }

        private void PopularControles()
        {
            ViewData["IdTipoControle"] = new SelectList(_context.TipoControles, "Id", "Nome");
            ViewData["IdTipoPatogeno"] = new SelectList(_context.TipoPatogenos, "Id", "Nome");
        }

        [BindProperty]
        public Patogeno Patogeno { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                PopularControles();
                return Page();
            }

            // Normaliza nome para comparação consistente
            var nomeNormalizado = (Patogeno.Nome ?? string.Empty).Trim();

            // Verifica existência (consulta rápida usando índice)
            bool existe = await _context.Patogenos
                .AnyAsync(p =>
                    p.Nome == nomeNormalizado
                    && p.IdTipoPatogeno == Patogeno.IdTipoPatogeno
                    && p.IdTipoControle == Patogeno.IdTipoControle);

            if (existe)
            {
                ModelState.AddModelError("Patogeno.Nome", "Já existe um patógeno com esse nome e tipos selecionados.");
                PopularControles();
                return Page();
            }

            // Aplica normalização antes de salvar
            Patogeno.Nome = nomeNormalizado;

            try
            {
                _context.Patogenos.Add(Patogeno);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException sqlEx && (sqlEx.Number == 2601 || sqlEx.Number == 2627))
            {
                // 2601/2627 = violação de índice único (tratamento para condição de corrida)
                ModelState.AddModelError(string.Empty, "Não foi possível salvar: registro já existe (concorrência).");
                PopularControles();
                return Page();
            }

            return RedirectToPage("./Index");
        }
    }
}
