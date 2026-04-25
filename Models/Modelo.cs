using System;
using System.Collections.Generic;

namespace AgenciaAutosMVC.Models;

public partial class Modelo
{
    public int IdModelo { get; set; }

    public int IdMarca { get; set; }

    public string Nombre { get; set; } = null!;

    public short Anio { get; set; }

    public virtual Marca IdMarcaNavigation { get; set; } = null!;

    public virtual ICollection<Vehiculo> Vehiculos { get; set; } = new List<Vehiculo>();
}
