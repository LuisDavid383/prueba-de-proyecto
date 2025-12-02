using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class clsGestionEquipoCD
    {
        public void mtdInsertarEquipoCD(int idCreador, string nombre, int idDeporte, string descripcion)
        {
            using (SqlConnection cn = clsConexion.mtdObtenerConexion())
            {
                cn.Open();

                SqlCommand cmd = new SqlCommand("sp_Equipo_Insertar", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IDCreador", idCreador);
                cmd.Parameters.AddWithValue("@Nombre", nombre);
                cmd.Parameters.AddWithValue("@IDDeporte", idDeporte);

                if (string.IsNullOrWhiteSpace(descripcion))
                    cmd.Parameters.AddWithValue("@Descripcion", DBNull.Value);
                else
                    cmd.Parameters.AddWithValue("@Descripcion", descripcion);

                cmd.ExecuteNonQuery();
            }
        }

        public DataTable mtdCListarDeportesActivosCD()
        {
            DataTable dt = new DataTable();

            string query = @"SELECT IDDeporte, Deporte 
                             FROM tbDeporte
                             WHERE Estado = 1 ";

            using (SqlConnection connection = clsConexion.mtdObtenerConexion())
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(dt);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al listar los deportes desde la BD (T-SQL directo). Detalle: " + ex.Message);
                }
            }
            return dt;
        }

        public DataTable mtdListarEquiposActivosCD(int idUsuario)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conexion = clsConexion.mtdObtenerConexion())
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_Equipo_ListarActivosPorUsuario", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IDUsuario", idUsuario);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
                catch (Exception ex)
                {
                    throw new Exception("Error al listar equipos: " + ex.Message);
                }
            }

            return dt;
        }

        public void mtdEliminarLogicoEquipoCD(int idEquipo)
        {
            using (SqlConnection conexion = clsConexion.mtdObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_Equipo_EliminarLogico", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IDEquipo", idEquipo);

                conexion.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void mtdModificarEquipoCD(int idEquipo, string nombre, string descripcion)
        {
            using (SqlConnection conexion = clsConexion.mtdObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_ModificarEquipo", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IDEquipo", idEquipo);
                cmd.Parameters.AddWithValue("@NombreEquipo", nombre);
                cmd.Parameters.AddWithValue("@Descripcion", descripcion);

                conexion.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public DataTable mtdListarIntegrantesCD(int idEquipo)
        {
            DataTable dt = new DataTable();
            using (SqlConnection conexion = clsConexion.mtdObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_Equipo_ListarIntegrantes", conexion);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@IDEquipo", idEquipo);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
            return dt;
        }

        public void mtdInsertarInvitacionCD(int idEquipo, int idUsuarioEmisor, int idUsuarioReceptor)
        {
            using (SqlConnection conexion = clsConexion.mtdObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_InvitacionEquipo_Insertar", conexion);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IDEquipo", idEquipo);
                cmd.Parameters.AddWithValue("@IDUsuarioEmisor", idUsuarioEmisor);
                cmd.Parameters.AddWithValue("@IDUsuarioReceptor", idUsuarioReceptor);

                conexion.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public DataTable mtdListarUsuariosParaInvitacionCD(string busqueda = null)
        {
            DataTable dt = new DataTable();

            using (SqlConnection conexion = clsConexion.mtdObtenerConexion())
            {
                try
                {
                    SqlCommand cmd = new SqlCommand("sp_ListarUsuariosParaInvitacion", conexion);
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Parámetro opcional
                    if (!string.IsNullOrEmpty(busqueda))
                        cmd.Parameters.AddWithValue("@Busqueda", busqueda);
                    else
                        cmd.Parameters.AddWithValue("@Busqueda", DBNull.Value);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
                catch (Exception ex)
                {
                    // Manejar error si se desea
                    throw new Exception("Error al listar usuarios: " + ex.Message);
                }
            }

            return dt;
        }

        public DataTable mtdListarInvitacionesCD(int idUsuario)
        {
            DataTable tabla = new DataTable();

            using (SqlConnection cn = clsConexion.mtdObtenerConexion())
            {
                SqlCommand cmd = new SqlCommand("sp_InvitacionEquipo_Listar", cn);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@IDUsuario", idUsuario);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tabla);
            }

            return tabla;
        }

        public void mtdAceptarInvitacionCD(int idInvitacion, int idUsuarioReceptor)
        {
            using (SqlConnection cn = clsConexion.mtdObtenerConexion())
            {
                cn.Open();

                using (SqlCommand cmd = new SqlCommand("sp_AceptarInvitacionEquipo", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IDInvitacion", idInvitacion);
                    cmd.Parameters.AddWithValue("@IDUsuarioReceptor", idUsuarioReceptor);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void mtdRechazarInvitacionCD(int idInvitacion, int idUsuarioReceptor)
        {
            using (SqlConnection cn = clsConexion.mtdObtenerConexion())
            {
                cn.Open();

                using (SqlCommand cmd = new SqlCommand("sp_RechazarInvitacionEquipo", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IDInvitacion", idInvitacion);
                    cmd.Parameters.AddWithValue("@IDUsuarioReceptor", idUsuarioReceptor);

                    cmd.ExecuteNonQuery();
                }
            }
        }
    }
}
