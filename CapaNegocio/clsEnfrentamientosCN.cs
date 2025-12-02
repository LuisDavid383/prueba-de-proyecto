using CapaDatos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaNegocio
{
    public static class clsEnfrentamientosCN
    {
        private static clsGestionTorneoCD objEnfr = new clsGestionTorneoCD();

        public static List<Enfrentamiento> ListarPorTorneo(int idTorneo)
        {
            return objEnfr.ListarPorTorneo(idTorneo);
        }
    }
}
