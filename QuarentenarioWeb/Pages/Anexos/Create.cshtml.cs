using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuarentenarioWeb.Data;
using QuarentenarioWeb.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace QuarentenarioWeb.Pages.Anexos
{
    public class CreateModel : PageModel
    {
        private readonly QuarentenarioWeb.Data.QuarentenarioContext _context;
        private readonly IWebHostEnvironment _environment;

        public CreateModel(QuarentenarioWeb.Data.QuarentenarioContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public IActionResult OnGet()
        {
            PopularControles();
            return Page();
        }

        [BindProperty]
        public Anexo Anexo { get; set; } = default!;

        [BindProperty]
        public IFormFile? Upload { get; set; }

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            // Validate file upload
            if (Upload == null || Upload.Length == 0)
            {
                ModelState.AddModelError("Upload", "Selecione um arquivo para upload.");
            }

            ModelState.Remove($"{nameof(Anexo)}.{nameof(Anexo.NomeArmazenado)}");
            ModelState.Remove($"{nameof(Anexo)}.{nameof(Anexo.NomeArquivo)}");
            ModelState.Remove($"{nameof(Anexo)}.{nameof(Anexo.TipoConteudo)}");

            if (!ModelState.IsValid)
            {
                PopularControles();
                return Page();
            }

            // populate Anexo properties from uploaded file
            var originalFileName = Path.GetFileName(Upload!.FileName);
            var storedFileName = Guid.NewGuid().ToString("N") + Path.GetExtension(originalFileName);

            Anexo.NomeArquivo = originalFileName;
            Anexo.NomeArmazenado = storedFileName;
            Anexo.TipoConteudo = Upload.ContentType ?? string.Empty;

            // Save file to App_Data/Anexos inside content root (not under wwwroot)
            var storageDir = Path.Combine(_environment.ContentRootPath, "App_Data", "Anexos");
            if (!Directory.Exists(storageDir))
            {
                Directory.CreateDirectory(storageDir);
            }

            var filePath = Path.Combine(storageDir, storedFileName);
            using (var stream = System.IO.File.Create(filePath))
            {
                await Upload.CopyToAsync(stream);
            }

            _context.Anexos.Add(Anexo);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
            {
                ModelState.AddModelError(string.Empty, "Não foi possível incluir o anexo. Deverá se escolhida uma análise ou uma análise detalhe.");
                //+ (ex.InnerException?.Message ?? ex.Message));
                PopularControles();
                return Page();
            }

            return RedirectToPage("./Index");
        }

        private void PopularControles()
        {
            ViewData["IdAnaliseDetalhe"] = new SelectList(_context.AnaliseDetalhes, "Id", "Descricao");
            ViewData["IdAnalise"] = new SelectList(_context.Analises, "Id", "Descricao");
        }
    }
}
