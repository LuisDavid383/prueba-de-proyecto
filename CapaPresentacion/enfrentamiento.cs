using System;
using System.Collections.Generic;
using System.Text;

namespace CapaPresentacion
{
    public class enfrentamiento
    {
        public int IDEnfrentamiento { get; set; }

        // IDs que necesitas para guardar en BD
        public int IDLocal { get; set; }
        public int IDVisitante { get; set; }

        // Nombres solo para mostrar en la tabla
        public string LocalNombre { get; set; }
        public string VisitanteNombre { get; set; }

        public DateTime FechaPartido { get; set; }
        public string Sede { get; set; }
        public string EstadoPartido { get; set; }
    }
}
