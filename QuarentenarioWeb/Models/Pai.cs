using System;
using System.Collections.Generic;

namespace QuarentenarioWeb.Models;

public partial class Pai
{
    public int Id { get; set; }

    public string Nome { get; set; } = null!;

    public virtual ICollection<Boletim> Boletims { get; set; } = new List<Boletim>();
}
