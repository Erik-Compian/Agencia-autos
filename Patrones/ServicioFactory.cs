using Microsoft.AspNetCore.Mvc;
using AgenciaAutosMVC.Models; 
using System;


namespace AgenciaAutosMVC.Patrones
{
    public class ServicioFactory
    {
        // 1. Agregamos el parámetro "quienEntrego" al final
        public static Servicio CrearServicioNuevo(string tipoMantenimiento, int idVehiculo, int idAdmin, string quienEntrego)
        {
            Servicio nuevoServicio = new Servicio
            {
                IdVehiculo = idVehiculo,
                IdAdmin = idAdmin,
                FechaIngreso = DateOnly.FromDateTime(DateTime.Now),
                Estatus = "En espera",
                QuienEntrego = quienEntrego // 2. Se lo asignamos al objeto
            };

            // ... (Lo de Preventivo y Correctivo se queda exactamente igual)
            if (tipoMantenimiento == "Preventivo")
            {
                nuevoServicio.IdTipoServ = 1;
                nuevoServicio.Descripcion = "Revisión general de rutina.";
            }
            else if (tipoMantenimiento == "Correctivo")
            {
                nuevoServicio.IdTipoServ = 2;
                nuevoServicio.Descripcion = "Requiere diagnóstico por falla.";
            }

            return nuevoServicio;
        }
    }
}