using System;
using System.Collections.Generic;

namespace AgenciaAutosMVC.Models;

public partial class Administrador
{
    public int IdAdmin { get; set; }

    public string Usuario { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string Email { get; set; } = null!;

    public virtual ICollection<Servicio> Servicios { get; set; } = new List<Servicio>();
}
