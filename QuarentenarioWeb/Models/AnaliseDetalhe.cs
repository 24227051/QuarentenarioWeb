using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System;
using System.Collections.Generic;

namespace QuarentenarioWeb.Models;

public partial class AnaliseDetalhe
{
    public int Id { get; set; }

    public int IdAnalise { get; set; }

    public int IdPatogeno { get; set; }

    public string Descricao { get; set; } = null!;

    public DateTime DataInicio { get; set; }

    public DateTime? DataTermino { get; set; }

    public bool Finalizada { get; set; }

    public virtual ICollection<Anexo> Anexos { get; set; } = new List<Anexo>();

    [ValidateNever]
    public virtual Analise IdAnaliseNavigation { get; set; } = null!;

    [ValidateNever]
    public virtual Patogeno IdPatogenoNavigation { get; set; } = null!;
}
