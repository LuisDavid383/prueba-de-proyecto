using System;
using System.Collections.Generic;
using System.Text;

namespace CapaPresentacion
{
    public static class clsDatosUsuario
    {
        public static int IDUsuario { get; set; }
        public static string NombreUsuario { get; set; }

        public static void CerrarSesion()
        {
            IDUsuario = 0;
            NombreUsuario = null;
        }
    }
}
