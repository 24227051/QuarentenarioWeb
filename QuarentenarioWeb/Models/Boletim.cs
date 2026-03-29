using System;
using System.Collections.Generic;

namespace QuarentenarioWeb.Models;

public partial class Boletim
{
    public int Id { get; set; }

    public int IdMaterial { get; set; }

    public int IdPais { get; set; }

    public string Descricao { get; set; } = null!;

    public DateTime DataInicio { get; set; }

    public DateTime? DataTermino { get; set; }

    public bool Finalizada { get; set; }

    public bool Positivo { get; set; }

    public virtual ICollection<AnaliseDetalhe> AnaliseDetalhes { get; set; } = new List<AnaliseDetalhe>();

    public virtual ICollection<Anexo> Anexos { get; set; } = new List<Anexo>();

    public virtual Material IdMaterialNavigation { get; set; } = null!;

    public virtual Pai IdPaisNavigation { get; set; } = null!;
}
