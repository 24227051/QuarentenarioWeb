using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuarentenarioWeb.Models;

public partial class Patogeno
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    [Display(Name = "Id Tipo Patógeno")]
    public int IdTipoPatogeno { get; set; }

    [Display(Name = "Id Tipo Controle")]
    public int IdTipoControle { get; set; }

    public virtual ICollection<AnaliseDetalhe> AnaliseDetalhes { get; set; } = new List<AnaliseDetalhe>();

    [ValidateNever]
    [Display(Name = "Tipo Controle")]
    public virtual TipoControle IdTipoControleNavigation { get; set; } = null!;

    [ValidateNever]
    [Display(Name = "Tipo Patógeno")]
    public virtual TipoPatogeno IdTipoPatogenoNavigation { get; set; } = null!;

    public virtual ICollection<Material> IdMaterials { get; set; } = new List<Material>();
}
