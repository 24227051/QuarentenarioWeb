using System;
using System.Collections.Generic;

namespace QuarentenarioWeb.Models;

public partial class Material
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public virtual ICollection<Analise> Analises { get; set; } = new List<Analise>();

    public virtual ICollection<Patogeno> IdPatogenos { get; set; } = new List<Patogeno>();
}
