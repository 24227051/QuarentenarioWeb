using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;

namespace QuarentenarioWeb.Models;

public partial class Patogeno
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public int IdTipoPatogeno { get; set; }

    public int IdTipoControle { get; set; }

    public virtual ICollection<AnaliseDetalhe> AnaliseDetalhes { get; set; } = new List<AnaliseDetalhe>();

    [ValidateNever] 
    public virtual TipoControle IdTipoControleNavigation { get; set; } = null!;

    [ValidateNever]
    public virtual TipoPatogeno IdTipoPatogenoNavigation { get; set; } = null!;

    public virtual ICollection<Material> IdMaterials { get; set; } = new List<Material>();
}
