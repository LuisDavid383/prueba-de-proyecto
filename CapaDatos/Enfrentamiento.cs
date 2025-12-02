using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class Enfrentamiento
    {
        public int IDEnfrentamiento { get; set; }
        public string LocalNombre { get; set; }
        public string VisitanteNombre { get; set; }
        public DateTime FechaPartido { get; set; }
        public string EstadoPartido { get; set; }
        public int IDTorneo { get; set; } // opcional
    }
}
