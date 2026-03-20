using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using QuarentenarioWeb.Models;

namespace QuarentenarioWeb.Pages.MateriaisPatogenos
{
    public class CreateModel : PageModel
    {
        private readonly QuarentenarioWeb.Data.QuarentenarioContext _context;

        public CreateModel(QuarentenarioWeb.Data.QuarentenarioContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Material? Material { get; set; } = default!;

        [BindProperty]
        public IList<Patogeno> Patogenos { get; set; } = new List<Patogeno>();

        // Propriedade escalar para ligar ao <select> (permite escolher apenas 1)
        [BindProperty]
        public int? SelectedPatogenoId { get; set; }

        private void PopularControles()
        {
            ViewData["IdPatogeno"] = new SelectList(Patogenos, "Id", "Nome");
        }

        public async Task<IActionResult> OnGetAsync(int? idMaterial)
        {
            if (idMaterial == null)
            {
                return NotFound();
            }

            // Load the analysis to get the associated material
            Material = await _context.Materials
                .FirstOrDefaultAsync(a => a.Id == idMaterial);

            if (Material == null)
            {
                return NotFound();
            }

            Patogenos = await _context.Patogenos
                .OrderBy(p => p.Nome)
                .ToListAsync();

            PopularControles();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Validação básica do selecionado
            if (!SelectedPatogenoId.HasValue)
            {
                ModelState.AddModelError(nameof(SelectedPatogenoId), "Selecione um patógeno.");
            }

            if (Material == null || Material.Id == 0)
            {
                ModelState.AddModelError(string.Empty, "Material inválido.");
            }

            ModelState.Remove("Material.Nome");

            if (!ModelState.IsValid)
            {
                int materialID = Material.Id;

                Material = await _context.Materials
                                    .FirstOrDefaultAsync(a => a.Id == materialID);

                // Recarrega a lista para o select antes de retornar a página
                Patogenos = await _context.Patogenos.OrderBy(p => p.Nome).ToListAsync();
                PopularControles();
                return Page();
            }

            // Carrega a entidade Material do banco
            var materialEntity = await _context.Materials
                .Include(m => m.IdPatogenos)
                .FirstOrDefaultAsync(m => m.Id == Material!.Id);

            if (materialEntity == null)
            {
                return NotFound();
            }

            // Busca o patógeno selecionado
            var patogeno = await _context.Patogenos.FindAsync(SelectedPatogenoId.Value);
            if (patogeno == null)
            {
                ModelState.AddModelError(nameof(SelectedPatogenoId), "Patógeno não encontrado.");
                Patogenos = await _context.Patogenos.OrderBy(p => p.Nome).ToListAsync();
                PopularControles();
                return Page();
            }

            // Adiciona a relação (se ainda não existir)
            if (!materialEntity.IdPatogenos.Any(p => p.Id == patogeno.Id))
            {
                materialEntity.IdPatogenos.Add(patogeno);
                _context.Update(materialEntity);
                await _context.SaveChangesAsync();
            }

            // Redireciona para a lista/voltar — ajuste conforme sua navegação
            return RedirectToPage("./Index", new { id = materialEntity.Id });
        }
    }
}
