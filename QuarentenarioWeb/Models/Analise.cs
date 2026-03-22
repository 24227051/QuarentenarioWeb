using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuarentenarioWeb.Models;

public partial class Analise
{
    public int Id { get; set; }

    [Display(Name = "Id Material")]
    public int IdMaterial { get; set; }

    [Display(Name = "Id Cliente")]
    public int IdCliente { get; set; }

    public string Descricao { get; set; } = null!;

    public DateTime DataInicio { get; set; }

    public DateTime? DataTermino { get; set; }

    public bool Finalizada { get; set; }

    public virtual ICollection<AnaliseDetalhe> AnaliseDetalhes { get; set; } = new List<AnaliseDetalhe>();

    public virtual ICollection<Anexo> Anexos { get; set; } = new List<Anexo>();

    [ValidateNever]
    [Display(Name = "Cliente")]
    public virtual Cliente IdClienteNavigation { get; set; } = null!;

    [ValidateNever]
    [Display(Name = "Material")]
    public virtual Material IdMaterialNavigation { get; set; } = null!;
}
