using System;
using System.Collections.Generic;

namespace QuarentenarioWeb.Models;

public partial class TipoControle
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public virtual ICollection<Patogeno> Patogenos { get; set; } = new List<Patogeno>();
}
