using System;
using System.Collections.Generic;

namespace QuarentenarioWeb.Models;

public partial class Importacao
{
    public string Material { get; set; } = null!;

    public string Patogeno { get; set; } = null!;

    public string TipoControle { get; set; } = null!;

    public string TipoPatageno { get; set; } = null!;
}
