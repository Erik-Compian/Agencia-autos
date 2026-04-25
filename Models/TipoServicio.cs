using System;
using System.Collections.Generic;

namespace AgenciaAutosMVC.Models;

public partial class TipoServicio
{
    public int IdTipoServ { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }
    public decimal Precio { get; set; }

    public virtual ICollection<Servicio> Servicios { get; set; } = new List<Servicio>();
}