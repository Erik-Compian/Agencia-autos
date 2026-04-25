using System;
using System.Collections.Generic;

namespace AgenciaAutosMVC.Models;

public partial class Vehiculo
{
    public int IdVehiculo { get; set; }

    public int IdCliente { get; set; }

    public int IdModelo { get; set; }

    public string NumSerie { get; set; } = null!;

    public string? Color { get; set; }

    public string Placa { get; set; } = null!;

    public int KmActual { get; set; }

    public virtual Cliente IdClienteNavigation { get; set; } = null!;

    public virtual Modelo IdModeloNavigation { get; set; } = null!;

    public virtual ICollection<Servicio> Servicios { get; set; } = new List<Servicio>();
}
