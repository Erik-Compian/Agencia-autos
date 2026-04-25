using System;
using System.Collections.Generic;

namespace AgenciaAutosMVC.Models;

public partial class ProximoServicio
{
    public int IdProxServ { get; set; }

    public int Folio { get; set; }

    public DateOnly FechaProg { get; set; }

    public int? KmProximo { get; set; }

    public string? Notas { get; set; }

    public virtual Servicio FolioNavigation { get; set; } = null!;
}
