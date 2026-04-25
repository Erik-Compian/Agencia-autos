namespace AgenciaAutosMVC.Services
{
    public class ConfiguradorAgencia
    {
        // 1. La instancia única (estática)
        private static ConfiguradorAgencia? _instancia;

        // Propiedades de la Agencia
        public string NombreAgencia { get; set; } = "Agencia Automotriz Saltillo";
        public string Telefono { get; set; } = "844-123-4567";
        public string Direccion { get; set; } = "Blvd. V. Carranza #123";
        public decimal IVA { get; set; } = 0.16m;

        // 2. Constructor PRIVADO (nadie fuera de aquí puede hacer un "new")
        private ConfiguradorAgencia() { }

        // 3. Método para obtener la instancia (si no existe, la crea)
        public static ConfiguradorAgencia Instancia
        {
            get
            {
                if (_instancia == null)
                {
                    _instancia = new ConfiguradorAgencia();
                }
                return _instancia;
            }
        }
    }
}