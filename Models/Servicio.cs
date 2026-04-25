using System;
using System.Collections.Generic;

namespace AgenciaAutosMVC.Models;

public partial class Servicio
{
    public int Folio { get; set; }

    public int IdVehiculo { get; set; }

    public int IdAdmin { get; set; }

    public int IdTipoServ { get; set; }

    public string QuienEntrego { get; set; } = null!;

    public DateOnly FechaIngreso { get; set; }

    public DateOnly? FechaSalida { get; set; }

    public string Estatus { get; set; } = null!;

    public string? Descripcion { get; set; }

    public virtual Administrador IdAdminNavigation { get; set; } = null!;

    public virtual TipoServicio IdTipoServNavigation { get; set; } = null!;

    public virtual Vehiculo IdVehiculoNavigation { get; set; } = null!;

    public virtual ProximoServicio? ProximoServicio { get; set; }

    public virtual ICollection<ServicioRefaccion> ServicioRefaccions { get; set; } = new List<ServicioRefaccion>();
}
