using System;
using System.Collections.Generic;

namespace QuarentenarioWeb.Models;

public partial class Anexo
{
    public int Id { get; set; }

    public string NomeArquivo { get; set; } = null!;

    public string NomeArmazenado { get; set; } = null!;

    public string TipoConteudo { get; set; } = null!;

    public int? IdBoletim { get; set; }

    public int? IdAnaliseDetalhe { get; set; }

    public virtual AnaliseDetalhe? IdAnaliseDetalheNavigation { get; set; }

    public virtual Boletim? IdBoletimNavigation { get; set; }
}
