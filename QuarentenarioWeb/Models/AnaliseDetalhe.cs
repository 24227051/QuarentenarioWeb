using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuarentenarioWeb.Models;

public partial class AnaliseDetalhe
{
    public int Id { get; set; }

    [Display(Name = "Id Boletim")]
    public int IdAnalise { get; set; }

    [Display(Name = "Id Patogeno")]
    public int IdPatogeno { get; set; }

    public string Descricao { get; set; } = null!;

    public DateTime DataInicio { get; set; }

    public DateTime? DataTermino { get; set; }

    public bool Finalizada { get; set; }

    public bool Positivo { get; set; } = false;

    public virtual ICollection<Anexo> Anexos { get; set; } = new List<Anexo>();

    [ValidateNever]
    [Display(Name = "Boletim")]
    public virtual Analise IdAnaliseNavigation { get; set; } = null!;

    [ValidateNever]
    [Display(Name = "Patogeno")]
    public virtual Patogeno IdPatogenoNavigation { get; set; } = null!;
}
