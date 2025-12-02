using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class clsConexion
    {
        private static readonly string connectionString = @"server = David; 
                                                            database= BD_SIS_DEPORTE; 
                                                            integrated security = true";

        public static SqlConnection mtdObtenerConexion()
        {
            return new SqlConnection(connectionString);
        }
    }
}
