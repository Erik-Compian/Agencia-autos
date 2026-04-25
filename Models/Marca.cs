using System;
using System.Collections.Generic;

namespace AgenciaAutosMVC.Models;

public partial class Marca
{
    public int IdMarca { get; set; }

    public string Nombre { get; set; } = null!;

    public virtual ICollection<Modelo> Modelos { get; set; } = new List<Modelo>();
}
