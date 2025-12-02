using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaDatos
{
    public class clsGestionTorneoCD
    {
        public void mtdInsertarTorneoCD(int idCreador, string nombre, string descripcion)
        {
            using (SqlConnection cn = clsConexion.mtdObtenerConexion())
            {
                cn.Open();

                using (SqlCommand cmd = new SqlCommand("sp_Torneo_Insertar", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IDCreador", idCreador);
                    cmd.Parameters.AddWithValue("@Nombre", nombre);
                    cmd.Parameters.AddWithValue("@Descripcion", descripcion);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public DataTable mtdListarTorneosActivosPorUsuarioCD(int idUsuario)
        {
            DataTable dt = new DataTable();

            using (SqlConnection cn = clsConexion.mtdObtenerConexion())
            {
                cn.Open();

                using (SqlCommand cmd = new SqlCommand("sp_Torneos_ListarActivosPorUsuario", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IDUsuario", idUsuario);

                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }
            }

            return dt;
        }

        public void mtdModificarTorneoCD(int idTorneo, int idUsuario, string nombre, string descripcion)
        {
            using (SqlConnection cn = clsConexion.mtdObtenerConexion())
            {
                cn.Open();

                using (SqlCommand cmd = new SqlCommand("sp_ModificarTorneo", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IDTorneos", idTorneo);
                    cmd.Parameters.AddWithValue("@IDCreador", idUsuario);
                    cmd.Parameters.AddWithValue("@Nombre", nombre);
                    cmd.Parameters.AddWithValue("@Descripcion", descripcion);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public void mtdEliminarTorneoCD(int idTorneo, int idUsuario)
        {
            using (SqlConnection cn = clsConexion.mtdObtenerConexion())
            {
                cn.Open();

                using (SqlCommand cmd = new SqlCommand("sp_Torneo_EliminarLogico", cn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IDTorneo", idTorneo);
                    cmd.Parameters.AddWithValue("@IDUsuario", idUsuario);

                    cmd.ExecuteNonQuery();
                }
            }
        }

        public DataTable mtdBuscarEquipoPorNombreCD(string nombre)
        {
            DataTable dt = new DataTable();

            using (SqlConnection cn = clsConexion.mtdObtenerConexion())
            using (SqlCommand cmd = new SqlCommand("spBuscarEquiposPorNombre", cn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@Nombre", nombre);

                SqlDataAdapter da = new SqlDataAdapter(cmd);

                cn.Open();   // <- necesario
                da.Fill(dt); // <- tu línea que está fallando
            }

            return dt;
        }

        public string InvitarEquipoATorneo(int idTorneo, int idEquipo, int idUsuarioEmisor)
        {
            try
            {
                using (SqlConnection conn = clsConexion.mtdObtenerConexion())
                {
                    SqlCommand cmd = new SqlCommand("spInvitarEquipoATorneo", conn);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IDTorneo", idTorneo);
                    cmd.Parameters.AddWithValue("@IDEquipo", idEquipo);
                    cmd.Parameters.AddWithValue("@IDUsuarioEmisor", idUsuarioEmisor);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                return "OK";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        public DataTable ListarInvitaciones(int idUsuario)
        {
            DataTable dt = new DataTable();

            try
            {
                using (SqlConnection conn = clsConexion.mtdObtenerConexion())
                {
                    SqlCommand cmd = new SqlCommand("spListarInvitacionesDeEquipos", conn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@IDUsuario", idUsuario);

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Error al listar invitaciones: " + ex.Message);
            }

            return dt;
        }


    }
}
