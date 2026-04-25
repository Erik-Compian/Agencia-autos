using System;
using System.Collections.Generic;

namespace AgenciaAutosMVC.Models;

public partial class Refaccion
{
    public int IdRefaccion { get; set; }

    public string Nombre { get; set; } = null!;

    public string Codigo { get; set; } = null!;

    public decimal Precio { get; set; }

    public int Stock { get; set; }

    public virtual ICollection<ServicioRefaccion> ServicioRefaccions { get; set; } = new List<ServicioRefaccion>();
}
