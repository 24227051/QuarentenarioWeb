using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using QuarentenarioWeb.Data;
using QuarentenarioWeb.Models;

namespace QuarentenarioWeb.Pages.Anexos
{
    [Authorize]
    public class DownloadModel : PageModel
    {
        private readonly QuarentenarioContext _context;
        private readonly IWebHostEnvironment _environment;

        public DownloadModel(QuarentenarioContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var anexo = await _context.Anexos.FirstOrDefaultAsync(a => a.Id == id.Value);
            if (anexo == null)
            {
                return NotFound();
            }

            // Additional authorization/validation can be done here (e.g. check ownership, roles)
            if (!User.Identity?.IsAuthenticated ?? true)
            {
                return Challenge();
            }

            var storageDir = Path.Combine(_environment.ContentRootPath, "App_Data", "Anexos");
            var filePath = Path.Combine(storageDir, anexo.NomeArmazenado ?? string.Empty);

            // Prevent path traversal by normalizing and ensuring the file is inside storageDir
            var fullStorageDir = Path.GetFullPath(storageDir);
            var fullFilePath = Path.GetFullPath(filePath);
            if (!fullFilePath.StartsWith(fullStorageDir, StringComparison.OrdinalIgnoreCase))
            {
                return BadRequest();
            }

            if (!System.IO.File.Exists(fullFilePath))
            {
                return NotFound();
            }

            var contentType = string.IsNullOrEmpty(anexo.TipoConteudo) ? "application/octet-stream" : anexo.TipoConteudo;
            var downloadName = string.IsNullOrEmpty(anexo.NomeArquivo) ? anexo.NomeArmazenado : anexo.NomeArquivo;

            return PhysicalFile(fullFilePath, contentType, downloadName);
        }
    }
}
