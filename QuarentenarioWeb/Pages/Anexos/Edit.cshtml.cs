using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using QuarentenarioWeb.Data;
using QuarentenarioWeb.Models;

namespace QuarentenarioWeb.Pages.Anexos
{
    public class EditModel : PageModel
    {
        private readonly QuarentenarioWeb.Data.QuarentenarioContext _context;
        private readonly IWebHostEnvironment _environment;

        public EditModel(QuarentenarioWeb.Data.QuarentenarioContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        [BindProperty]
        public Anexo Anexo { get; set; } = default!;

        [BindProperty]
        public IFormFile? Upload { get; set; }

        [BindProperty]
        public int? IdAnalise { get; set; }

        [BindProperty]
        public int? IdAnaliseDetalhe { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var anexo = await _context.Anexos.FirstOrDefaultAsync(m => m.Id == id);
            if (anexo == null)
            {
                return NotFound();
            }

            IdAnalise = anexo.IdAnalise;
            IdAnaliseDetalhe = anexo.IdAnaliseDetalhe;

            Anexo = anexo;
            PopularControles();
            return Page();
        }

        private void PopularControles()
        {
            ViewData["IdAnaliseDetalhe"] = new SelectList(_context.AnaliseDetalhes, "Id", "Descricao");
            ViewData["IdAnalise"] = new SelectList(_context.Analises, "Id", "Descricao");
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            ModelState.Remove($"{nameof(Anexo)}.{nameof(Anexo.NomeArmazenado)}");
            ModelState.Remove($"{nameof(Anexo)}.{nameof(Anexo.NomeArquivo)}");
            ModelState.Remove($"{nameof(Anexo)}.{nameof(Anexo.TipoConteudo)}");

            if (!ModelState.IsValid)
            {
                PopularControles();
                return Page();
            }

            var existing = await _context.Anexos.AsNoTracking().FirstOrDefaultAsync(a => a.Id == Anexo.Id);
            if (existing == null)
            {
                return NotFound();
            }

            // If a new file was uploaded, replace stored file
            if (Upload != null && Upload.Length > 0)
            {
                var originalFileName = Path.GetFileName(Upload.FileName);
                var storedFileName = Guid.NewGuid().ToString("N") + Path.GetExtension(originalFileName);

                // Save new file
                var storageDir = Path.Combine(_environment.ContentRootPath, "App_Data", "Anexos");
                if (!Directory.Exists(storageDir)) Directory.CreateDirectory(storageDir);
                var newFilePath = Path.Combine(storageDir, storedFileName);
                using (var stream = System.IO.File.Create(newFilePath))
                {
                    await Upload.CopyToAsync(stream);
                }

                // delete old file if exists
                if (!string.IsNullOrEmpty(existing.NomeArmazenado))
                {
                    var oldPath = Path.Combine(storageDir, existing.NomeArmazenado);
                    try { if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath); } catch { }
                }

                // update Anexo fields
                Anexo.NomeArquivo = originalFileName;
                Anexo.NomeArmazenado = storedFileName;
                Anexo.TipoConteudo = Upload.ContentType ?? string.Empty;
            }
            else
            {
                // keep previous file info
                Anexo.NomeArquivo = existing.NomeArquivo;
                Anexo.NomeArmazenado = existing.NomeArmazenado;
                Anexo.TipoConteudo = existing.TipoConteudo;
            }

            _context.Attach(Anexo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!AnexoExists(Anexo.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError(string.Empty, "Não foi possível incluir o anexo. Deverá se escolhida uma análise ou uma análise detalhe.");
                //+ (ex.InnerException?.Message ?? ex.Message));
                PopularControles();
                return Page();
            }

            return RedirectToPage("./Index", new { idAnalise = IdAnalise, idAnaliseDetalhe = IdAnaliseDetalhe });
        }

        private bool AnexoExists(int id)
        {
            return _context.Anexos.Any(e => e.Id == id);
        }
    }
}
