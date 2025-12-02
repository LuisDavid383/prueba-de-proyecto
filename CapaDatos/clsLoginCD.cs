using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class clsLoginCD
    {
        public DataTable mtdLoginCD(string dato)
        {
            DataTable tabla = new DataTable();

            using (SqlConnection cn = clsConexion.mtdObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_LoginUsuario", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Usuario", (object)dato ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Correo", (object)dato ?? DBNull.Value);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tabla);
            }

            return tabla;
        }

        public void mtdRegistrarUsuarioCD(string Nombres, string ApellidoPaterno, string ApellidoMaterno,
                                     int IDTipoDocumento, string Documento, DateTime FechaNacimiento,
                                     string Telefono, string Correo, string Genero,
                                     string NombreUsuario, string ContraseñaHash)
        {
            using (SqlConnection cn = clsConexion.mtdObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_RegistrarUsuario", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                // Persona
                cmd.Parameters.AddWithValue("@Nombres", Nombres);
                cmd.Parameters.AddWithValue("@ApellidoPaterno", ApellidoPaterno);
                cmd.Parameters.AddWithValue("@ApellidoMaterno", ApellidoMaterno);
                cmd.Parameters.AddWithValue("@IDTipoDocumento", IDTipoDocumento);
                cmd.Parameters.AddWithValue("@Documento", Documento);
                cmd.Parameters.AddWithValue("@FechaNacimiento", FechaNacimiento);
                cmd.Parameters.AddWithValue("@Telefono", (object)Telefono ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Correo", Correo);
                cmd.Parameters.AddWithValue("@Genero", Genero);

                // Usuario
                cmd.Parameters.AddWithValue("@NombreUsuario", NombreUsuario);
                cmd.Parameters.AddWithValue("@ContraseñaHash", ContraseñaHash);

                cn.Open();
                cmd.ExecuteNonQuery();   // Solo inserta
            }
        }

        public DataTable ListarActivosDT()
        {
            DataTable dt = new DataTable();

            using (SqlConnection con = clsConexion.mtdObtenerConexion())
            {
                con.Open();

                string query = @"SELECT IDTipoDocumento, TipoDocumento
                             FROM tbTipoDocumento
                             WHERE Estado = 1";

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }

            return dt;
        }

    }
}
