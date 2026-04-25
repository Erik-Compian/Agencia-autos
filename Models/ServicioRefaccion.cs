using System;
using System.Collections.Generic;

namespace AgenciaAutosMVC.Models;

public partial class ServicioRefaccion
{
    public int Folio { get; set; }

    public int IdRefaccion { get; set; }

    public int Cantidad { get; set; }

    public decimal PrecioAplicado { get; set; }

    public virtual Servicio FolioNavigation { get; set; } = null!;

    public virtual Refaccion IdRefaccionNavigation { get; set; } = null!;
}
