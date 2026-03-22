using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace QuarentenarioWeb.Models;

public partial class Anexo
{
    public int Id { get; set; }

    [Display(Name = "Nome Arquivo")]
    public string NomeArquivo { get; set; } = null!;

    [Display(Name = "Nome Armazenado")]
    public string NomeArmazenado { get; set; } = null!;

    [Display(Name = "Tipo Conteudo")]
    public string TipoConteudo { get; set; } = null!;

    [Display(Name = "Id Análise")]
    public int? IdAnalise { get; set; }

    [Display(Name = "Id Controle")]
    public int? IdAnaliseDetalhe { get; set; }

    [Display(Name = "Controle")]
    public virtual AnaliseDetalhe? IdAnaliseDetalheNavigation { get; set; }

    [Display(Name = "Análise")]
    public virtual Analise? IdAnaliseNavigation { get; set; }
}
