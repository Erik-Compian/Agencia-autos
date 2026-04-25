using Microsoft.AspNetCore.Mvc;

namespace AgenciaAutosMVC.Patrones 
{
    public class AgenciaManager
    {
        private static AgenciaManager _instancia;

        public string NombreAgencia { get; private set; }
        public string Ubicacion { get; private set; }

        private AgenciaManager()
        {
            NombreAgencia = "Agencia Automotriz";
            Ubicacion = "Sucursal Saltillo";
        }

        public static AgenciaManager GetInstancia()
        {
            if (_instancia == null)
            {
                _instancia = new AgenciaManager();
            }
            return _instancia;
        }
    }
}